using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Core.Utilities
{
    public static class Hashing
    {

        // set permutations
        public const String StrPermutation = "asdasdasdasd";
        public const Int32 BytePermutation1 = 0x19;
        public const Int32 BytePermutation2 = 0x59;
        public const Int32 BytePermutation3 = 0x17;
        public const Int32 BytePermutation4 = 0x41;
        // decoding


        // encoding
        public static string Encrypt(string strData)
        {

            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(strData)));
            // reference https://msdn.microsoft.com/en-us/library/ds4kkd55(v=vs.110).aspx

        }


        // decoding
        public static string Decrypt(string strData)
        {
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(strData)));
            // reference https://msdn.microsoft.com/en-us/library/system.convert.frombase64string(v=vs.110).aspx

        }

        // encrypt
        public static byte[] Encrypt(byte[] strData)
        {
            PasswordDeriveBytes passbytes =
            new PasswordDeriveBytes(StrPermutation,
            new byte[] { BytePermutation1,
                         BytePermutation2,
                         BytePermutation3,
                         BytePermutation4
            });

            MemoryStream memstream = new MemoryStream();
            Aes aes = new AesManaged() { KeySize = 256 };
            aes.Key = passbytes.GetBytes(aes.KeySize / 8);
            aes.IV = passbytes.GetBytes(aes.BlockSize / 8);

            CryptoStream cryptostream = new CryptoStream(memstream,
            aes.CreateEncryptor(), CryptoStreamMode.Write);
            cryptostream.Write(strData, 0, strData.Length);
            cryptostream.Close();
            return memstream.ToArray();
        }

        // decrypt
        public static byte[] Decrypt(byte[] strData)
        {
            PasswordDeriveBytes passbytes =
            new PasswordDeriveBytes(StrPermutation,
            new byte[] { BytePermutation1,
                         BytePermutation2,
                         BytePermutation3,
                         BytePermutation4
            });

            MemoryStream memstream = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = passbytes.GetBytes(aes.KeySize / 8);
            aes.IV = passbytes.GetBytes(aes.BlockSize / 8);

            CryptoStream cryptostream = new CryptoStream(memstream,
            aes.CreateDecryptor(), CryptoStreamMode.Write);
            cryptostream.Write(strData, 0, strData.Length);
            cryptostream.Close();
            return memstream.ToArray();
        }

    }
}
