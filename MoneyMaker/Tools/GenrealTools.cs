using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Objects;

namespace Toolbox
{
    public class EncryptionTools
    {
        private const int keysize = 256;

        public static string UserEncrypt(string plainText, User user)
        {
            byte[] initVectorBytes = new byte[16];

            for (int ii = 0; ii < initVectorBytes.Length; ii++)
            {
                initVectorBytes[ii] = Convert.ToByte(user.Pass.ElementAt(ii));
            }

            string passPhrase = user.Pass.Remove(32);

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null))
            {
                byte[] keyBytes = password.GetBytes(keysize / 8);
                using (RijndaelManaged symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                byte[] cipherTextBytes = memoryStream.ToArray();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }
        public static SecureString UserDecrypt(string cipherText, User user)
        {
            byte[] initVectorBytes = new byte[16];

            for (int ii = 0; ii < initVectorBytes.Length; ii++)
            {
                initVectorBytes[ii] = Convert.ToByte(user.Pass.ElementAt(ii));
            }

            string passPhrase = user.Pass.Remove(32);

            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            using (PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null))
            {
                byte[] keyBytes = password.GetBytes(keysize / 8);
                using (RijndaelManaged symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
                    {
                        using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                string temp = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                                SecureString ss = new SecureString();
                                foreach (var c in temp)
                                {
                                    ss.AppendChar(c);
                                }
                                temp = null;
                                return ss;
                            }
                        }
                    }
                }
            }
        }

        public static byte[] MSProtect(string rawString, string passHash)
        {
            byte[] data = new byte[rawString.Length];
            for (int ii = 0; ii < rawString.Length; ii++)
            {
                data[ii] = Convert.ToByte(rawString.ElementAt(ii));
            }
            rawString = null;

            byte[] s_aditionalEntropy = new byte[passHash.Length];
            for (int ii = 0; ii < passHash.Length; ii++)
            {
                s_aditionalEntropy[ii] = Convert.ToByte(passHash.ElementAt(ii));
            }
            passHash = null;

            return ProtectedData.Protect(data, s_aditionalEntropy, DataProtectionScope.CurrentUser);
        }
        public static string MSUnprotect(byte[] data, string passHash)
        {
            byte[] s_aditionalEntropy = new byte[passHash.Length];
            for (int ii = 0; ii < passHash.Length; ii++)
            {
                s_aditionalEntropy[ii] = Convert.ToByte(passHash.ElementAt(ii));
            }
            passHash = null;

            byte[] Unprotected = ProtectedData.Unprotect(data, s_aditionalEntropy, DataProtectionScope.CurrentUser);
            StringBuilder sB = new StringBuilder();
            foreach (var b in Unprotected)
            {
                sB.Append(b);
            }
            Unprotected = null;

            return sB.ToString();
        }
        public static string ConvertToUnsecureString(SecureString securePassword)
        {
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }

    public static class GeneralTools
    {
        public static DateTime TimeStampToDateTimeUsingMilliseconds(Int64 unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        public static DateTime TimeStampToDateTimeUsingSeconds(Int64 unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
    public static class UnixTime
    {
        static DateTime unixEpoch;
        static UnixTime()
        {
            unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }

        public static UInt32 Now { get { return GetFromDateTime(DateTime.UtcNow); } }
        public static UInt32 GetFromDateTime(DateTime d) { return (UInt32)(d - unixEpoch).TotalSeconds; }
        public static DateTime ConvertToDateTime(double unixtime) { return unixEpoch.AddSeconds(unixtime); }
    }
}
