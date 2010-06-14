using System;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Nohros.Net.Security
{
    public sealed class NSecurity
    {
        public static string GetStringHash(string s) {
            return Nohros.NSecurity.GetStringHash(s);
        }

        public static NCryptoParms EncryptCode(string decData, string spKey) {
            return Nohros.NSecurity.EncryptCode(decData, spKey);
        }

        public static string DecryptCode(string encData, string encIV, string encKey, string jpbkey, string sprkey, string signature) {
            return Nohros.NSecurity.DecryptCode(encData, encIV, encKey, jpbkey, sprkey, signature);
        }

        public static string BasicCryptoString(string dec) {
            return Nohros.NSecurity.BasicCryptoString(dec);
        }

        public static string BasicDeCryptoString(string enc) {
            return Nohros.NSecurity.BasicDeCryptoString(enc);
        }
    }
}