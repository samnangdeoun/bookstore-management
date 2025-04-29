using System;
using System.Linq;
using System.Windows.Forms;

namespace BookstoreManagement.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            using (var db = new BookstoreDataDataContext())
            {
                var user = db.Users
                    .FirstOrDefault(u => u.Username == txtUsername.Text && u.PasswordHash == txtPassword.Text); // Replace with hashed check

                if (user != null)
                {
                    new MainForm().Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid login!");
                }
            }
        }
    }
}
