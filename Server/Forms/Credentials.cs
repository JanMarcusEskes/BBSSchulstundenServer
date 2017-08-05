using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server.Forms
{
    public partial class Credentials : Form
    {
        public Credentials()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Credentials_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.settings.User(txtUser.Text);
            Program.settings.Passwort(txtPassword.Text);
        }

        private void txtPassword_KeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(null, null);
            }
        }
    }
}
