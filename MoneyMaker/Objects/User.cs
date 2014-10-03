using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Toolbox;

namespace Objects
{
    public class User : IEquatable<User>, IDisposable
    {
        public string UserName { get; set; }
        public string Pass { get; set; }
        private Dictionary<ExchangeEnum, string[]> _encryptedKeys;
        public Dictionary<ExchangeEnum, string[]> EncryptedKeys
        {
            get
            {
                if (null == _encryptedKeys)
                    _encryptedKeys = new Dictionary<ExchangeEnum, string[]>();
                return _encryptedKeys;
            }
            protected set
            {
                _encryptedKeys = value;
            }
        }
        
        public User() { }

        public void CreateUser(string userName, string password)
        {
            this.UserName = userName;
            this.Pass = MakeHash(userName, password);
        }
        public bool Unlock(string pass)
        {
            bool success = false;
            string key = MakeHash(UserName, pass);

            if (null != Pass)
            {
                if (key == Pass)
                {
                    success = true;
                }
            }

            return success;
        }
        
        public bool SetKey(ExchangeEnum exch, string publicKey, string privateKey)
        {
            string ePub = EncryptionTools.UserEncrypt(publicKey, this);
            string ePvt = null;
            if (privateKey != null)
                ePvt = EncryptionTools.UserEncrypt(privateKey, this);

            publicKey = null;
            privateKey = null;

            bool success = false;
            
            string[] ks = new string[] {ePub, ePvt, "true"};
            if (!EncryptedKeys.ContainsKey(exch))
            {                    
                EncryptedKeys.Add(exch, ks);
            }
            else
            {
                EncryptedKeys[exch] = ks;
            }
            success = true;

            Core.SetUser(this);
            return success;
        }
        public void NotNew(ExchangeEnum exch)
        {
            var keys = EncryptedKeys[exch];
            keys[2] = "false";
        }

        public override string ToString()
        {
            return UserName;
        }
        public string ToStringVerbose()
        {
            string msg = string.Format("User:{0}, KeyCount:{1}", UserName, EncryptedKeys.Count);
            return msg;
        }
        public bool Equals(User other)
        {
            return this.UserName == other.UserName;
        }

        private static string MakeHash(string seed, string pass)
        {
            HMACSHA512 hmAcSha = new HMACSHA512(Encoding.ASCII.GetBytes(pass));
            byte[] messagebyte = Encoding.ASCII.GetBytes(seed);
            byte[] hashmessage = hmAcSha.ComputeHash(messagebyte);
            string sign = BitConverter.ToString(hashmessage);
            sign = sign.Replace("-", "");
            return sign;
        }

        public void Dispose()
        {
            this.EncryptedKeys = null;
        }
    }
}
