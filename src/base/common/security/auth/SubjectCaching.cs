using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.Data.SQLite;
using System.Data;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using Nohros.Data;

namespace Nohros.Security.Auth
{
    public partial class Subject
    {
        static Dictionary<string, CacheEntry> cache;

        static Subject()
        {
            cache = new Dictionary<string, CacheEntry>();
            SQLite.EnsureSQLite();
        }

        /// <summary>
        /// Adds the specified <see cref="Subject"/> to the internal cache with expiration.
        /// </summary>
        internal static void Add(Subject subject)
        {
            CacheEntry entry = new CacheEntry(subject.ID, subject, DateTime.MaxValue, TimeSpan.Zero);
            cache[entry.Key] = entry;

            ThreadPool.QueueUserWorkItem(delegate(object o)
            {
                CacheEntry e = (CacheEntry)o;

                long[] ids; int i = 0;
                MemoryStream inMemory = null;
                SQLiteConnection conn = null;
                SQLiteTransaction tran = null;
                try
                {
                    conn = new SQLiteConnection("Data Source="+SQLite.path+";Version=3;Pooling=True;Max Pool Size=100;");

                    // store the subjet
                    SQLiteCommand cmd = new SQLiteCommand(conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO nas_subject(registry_key, utcexpires, utccreated) values(@registry_key, @utcexpires, @utccreated);SELECT last_insert_rowid();";

                    cmd.Parameters.Add("@registry_key", DbType.String, 100).Value = e.Key;
                    cmd.Parameters.Add("@utcexpires", DbType.DateTime).Value = e.UtcExpires;
                    cmd.Parameters.Add("@utccreated", DbType.DateTime).Value = e.UtcCreated;

                    conn.Open();

                    tran = conn.BeginTransaction();

                    ids = new long[subject.Permissions.Count + 1];
                    ids[i++] = (long)cmd.ExecuteScalar();

                    cmd.Parameters.Clear();

                    inMemory = new MemoryStream();
                    BinaryFormatter formatter = new BinaryFormatter();

                    // store the subject's permissions
                    cmd.CommandText = "INSERT INTO nas_permission(type, name, mask, binary) values(@type, @name, @mask, @binary);SELECT last_insert_rowid();";
                    cmd.Parameters.Add("@type", DbType.String, 300);
                    cmd.Parameters.Add("@name", DbType.String, 150);
                    cmd.Parameters.Add("@mask", DbType.Int64);
                    cmd.Parameters.Add("@binary", DbType.Binary);

                    IList<IPermission> perms = subject.Permissions;
                    foreach (IPermission perm in perms)
                    {
                        cmd.Parameters[0].Value = perm.GetType().ToString();
                        cmd.Parameters[1].Value = perm.Name;
                        cmd.Parameters[2].Value = perm.Mask;

                        // permission serializing
                        inMemory.Position = 0;
                        formatter.Serialize(inMemory, perm);

                        cmd.Parameters[3].Value = inMemory.ToArray();

                        ids[i++] = (long)cmd.ExecuteScalar();
                    }

                    // links the permissions to the subject
                    cmd.Parameters.Clear();
                    cmd.CommandText = "INSERT INTO nas_subjectpermission(subjectid, permissionid) VALUES(@subjectid, @permissionid)";

                    cmd.Parameters.Add("@subjectid", DbType.Int32).Value = ids[0];
                    cmd.Parameters.Add("@permissionid", DbType.Int32);
                    while (--i > 0)
                    {
                        cmd.Parameters[1].Value = ids[i];
                        cmd.ExecuteNonQuery();
                    }

                    tran.Commit();

                    conn.Close();
                }
#if DEBUG
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);

                    if (conn != null && conn.State == ConnectionState.Open)
                        tran.Rollback();
                }
#else
                catch
                {
                    if (conn != null && conn.State == ConnectionState.Open)
                        tran.Rollback();
                }
#endif
                finally
                {
                    if (conn != null)
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }

                    if (inMemory != null)
                        inMemory.Dispose();
                }
            }, entry);
        }

        /// <summary>
        /// Retrieves the specified <see cref="Subject"/> from the internal cache object.
        /// </summary>
        /// <param name="id">The identifier for the cached <see cref="Subject"/> to retrieve.</param>
        /// <returns>The retrieved <see cref="Subject"/>, or null if the id is not found.</returns>
        private static CacheEntry GetCacheEntry(string id)
        {
            CacheEntry entry = null;
            if (!cache.TryGetValue(id, out entry))
            {
                MemoryStream inMemoryData = null;
                BinaryFormatter formatter = null;
                SQLiteConnection conn = null;
                SQLiteDataReader dr = null;
                try
                {
                    conn = new SQLiteConnection("Data Source="+SQLite.path+";Version=3;Pooling=True;Max Pool Size=100;");
                    SQLiteCommand cmd = new SQLiteCommand(conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText =
                        @"SELECT registry_key,
                        utcexpires,
                        utccreated
                   from nas_subject s
                   where registry_key ='" + id + @"';
                   SELECT type,
                        name,
                        mask,
                        binary
                    from nas_subject s inner join
                        nas_subjectpermission sp on sp.subjectid = s.subjectid inner join
                        nas_permission p on p.permissionid = sp.permissionid
                    where registry_key=" + id;

                    conn.Open();

                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    if (dr.Read())
                    {
                        // build the subject and the cache entry
                        Subject subject = new Subject();
                        subject._id = (string)dr["registry_key"];

                        entry = new CacheEntry(subject.ID, subject, (DateTime)dr["utcexpires"], CacheEntry.NoSlidingExpiration);
                        entry._utcCreated = (DateTime)dr["utccreated"];

                        inMemoryData = new MemoryStream();

                        // get the subject's permissions
                        dr.NextResult();
                        while(dr.Read())
                        {
                            byte[] inMemoryBytes = (byte[])dr["binary"];
                            inMemoryData.Position = 0;
                            inMemoryData.Write(inMemoryBytes, 0, inMemoryBytes.Length);
                            
                            // deserialize the permission
                            object perm = formatter.Deserialize(inMemoryData);
                            subject.Permissions.Add((IPermission)perm);
                        }
                    }
                }
#if DEBUG
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);

                    /* logging */
                    entry = null;
                }
#else
                catch
                {
                    /* logging */
                    entry = null;
                }
#endif
                finally
                {
                    if (dr != null)
                        dr.Dispose();

                    if (conn != null)
                    {
                        if (conn.State == ConnectionState.Open)
                            conn.Close();
                        conn.Dispose();
                    }
                }
            }
            return entry;
        }

        /// <summary>
        /// Retrieves the specified <see cref="Subject"/> from the internal cache object.
        /// </summary>
        /// <param name="id">The identifier for the cached <see cref="Subject"/> to retrieve.</param>
        /// <returns>The retrieved <see cref="Subject"/>, or null if the id is not found.</returns>
        internal static Subject Get(string id)
        {
            CacheEntry entry = GetCacheEntry(id);
            return (entry != null && entry.UtcExpires > DateTime.UtcNow) ? (Subject)entry.Value : null;
        }

        /// <summary>
        /// Retrieves the specified <see cref="Subject"/> from the internal cache object and
        /// set the subject's expiration date as equal to the ticket's expiration date.
        /// </summary>
        /// <param name="ticket">The authentication ticket related with the subject</param>
        /// <returns>The retrieved <see cref="Subject"/>, or null if the id was not found.</returns>
        internal static Subject Get(System.Web.Security.FormsAuthenticationTicket ticket)
        {
            CacheEntry entry = GetCacheEntry(ticket.Name);
            if (entry != null)
            {
                // If the ticket was not expired, set the expiration date
                // of the subject to the ticket expiration date.
                if (!ticket.Expired)
                {
                    entry.UtcExpires = ticket.Expiration.ToUniversalTime();
                    return (Subject)entry.Value;
                }
            }
            return null;
        }
    }
}
