using System;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace BookstoreManagement.Forms
{
    public partial class LoginForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;

        public LoginForm()
        {
            this.Text = "Login";
            this.Size = new Size(300, 200);
            this.StartPosition = FormStartPosition.CenterScreen;

            InitializeControls();
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private void InitializeControls()
        {
            Label lblUsername = new Label
            {
                Text = "Username:",
                Top = 20,
                Left = 20,
                Width = 80
            };
            txtUsername = new TextBox
            {
                Top = lblUsername.Top,
                Left = 110,
                Width = 150
            };

            Label lblPassword = new Label
            {
                Text = "Password:",
                Top = 60,
                Left = 20,
                Width = 80
            };
            txtPassword = new TextBox
            {
                Top = lblPassword.Top,
                Left = 110,
                Width = 150,
                UseSystemPasswordChar = true
            };

            btnLogin = new Button
            {
                Text = "Login",
                Top = 100,
                Left = 110,
                Width = 100
            };
            btnLogin.Click += btnLogin_Click;

            this.Controls.Add(lblUsername);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            using (var db = new BookStoreDBDataContext())
            {
                var hashedPassword = HashPassword(txtPassword.Text);
                var user = db.Users
                    .FirstOrDefault(u => u.Username == txtUsername.Text && u.PasswordHash == hashedPassword); // NOTE: Ideally hash the password before comparing

                Console.WriteLine(db + " Username");
                if (user != null)
                {
                    new MainForm().Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid login!", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
