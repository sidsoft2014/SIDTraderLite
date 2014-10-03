using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Objects
{
    public static class UserManager
    {
        internal static string SettingsFolder = string.Format("{0}\\{1}", Application.StartupPath, "Settings");
        internal static string UsersFile = string.Format("{0}\\{1}", SettingsFolder, "Users.json");
        private static List<User> userList;
        internal static List<User> UserList
        {
            get
            {
                if (null == userList)
                    userList = new List<User>();
                return userList;
            }
            set
            {
                userList = value;
            }
        }

        public static User AddUser(string uName, string pass)
        {
            foreach (var u in UserList)
            {
                if (u.UserName == uName)
                {
                    return null;
                }
            }
            
            User user = new User();
            user.CreateUser(uName, pass);
            UserList.Add(user);
            
            SaveUsers();
            return user;
        }
        public static User GetUser(string uName, string pass)
        {
            User u = null;
            
            if (!string.IsNullOrWhiteSpace(uName) && UserList.Count > 0)
            {
                foreach (var item in UserList)
                {
                    if (item.ToString() == uName)
                    {
                        if (item.Unlock(pass))
                        {
                            u = item;
                        }
                        break;
                    }
                }
            }
            return u;
        }
        public static bool EditUser(User user)
        {
            var q = from uN in UserList
                    where uN.UserName == user.UserName
                    select uN;

            if (q.Count() > 0)
            {
                User oldUserData = userList.First();
                oldUserData = user;
                SaveUsers();
                return true;
            }
            else return false;
        }

        public static void SaveUsers()
        {
            string json = JsonConvert.SerializeObject(userList);

            if (!Directory.Exists(SettingsFolder))
                Directory.CreateDirectory(SettingsFolder);
            if (!File.Exists(UsersFile))
                File.Create(UsersFile).Dispose();

            File.WriteAllText(UsersFile, json);
        }
        public static void LoadUsers()
        {
            if (File.Exists(UsersFile))
            {
                string json = File.ReadAllText(UsersFile);
                userList = JsonConvert.DeserializeObject<List<User>>(json);
            }
        }
        public static bool WipeUsers()
        {
            if (File.Exists(UsersFile))
            {
                File.Delete(UsersFile);
                return true;
            }
            else return false;
        }
    }
}
