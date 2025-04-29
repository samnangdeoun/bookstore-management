using System;
using System.Linq;
using System.Windows.Forms;

namespace BookstoreManagement.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnBooks_Click(object sender, EventArgs e)
        {
            new BookListForm().ShowDialog();
        }

        private void btnGenres_Click(object sender, EventArgs e)
        {
            new GenreForm().ShowDialog();
        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            new SellForm().ShowDialog();
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            new DiscountForm().ShowDialog();
        }

        private void btnReserved_Click(object sender, EventArgs e)
        {
            new ReservedBooksForm().ShowDialog();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            new ReportsForm().ShowDialog();
        }
    }
}
