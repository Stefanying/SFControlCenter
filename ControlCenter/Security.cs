using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace ControlCenter
{
    public class Sercurity
    {
        public static string TripleDESCrypto(string str, string key)
        {
            byte[] data = UnicodeEncoding.Unicode.GetBytes(str);
            byte[] keys = ASCIIEncoding.ASCII.GetBytes(key);

            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Key = keys;
            des.Mode = CipherMode.ECB;
            ICryptoTransform cryp = des.CreateEncryptor();
            return Convert.ToBase64String(cryp.TransformFinalBlock(data, 0, data.Length));
        }

        public static string TripleDESCryptoDe(string str, string key)
        {
            byte[] data = Convert.FromBase64String(str);
            byte[] keys = ASCIIEncoding.ASCII.GetBytes(key);

            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Key = keys;
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.PKCS7;
            ICryptoTransform cryp = des.CreateDecryptor();

            return UnicodeEncoding.Unicode.GetString(cryp.TransformFinalBlock(data, 0, data.Length));
        }
    }
}
