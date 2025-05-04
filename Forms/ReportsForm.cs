using BookstoreManagement.Data;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace BookstoreManagement.Forms
{
    public partial class ReportsForm : Form
    {
        // Controls for Best Sellers tab
        private DateTimePicker dtpStartBestSellers, dtpEndBestSellers;
        private Button btnGenerateBestSellers, btnExportBestSellers;
        private DataGridView dgvBestSellers;

        // Controls for Popular Authors tab
        private DateTimePicker dtpStartAuthors, dtpEndAuthors;
        private Button btnGenerateAuthors, btnExportAuthors;
        private DataGridView dgvAuthors;

        // Controls for Popular Genres tab
        private DateTimePicker dtpStartGenres, dtpEndGenres;
        private Button btnGenerateGenres, btnExportGenres;
        private DataGridView dgvGenres;

        private ComboBox cmbSearchGenre, cmbReleaseFilter;
        private DataGridView dgvSearchBooks, dgvNewReleases;
        private TextBox txtSearchName, txtSearchAuthor;
        private Button btnSearch, btnLoadNewReleases;

        private TabControl tabControlReports;
        private TabPage tabBestSellers, tabPopularAuthors, tabPopularGenres, tabSearchBooks, tabNewReleases;

        public ReportsForm()
        {
            this.Text = "Sales Report";
            this.Size = new System.Drawing.Size(800, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeControls();
        }

        private void InitializeControls()
        {
            tabControlReports = new TabControl { Dock = DockStyle.Fill };

            tabBestSellers = new TabPage("Best Sellers");
            tabPopularAuthors = new TabPage("Popular Authors");
            tabPopularGenres = new TabPage("Popular Genres");
            tabSearchBooks = new TabPage("Search Books");
            tabNewReleases = new TabPage("New Releases");

            tabControlReports.TabPages.AddRange(new TabPage[]
            {
                tabBestSellers, tabPopularAuthors, tabPopularGenres, tabNewReleases, tabSearchBooks
            });

            this.Controls.Add(tabControlReports);

            InitializeBestSellersTab();
            InitializePopularAuthorsTab();
            InitializePopularGenresTab();
            InitializeSearchBooksTab();
            InitializeNewReleasesTab();
        }

        private void InitializeBestSellersTab()
        {
            dtpStartBestSellers = new DateTimePicker { Top = 20, Left = 120, Width = 180 };
            dtpEndBestSellers = new DateTimePicker { Top = 60, Left = 120, Width = 180 };
            btnGenerateBestSellers = new Button { Text = "Generate Best Sellers Report", Top = 100, Left = 20, Width = 180 };
            btnGenerateBestSellers.Click += btnGenerateBestSellersReport_Click;
            dgvBestSellers = new DataGridView { Top = 140, Left = 20, Width = 740, Height = 200, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            btnExportBestSellers = new Button { Text = "Export Report", Top = 360, Left = 20, Width = 180 };
            btnExportBestSellers.Click += (s, e) => ExportToExcel(dgvBestSellers);

            tabBestSellers.Controls.AddRange(new Control[]
            {
                new Label { Text = "Start Date:", Top = 20, Left = 20 },
                new Label { Text = "End Date:", Top = 60, Left = 20 },
                dtpStartBestSellers, dtpEndBestSellers,
                btnGenerateBestSellers, dgvBestSellers, btnExportBestSellers
            });
        }

        private void InitializePopularAuthorsTab()
        {
            dtpStartAuthors = new DateTimePicker { Top = 20, Left = 120, Width = 180 };
            dtpEndAuthors = new DateTimePicker { Top = 60, Left = 120, Width = 180 };
            btnGenerateAuthors = new Button { Text = "Generate Popular Authors Report", Top = 100, Left = 20, Width = 180 };
            btnGenerateAuthors.Click += btnGeneratePopularAuthorsReport_Click;
            dgvAuthors = new DataGridView { Top = 140, Left = 20, Width = 740, Height = 200, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            btnExportAuthors = new Button { Text = "Export Report", Top = 360, Left = 20, Width = 180 };
            btnExportAuthors.Click += (s, e) => ExportToExcel(dgvAuthors);

            tabPopularAuthors.Controls.AddRange(new Control[]
            {
                new Label { Text = "Start Date:", Top = 20, Left = 20 },
                new Label { Text = "End Date:", Top = 60, Left = 20 },
                dtpStartAuthors, dtpEndAuthors,
                btnGenerateAuthors, dgvAuthors, btnExportAuthors
            });
        }

        private void InitializePopularGenresTab()
        {
            dtpStartGenres = new DateTimePicker { Top = 20, Left = 120, Width = 180 };
            dtpEndGenres = new DateTimePicker { Top = 60, Left = 120, Width = 180 };
            btnGenerateGenres = new Button { Text = "Generate Popular Genres Report", Top = 100, Left = 20, Width = 180 };
            btnGenerateGenres.Click += btnGeneratePopularGenresReport_Click;
            dgvGenres = new DataGridView { Top = 140, Left = 20, Width = 740, Height = 200, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            btnExportGenres = new Button { Text = "Export Report", Top = 360, Left = 20, Width = 180 };
            btnExportGenres.Click += (s, e) => ExportToExcel(dgvGenres);

            tabPopularGenres.Controls.AddRange(new Control[]
            {
                new Label { Text = "Start Date:", Top = 20, Left = 20 },
                new Label { Text = "End Date:", Top = 60, Left = 20 },
                dtpStartGenres, dtpEndGenres,
                btnGenerateGenres, dgvGenres, btnExportGenres
            });
        }

        private void InitializeSearchBooksTab()
        {
            Label lblName = new Label { Text = "Book Name:", Top = 20, Left = 20 };
            Label lblAuthor = new Label { Text = "Author:", Top = 60, Left = 20 };
            Label lblGenre = new Label { Text = "Genre:", Top = 100, Left = 20 };

            txtSearchName = new TextBox { Top = 20, Left = 120, Width = 200 };
            txtSearchAuthor = new TextBox { Top = 60, Left = 120, Width = 200 };
            cmbSearchGenre = new ComboBox { Top = 100, Left = 120, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };

            btnSearch = new Button { Text = "Search", Top = 140, Left = 120 };
            btnSearch.Click += btnSearch_Click;

            dgvSearchBooks = new DataGridView { Top = 180, Left = 20, Width = 740, Height = 200, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

            tabSearchBooks.Controls.AddRange(new Control[]
            {
                lblName, lblAuthor, lblGenre,
                txtSearchName, txtSearchAuthor, cmbSearchGenre,
                btnSearch, dgvSearchBooks
            });

            using (BookStoreContext db = new BookStoreContext())
            {
                var genres = db.Genres.ToList();
                cmbSearchGenre.DataSource = genres;
                cmbSearchGenre.DisplayMember = "Name";
                cmbSearchGenre.ValueMember = "Id";
                cmbSearchGenre.SelectedIndex = -1;
            }
        }

        private void InitializeNewReleasesTab()
        {
            Label lblFilter = new Label { Text = "Time Filter:", Top = 20, Left = 20 };
            cmbReleaseFilter = new ComboBox
            {
                Top = 20,
                Left = 120,
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbReleaseFilter.Items.AddRange(new string[] { "Day", "Week", "Month", "Year" });
            cmbReleaseFilter.SelectedIndex = 2;

            btnLoadNewReleases = new Button { Text = "Load New Releases", Top = 60, Left = 120 };
            btnLoadNewReleases.Click += btnLoadNewReleases_Click;

            dgvNewReleases = new DataGridView { Top = 100, Left = 20, Width = 740, Height = 250, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

            tabNewReleases.Controls.AddRange(new Control[]
            {
                lblFilter, cmbReleaseFilter,
                btnLoadNewReleases, dgvNewReleases
            });
        }

        private void btnGenerateBestSellersReport_Click(object sender, EventArgs e)
        {
            using (var db = new BookStoreContext())
            {
                var startDate = dtpStartBestSellers.Value.Date;
                var endDate = dtpEndBestSellers.Value.Date.AddDays(1).AddTicks(-1);

                if (endDate < startDate)
                {
                    MessageBox.Show("End Date must be after Start Date.");
                    return;
                }

                var result = db.Sales
                    .Where(s => s.DateSold >= startDate && s.DateSold <= endDate)
                    .GroupBy(s => new { s.Book.Name, s.Book.AuthorName })
                    .Select(g => new
                    {
                        BookName = g.Key.Name,
                        Author = g.Key.AuthorName,
                        QuantitySold = g.Sum(s => s.Quantity),
                        TotalSales = g.Sum(s => s.TotalPrice)
                    })
                    .OrderByDescending(r => r.QuantitySold)
                    .ToList();

                dgvBestSellers.DataSource = result;
                dgvBestSellers.Columns["TotalSales"].DefaultCellStyle.Format = "C2";
            }
        }

        private void btnGeneratePopularAuthorsReport_Click(object sender, EventArgs e)
        {
            using (var db = new BookStoreContext())
            {
                var startDate = dtpStartAuthors.Value.Date;
                var endDate = dtpEndAuthors.Value.Date.AddDays(1).AddTicks(-1);

                if (endDate < startDate)
                {
                    MessageBox.Show("End Date must be after Start Date.");
                    return;
                }

                var result = db.Sales
                    .Where(s => s.DateSold >= startDate && s.DateSold <= endDate)
                    .GroupBy(s => s.Book.AuthorName)
                    .Select(g => new
                    {
                        Author = g.Key,
                        QuantitySold = g.Sum(s => s.Quantity),
                        TotalSales = g.Sum(s => s.TotalPrice)
                    })
                    .OrderByDescending(r => r.QuantitySold)
                    .ToList();

                dgvAuthors.DataSource = result;
                dgvAuthors.Columns["TotalSales"].DefaultCellStyle.Format = "C2";
            }
        }

        private void btnGeneratePopularGenresReport_Click(object sender, EventArgs e)
        {
            using (var db = new BookStoreContext())
            {
                var startDate = dtpStartGenres.Value.Date;
                var endDate = dtpEndGenres.Value.Date.AddDays(1).AddTicks(-1);

                if (endDate < startDate)
                {
                    MessageBox.Show("End Date must be after Start Date.");
                    return;
                }

                var result = db.Sales
                    .Where(s => s.DateSold >= startDate && s.DateSold <= endDate)
                    .GroupBy(s => s.Book.Genre)
                    .Select(g => new
                    {
                        Genre = g.Key.Name,
                        QuantitySold = g.Sum(s => s.Quantity),
                        TotalSales = g.Sum(s => s.TotalPrice)
                    })
                    .OrderByDescending(r => r.QuantitySold)
                    .ToList();

                dgvGenres.DataSource = result;
                dgvGenres.Columns["TotalSales"].DefaultCellStyle.Format = "C2";
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            using (var db = new BookStoreContext())
            {
                var query = db.Books.AsQueryable();

                if (!string.IsNullOrWhiteSpace(txtSearchName.Text))
                    query = query.Where(b => b.Name.Contains(txtSearchName.Text));
                if (!string.IsNullOrWhiteSpace(txtSearchAuthor.Text))
                    query = query.Where(b => b.AuthorName.Contains(txtSearchAuthor.Text));
                if (cmbSearchGenre.SelectedItem != null)
                    query = query.Where(b => b.GenreId == (int)cmbSearchGenre.SelectedValue);

                var results = query.Select(b => new
                {
                    b.Name,
                    b.AuthorName,
                    Genre = b.Genre.Name,
                    b.DatePublished,
                    b.SalePrice
                }).ToList();

                dgvSearchBooks.DataSource = results;
            }
        }

        private void btnLoadNewReleases_Click(object sender, EventArgs e)
        {
            DateTime since = DateTime.Now;
            switch (cmbReleaseFilter.SelectedItem.ToString())
            {
                case "Day": since = DateTime.Today; break;
                case "Week": since = DateTime.Today.AddDays(-7); break;
                case "Month": since = DateTime.Today.AddMonths(-1); break;
                case "Year": since = DateTime.Today.AddYears(-1); break;
            }

            using (var db = new BookStoreContext())
            {
                var newReleases = db.Books
                    .Where(b => b.DatePublished >= since)
                    .OrderByDescending(b => b.DatePublished)
                    .Select(b => new
                    {
                        b.Name,
                        b.AuthorName,
                        Genre = b.Genre.Name,
                        b.DatePublished,
                        b.SalePrice
                    }).ToList();

                dgvNewReleases.DataSource = newReleases;
            }
        }

        private void ExportToExcel(DataGridView dgv)
        {
            if (dgv.DataSource == null)
            {
                MessageBox.Show("Please generate a report first.");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "Excel Workbook|*.xlsx", Title = "Save Report" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var dt = new DataTable();

                    foreach (DataGridViewColumn col in dgv.Columns)
                        dt.Columns.Add(col.HeaderText);

                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        if (!row.IsNewRow)
                            dt.Rows.Add(row.Cells.Cast<DataGridViewCell>().Select(c => c.Value?.ToString()).ToArray());
                    }

                    using (var workbook = new XLWorkbook())
                    {
                        workbook.Worksheets.Add(dt, "Report");
                        workbook.SaveAs(sfd.FileName);
                    }

                    MessageBox.Show("Report exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
