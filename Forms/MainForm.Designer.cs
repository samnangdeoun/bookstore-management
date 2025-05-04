namespace BookstoreManagement.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Button btnBooks;
        private System.Windows.Forms.Button btnGenres;
        private System.Windows.Forms.Button btnSell;
        private System.Windows.Forms.Button btnDiscount;
        private System.Windows.Forms.Button btnReserved;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Button btnSignOut;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
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
            this.btnSignOut = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // Common values
            int buttonWidth = 200;
            int buttonHeight = 45;
            int marginTop = 20;
            int startY = 30;
            int leftX = 30;

            // 
            // btnBooks
            // 
            this.btnBooks.Location = new System.Drawing.Point(leftX, startY);
            this.btnBooks.Name = "btnBooks";
            this.btnBooks.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            this.btnBooks.TabIndex = 0;
            this.btnBooks.Text = "Manage Books";
            this.btnBooks.UseVisualStyleBackColor = true;
            this.btnBooks.Click += new System.EventHandler(this.btnBooks_Click);

            // 
            // btnGenres
            // 
            this.btnGenres.Location = new System.Drawing.Point(leftX, startY + (1 * (buttonHeight + marginTop)));
            this.btnGenres.Name = "btnGenres";
            this.btnGenres.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            this.btnGenres.TabIndex = 1;
            this.btnGenres.Text = "Manage Genres";
            this.btnGenres.UseVisualStyleBackColor = true;
            this.btnGenres.Click += new System.EventHandler(this.btnGenres_Click);

            // 
            // btnSell
            // 
            this.btnSell.Location = new System.Drawing.Point(leftX, startY + (2 * (buttonHeight + marginTop)));
            this.btnSell.Name = "btnSell";
            this.btnSell.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            this.btnSell.TabIndex = 2;
            this.btnSell.Text = "Sell Books";
            this.btnSell.UseVisualStyleBackColor = true;
            this.btnSell.Click += new System.EventHandler(this.btnSell_Click);

            // 
            // btnDiscount
            // 
            this.btnDiscount.Location = new System.Drawing.Point(leftX, startY + (3 * (buttonHeight + marginTop)));
            this.btnDiscount.Name = "btnDiscount";
            this.btnDiscount.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            this.btnDiscount.TabIndex = 3;
            this.btnDiscount.Text = "Manage Discounts";
            this.btnDiscount.UseVisualStyleBackColor = true;
            this.btnDiscount.Click += new System.EventHandler(this.btnDiscount_Click);

            // 
            // btnReserved
            // 
            this.btnReserved.Location = new System.Drawing.Point(leftX, startY + (4 * (buttonHeight + marginTop)));
            this.btnReserved.Name = "btnReserved";
            this.btnReserved.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            this.btnReserved.TabIndex = 4;
            this.btnReserved.Text = "Reserved Books";
            this.btnReserved.UseVisualStyleBackColor = true;
            this.btnReserved.Click += new System.EventHandler(this.btnReserved_Click);

            // 
            // btnReports
            // 
            this.btnReports.Location = new System.Drawing.Point(leftX, startY + (5 * (buttonHeight + marginTop)));
            this.btnReports.Name = "btnReports";
            this.btnReports.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            this.btnReports.TabIndex = 5;
            this.btnReports.Text = "Reports";
            this.btnReports.UseVisualStyleBackColor = true;
            this.btnReports.Click += new System.EventHandler(this.btnReports_Click);

            // 
            // btnSignOut
            // 
            this.btnSignOut.Location = new System.Drawing.Point(leftX, startY + (6 * (buttonHeight + marginTop)));
            this.btnSignOut.Name = "btnSignOut";
            this.btnSignOut.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            this.btnSignOut.TabIndex = 6;
            this.btnSignOut.Text = "Sign Out";
            this.btnSignOut.UseVisualStyleBackColor = true;
            this.btnSignOut.Click += new System.EventHandler(this.btnSignOut_Click);

            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(buttonWidth + 60, startY + (7 * (buttonHeight + marginTop)));
            this.Controls.Add(this.btnBooks);
            this.Controls.Add(this.btnGenres);
            this.Controls.Add(this.btnSell);
            this.Controls.Add(this.btnDiscount);
            this.Controls.Add(this.btnReserved);
            this.Controls.Add(this.btnReports);
            this.Controls.Add(this.btnSignOut);
            this.Name = "MainForm";
            this.Text = "Bookstore Management";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
        }
    }
}
