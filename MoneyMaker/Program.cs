using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Objects
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            RunLogInScreen();
        }
        
        #region Start Up
        /// <summary>
        /// NEEDS VALIDATION AND STORING
        /// </summary>
        private static void RunLogInScreen()
        {
            UserManager.LoadUsers();
            bool? isGuest = null;

            DialogResult loginDialog;
            using (var login = new LogIn())
            {
                loginDialog = login.ShowDialog();
                switch (loginDialog)
                {
                    ///Yes is "Log In"
                    case DialogResult.Yes:
                        {
                            isGuest = false;
                            break;
                        }
                    ///No is "Create"
                    case DialogResult.No:
                        {
                            using (var CreateUser = new UserSettingsForm())
                            {
                                DialogResult newUserDialog = CreateUser.ShowDialog();
                                switch (newUserDialog)
                                {
                                    ///Abort Application
                                    case DialogResult.Abort:
                                        {
                                            break;
                                        }
                                    ///Cancel New User Creation and Log in as Guest
                                    case DialogResult.Cancel:
                                        {
                                            isGuest = true;
                                            break;
                                        }
                                    ///Validate New User and Log In
                                    case DialogResult.OK:
                                        {
                                            isGuest = false;
                                            break;
                                        }
                                    default:
                                        {
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                }
            }
            if (null != isGuest)
                StartMainForm((bool)isGuest);
        }
        /// <summary>
        /// Launches Exchanges as either guest or authorised
        /// </summary>
        /// <param name="Guest"></param>
        private static void StartMainForm(bool Guest = true)
        {
            Application.Run(new MainForm(Guest));
        }
        #endregion

    }
}
