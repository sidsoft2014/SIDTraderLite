using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Toolbox;

namespace Objects
{
    public partial class UserSettingsForm : Form
    {
        private User user;

        public UserSettingsForm()
        {
            InitializeComponent();

            this.button_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Abort.DialogResult = System.Windows.Forms.DialogResult.Abort;
        }
        private void button_Create_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox_UName.Text)
                && !string.IsNullOrWhiteSpace(textBox_Pass.Text))
            {
                user = UserManager.AddUser(textBox_UName.Text, textBox_Pass.Text);
                if (null != user)
                {
                    this.button_Abort.Enabled = false;
                    this.button_Cancel.Enabled = false;
                    this.button_OkSub.Enabled = false;

                    this.button_Ok.Enabled = true;
                    this.button_Ok.Visible = true;
                }
                else
                {
                    MessageBox.Show("User already exists.");
                }
                textBox_Pass.Text = "";
            }
            else
                MessageBox.Show("Please enter a user name and password.");
        }
        private void button_Ok_Click(object sender, EventArgs e)
        {
            Core.SetUser(user);
        }

    }
}
