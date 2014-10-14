using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Objects
{
    public partial class KeySetter : Form
    {
        private ExchangeEnum selectedExchange;
        private User activeUser;

        public KeySetter(User user)
        {
            this.activeUser = user;
            InitializeComponent();
            label_Status.Text = "";
            label_uName.Text = string.Format("User: {0}", user.UserName);            
        }

        #region Exchange Buttons
        private void button_BTCe_Click(object sender, EventArgs e)
        {
            selectedExchange = ExchangeEnum.BTCe;
            UpdateUI();
        }
        private void button_Cryptsy_Click(object sender, EventArgs e)
        {
            selectedExchange = ExchangeEnum.Cryptsy;
            UpdateUI();
        }
        private void button_Poloniex_Click(object sender, EventArgs e)
        {
            selectedExchange = ExchangeEnum.Poloniex;
            UpdateUI();
        }
        private void button_Kraken_Click(object sender, EventArgs e)
        {
            selectedExchange = ExchangeEnum.Kraken;
            UpdateUI();
        }
        private void button_Mint_Click(object sender, EventArgs e)
        {
            selectedExchange = ExchangeEnum.MintPal;
            UpdateUI();
        }
        private void button_Bittrex_Click(object sender, EventArgs e)
        {
            selectedExchange = ExchangeEnum.Bittrex;
            UpdateUI();
        }
        private void button_Vircurex_Click(object sender, EventArgs e)
        {
            selectedExchange = ExchangeEnum.Vircurex;
            UpdateUI();
        }
        #endregion
       

        private void UpdateUI()
        {
            label_ExName.Text = selectedExchange.ToString();
            label_Status.Text = "";
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            if (label_ExName.Text != "Choose Exchange")
            {
                if (!string.IsNullOrEmpty(textBox_pKey.Text.Trim()) && !string.IsNullOrEmpty(textBox_sKey.Text.Trim()))
                {
                    if (activeUser.SetKey(selectedExchange, textBox_pKey.Text, textBox_sKey.Text))
                    {
                        if (UserManager.EditUser(activeUser))
                        {
                            textBox_pKey.Text = "Keys saved";
                            textBox_sKey.Text = "";
                        }
                        else
                            textBox_pKey.Text = "Error saving keys";
                    }
                    else
                        textBox_pKey.Text = "Error setting keys";
                }
                else
                {
                    MessageBox.Show("Please enter both keys.");
                }
            }
            else
            {
                MessageBox.Show("Please select an exchange.");
            }
        }        
        private void button_Wipe_Click(object sender, EventArgs e)
        {
            string msg = String.Empty;
            string warning = "This will wipe ALL user data.\n\nIf you choose 'Yes' the user file will be erased and all saved user accounts and keys will be lost.\n\nUse this if you plan to uninstall the software to ensure this file is also removed";
            DialogResult dr = MessageBox.Show(warning, "Wipe User Data?", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.No:
                    msg = "Wipe cancelled";
                    break;
                case DialogResult.Yes:
                    if (UserManager.WipeUsers())
                        msg = "User data deleted";
                    else msg = "No user data found";
                    break;
                default:
                    break;
            }
            label_Status.Text = msg;
        }

        private void KeySetter_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.activeUser = null;
        }

        private void textBox_pKey_Enter(object sender, EventArgs e)
        {
            if (textBox_pKey.Text == "Keys saved")
                textBox_pKey.Text = "";
        }

    }
}
