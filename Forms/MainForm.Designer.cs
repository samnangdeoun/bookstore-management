using System.Windows.Forms;

namespace BookstoreManagement.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private Button btnBooks;
        private Button btnGenres;
        private Button btnSell;
        private Button btnDiscount;
        private Button btnReserved;
        private Button btnReports;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnBooks = new System.Windows.Forms.Button();
            this.btnGenres = new System.Windows.Forms.Button();
            this.btnSell = new System.Windows.Forms.Button();
            this.btnDiscount = new System.Windows.Forms.Button();
            this.btnReserved = new System.Windows.Forms.Button();
            this.btnReports = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnBooks
            // 
            this.btnBooks.Location = new System.Drawing.Point(30, 30);
            this.btnBooks.Name = "btnBooks";
            this.btnBooks.Size = new System.Drawing.Size(200, 40);
            this.btnBooks.TabIndex = 0;
            this.btnBooks.Text = "Manage Books";
            this.btnBooks.UseVisualStyleBackColor = true;
            this.btnBooks.Click += new System.EventHandler(this.btnBooks_Click);
            // 
            // btnGenres
            // 
            this.btnGenres.Location = new System.Drawing.Point(30, 80);
            this.btnGenres.Name = "btnGenres";
            this.btnGenres.Size = new System.Drawing.Size(200, 40);
            this.btnGenres.TabIndex = 1;
            this.btnGenres.Text = "Manage Genres";
            this.btnGenres.UseVisualStyleBackColor = true;
            this.btnGenres.Click += new System.EventHandler(this.btnGenres_Click);
            // 
            // btnSell
            // 
            this.btnSell.Location = new System.Drawing.Point(30, 130);
            this.btnSell.Name = "btnSell";
            this.btnSell.Size = new System.Drawing.Size(200, 40);
            this.btnSell.TabIndex = 2;
            this.btnSell.Text = "Sell Books";
            this.btnSell.UseVisualStyleBackColor = true;
            this.btnSell.Click += new System.EventHandler(this.btnSell_Click);
            // 
            // btnDiscount
            // 
            this.btnDiscount.Location = new System.Drawing.Point(30, 180);
            this.btnDiscount.Name = "btnDiscount";
            this.btnDiscount.Size = new System.Drawing.Size(200, 40);
            this.btnDiscount.TabIndex = 3;
            this.btnDiscount.Text = "Manage Discounts";
            this.btnDiscount.UseVisualStyleBackColor = true;
            this.btnDiscount.Click += new System.EventHandler(this.btnDiscount_Click);
            // 
            // btnReserved
            // 
            this.btnReserved.Location = new System.Drawing.Point(30, 230);
            this.btnReserved.Name = "btnReserved";
            this.btnReserved.Size = new System.Drawing.Size(200, 40);
            this.btnReserved.TabIndex = 4;
            this.btnReserved.Text = "Reserved Books";
            this.btnReserved.UseVisualStyleBackColor = true;
            this.btnReserved.Click += new System.EventHandler(this.btnReserved_Click);
            // 
            // btnReports
            // 
            this.btnReports.Location = new System.Drawing.Point(30, 280);
            this.btnReports.Name = "btnReports";
            this.btnReports.Size = new System.Drawing.Size(200, 40);
            this.btnReports.TabIndex = 5;
            this.btnReports.Text = "Reports";
            this.btnReports.UseVisualStyleBackColor = true;
            this.btnReports.Click += new System.EventHandler(this.btnReports_Click);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(260, 360);
            this.Controls.Add(this.btnBooks);
            this.Controls.Add(this.btnGenres);
            this.Controls.Add(this.btnSell);
            this.Controls.Add(this.btnDiscount);
            this.Controls.Add(this.btnReserved);
            this.Controls.Add(this.btnReports);
            this.Name = "MainForm";
            this.Text = "Bookstore Management";
            this.ResumeLayout(false);
        }
    }
}
