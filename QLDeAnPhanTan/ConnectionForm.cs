using System;
using System.Windows.Forms;

namespace QLDeAnPhanTan
{
    public partial class ConnectionForm : Form
    {
        public string ServerName { get; private set; } = string.Empty;
        public string Username { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;
        public string DatabaseName { get; private set; } = string.Empty;

        public ConnectionForm()
        {
            InitializeComponent();
            LoadSites();
        }

        private void LoadSites()
        {
            cboSite.Items.Clear();
            cboSite.Items.Add("Site Gốc - MSI\\YLC (QLDeAn)");
            cboSite.Items.Add("Site P1 - DESKTOP-SEERKGC\\ANHTHU (QLDeAn_P1)");
            cboSite.Items.Add("Site P2 - LAPTOP-F37K83BK\\SQLEXPRESS (QLDeAn_P2)");
            cboSite.SelectedIndex = 0;
        }

        private void cboSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboSite.SelectedIndex)
            {
                case 0: // Site Gốc
                    txtServerName.Text = "MSI\\YLC";
                    DatabaseName = "QLDeAn";
                    txtPassword.Text = "";
                    break;
                case 1: // Site P1
                    txtServerName.Text = "DESKTOP-SEERKGC\\ANHTHU";
                    DatabaseName = "QLDeAn_P1";
                    txtPassword.Text = "123";
                    break;
                case 2: // Site P2
                    txtServerName.Text = "LAPTOP-F37K83BK\\SQLEXPRESS";
                    DatabaseName = "QLDeAn_P2";
                    txtPassword.Text = "sa123@";
                    break;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtServerName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên Server!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtServerName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập Username!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập Password!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            ServerName = txtServerName.Text.Trim();
            Username = txtUsername.Text.Trim();
            Password = txtPassword.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
