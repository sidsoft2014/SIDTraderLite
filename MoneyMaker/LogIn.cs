using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security;

namespace Objects
{
    public partial class LogIn : Form
    {
        public LogIn()
        {
            InitializeComponent();
            this.button_Create.DialogResult = System.Windows.Forms.DialogResult.No;
        }
        private void button_LogIn_Click(object sender, EventArgs e)
        {
            if (this.button_LogIn.Text != "Continue")
            {
                if (!string.IsNullOrWhiteSpace(textBox_UName.Text) && !string.IsNullOrWhiteSpace(textBox_Pass.Text))
                {
                    bool valid = true;

                    foreach (var c in textBox_UName.Text)
                    {
                        if (!Char.IsLetter(c) & !Char.IsNumber(c))
                        {
                            valid = false;
                            break;
                        }
                    }
                    foreach (var c in textBox_Pass.Text)
                    {
                        if (!Char.IsLetter(c) & !Char.IsNumber(c))
                        {
                            valid = false;
                            break;
                        }
                    }

                    if (valid)
                    {
                        User user = UserManager.GetUser(textBox_UName.Text, textBox_Pass.Text);

                        if (user != null)
                        {

                            this.Text = "Welcome - " + user.UserName;
                            this.button_LogIn.DialogResult = System.Windows.Forms.DialogResult.Yes;
                            Core.SetUser(user);
                            this.button_LogIn.Text = "Continue";

                        }
                        else
                        {
                            MessageBox.Show("Invalid user name or password.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please only use alpha-numeric characters for user name and password.");
                        textBox_UName.Text = "";
                        textBox_Pass.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a user name and password");
                }
            }
        }

        private void textBox_Pass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                button_LogIn_Click(new object(), null);
        }

    }
}
