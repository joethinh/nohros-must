using System;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Nohros
{
    public class NCryptoParms
    {
        public string iv;
        public string key;
        public string signature;
        public string jprkey;
        public string jpbkey;
        public string enc;
    }

    /// <summary>
    /// Generic static security class.
    /// </summary>
    /// <remarks>
    /// This class resides on the Nohros namespace for legacy compatibility.
    /// </remarks>
    public sealed class NSecurity
    {
        private static readonly byte[] sign = new byte[] { 77, 105, 114, 97, 99, 108, 101, 83, 105, 103, 110, 68, 97, 116, 97 };

        public static string GetStringHash(string s)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            StringBuilder sb = new StringBuilder();

            byte[] data = Encoding.ASCII.GetBytes(s);
            byte[] hash = provider.ComputeHash(data);

            foreach (byte hex in hash)
            {
                sb.Append(hex.ToString("X2"));
            }
            return sb.ToString();
        }

        public static NCryptoParms EncryptCode(string decData, string spKey)
        {
            NCryptoParms parms = new NCryptoParms();
            ICryptoTransform encryptor;
            CryptoStream cStream;
            MemoryStream mStream = new MemoryStream();

            try
            {
                // Generate a new RSA public/private key pair
                // This key will be used to signature the DES IV and Key.
                RSACryptoServiceProvider jRsa = new RSACryptoServiceProvider();

                byte[] signature = jRsa.SignData(sign, new MD5CryptoServiceProvider());

                parms.jpbkey = Convert.ToBase64String(Encoding.ASCII.GetBytes(jRsa.ToXmlString(false)));
                parms.jprkey = Convert.ToBase64String(Encoding.ASCII.GetBytes(jRsa.ToXmlString(true)));
                parms.signature = Convert.ToBase64String(signature);
                jRsa.Clear();

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(Encoding.ASCII.GetString(Convert.FromBase64String(spKey)));

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.GenerateIV();
                des.GenerateKey();

                parms.key = Convert.ToBase64String(rsa.Encrypt(des.Key, false));
                parms.iv = Convert.ToBase64String(rsa.Encrypt(des.IV, false));

                encryptor = des.CreateEncryptor(des.Key, des.IV);
                cStream = new CryptoStream(mStream, encryptor, CryptoStreamMode.Write);

                byte[] bytesIn = Encoding.ASCII.GetBytes(decData);

                cStream.Write(bytesIn, 0, bytesIn.Length);
                cStream.FlushFinalBlock();
                cStream.Close();

                byte[] bytesOut = mStream.ToArray();
                mStream.Close();

                parms.enc = Convert.ToBase64String(bytesOut);
            }
            catch { mStream.Close(); }
            return parms;
        }

        public static string DecryptCode(string encData, string encIV, string encKey, string jpbkey, string sprkey, string signature)
        {
            string decData = null;

            try
            {
                // Check if the signature is valid.
                RSACryptoServiceProvider jRsa = new RSACryptoServiceProvider();
                jRsa.FromXmlString(Encoding.ASCII.GetString(Convert.FromBase64String(jpbkey)));

                bool leg = jRsa.VerifyData(sign, new MD5CryptoServiceProvider(), Convert.FromBase64String(signature));
                jRsa.Clear();

                if (leg)
                {
                    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                    rsa.FromXmlString(Encoding.ASCII.GetString(Convert.FromBase64String(sprkey)));

                    byte[] iv = rsa.Decrypt(Convert.FromBase64String(encIV), false);
                    byte[] key = rsa.Decrypt(Convert.FromBase64String(encKey), false);
                    byte[] data = Convert.FromBase64String(encData);

                    MemoryStream mStream = new MemoryStream(data);

                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                    ICryptoTransform decryptor = des.CreateDecryptor(key, iv);

                    CryptoStream cStream = new CryptoStream(mStream, decryptor, CryptoStreamMode.Read);

                    using (StreamReader reader = new StreamReader(cStream)) {
                        decData = reader.ReadToEnd();
                    }

                    mStream.Close();
                    cStream.Close();
                }
            }
            catch { decData = string.Empty; }
            return decData;
        }

        /// <summary>
        /// Provides a very basic mechanism to encrypt strings.
        /// </summary>
        /// <param name="dec">The decrypted string to encrypt</param>
        /// <returns>The encrypted form of the <paramref name="dec"/> string.</returns>
        /// <remarks>
        /// Strings encrypted with the <see cref="BasicCryptoString(string)"/> are not really secure. Its is
        /// used only to provide a very basic security mechanism. Anyone with with basic knowledge of programing
        /// could decrypt strings encrypted with this method. Do not use them to protect sensitive data.
        /// </remarks>
        public static string BasicCryptoString(string dec)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(dec);
            for (int i = 0, j = bytes.Length; i < j; i++)
                bytes[i] = (byte)RotateLeft(bytes[i], 1);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Decrypts a string encrypted with the <see cref="BasicCryptoString(string)"/> method.
        /// </summary>
        /// <param name="enc">A string encrypted with the to <see cref="BasicCryptoString(string)"/> method.</param>
        /// <returns>The encrypted form of the <paramref name="enc"/>string.</returns>
        /// <remarks>
        /// Strings encrypted with the <see cref="BasicCryptoString(string)"/> are not really secure. Its is
        /// used only to provide a very basic security mechanism. Anyone with with basic knowledge of programing
        /// could decrypt strings encrypted with this method. Do not use them to protect sensitive data.
        /// </remarks>
        /// <seealso cref="BasicDeCryptoString(string)"/>
        public static string BasicDeCryptoString(string enc)
        {
            byte[] bytes = Convert.FromBase64String(enc);
            for (int i = 0, j = bytes.Length; i < j; i++)
                bytes[i] = (byte)RotateRight(bytes[i], 1);
            return Encoding.ASCII.GetString(bytes);
        }

        /// <summary>
        /// Implements the left circular shift.
        /// </summary>
        /// <param name="x">The value to be shifted</param>
        /// <param name="n">The number of bits to rotate</param>
        private static UInt32 RotateLeft(UInt32 x, Byte n)
        {
            return (UInt32)(((x) << (n)) | ((x) >> (32 - (n))));
        }

        /// <summary>
        /// Implements teh right circular shift
        /// </summary>
        /// <param name="x">The value to be shifted</param>
        /// <param name="n">The number of bits to rotate</param>
        private static UInt32 RotateRight(UInt32 x, Byte n)
        {
            return (UInt32)(((x) >> (n)) | ((x) >> (32 - (n))));
        }
    }
}