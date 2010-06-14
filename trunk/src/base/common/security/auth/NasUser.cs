using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Security.Auth
{
    public struct NasUser
    {
        int _id;
        string _username;
        string _password;
        string _name;

        #region .ctor
        public static readonly NasUser INVALID;
        static NasUser()
        {
            INVALID = new NasUser(-1, string.Empty, string.Empty);
        }

        public NasUser(int id, string username, string password):this(id, string.Empty, password, string.Empty)
        {
        }

        public NasUser(int id, string username, string password, string name)
        {
            _id = id;
            _username = username;
            _password = password;
            _name = name;
        }
        #endregion

        /// <summary>
        /// Gets or ets a value that uniquely identifies the user within an application
        /// </summary>
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Gets or sets the user name
        /// </summary>
        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }

        /// <summary>
        /// Gets or sets the password of the user
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// Gets or sets the name of the user
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public override int GetHashCode()
        {
            return _id;
        }

        public override bool Equals(object obj)
        {
            return (obj is NasUser && ((NasUser)obj)._id == _id);
        }

        public static bool operator!=(NasUser u1, NasUser u2)
        {
            return (u1._id != u2._id);
        }

        public static bool operator ==(NasUser u1, NasUser u2)
        {
            return u1._id == u2._id;
        }

        public bool IsInvalid
        {
            get
            {
                return _id == -1;
            }
        }
    }
}