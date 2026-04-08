using System;
using System.Linq;
using System.Windows.Forms;

namespace Report_Management  // Изменено
{
    public partial class Form1 : Form
    {
        private ReportManager reportManager = null!;
        private TextBox titleTextBox = null!;
        private TextBox contentTextBox = null!;
        private Button addReportButton = null!;
        private Button removeReportButton = null!;
        private Button updateReportButton = null!;
        private ListBox reportsListBox = null!;
        private Label titleLabel = null!;
        private Label contentLabel = null!;

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
            reportManager = new ReportManager();
            UpdateReportsList();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Управление отчётами";
            this.Width = 650;
            this.Height = 550;
            this.StartPosition = FormStartPosition.CenterScreen;

            titleLabel = new Label
            {
                Location = new System.Drawing.Point(10, 15),
                Text = "Название:",
                Width = 70
            };

            titleTextBox = new TextBox
            {
                Location = new System.Drawing.Point(85, 12),
                Width = 300,
                PlaceholderText = "Введите название отчёта"
            };

            contentLabel = new Label
            {
                Location = new System.Drawing.Point(10, 45),
                Text = "Содержание:",
                Width = 70
            };

            contentTextBox = new TextBox
            {
                Location = new System.Drawing.Point(85, 42),
                Width = 400,
                Height = 120,
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                PlaceholderText = "Введите содержание отчёта"
            };

            addReportButton = new Button
            {
                Location = new System.Drawing.Point(85, 175),
                Text = "Добавить",
                Width = 100,
                BackColor = System.Drawing.Color.LightGreen
            };
            addReportButton.Click += AddReportButton_Click;

            removeReportButton = new Button
            {
                Location = new System.Drawing.Point(195, 175),
                Text = "Удалить",
                Width = 100,
                BackColor = System.Drawing.Color.LightCoral
            };
            removeReportButton.Click += RemoveReportButton_Click;

            updateReportButton = new Button
            {
                Location = new System.Drawing.Point(305, 175),
                Text = "Обновить",
                Width = 100,
                BackColor = System.Drawing.Color.LightBlue
            };
            updateReportButton.Click += UpdateReportButton_Click;

            reportsListBox = new ListBox
            {
                Location = new System.Drawing.Point(10, 210),
                Width = 600,
                Height = 280,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            reportsListBox.SelectedIndexChanged += ReportsListBox_SelectedIndexChanged;

            this.Controls.Add(titleLabel);
            this.Controls.Add(titleTextBox);
            this.Controls.Add(contentLabel);
            this.Controls.Add(contentTextBox);
            this.Controls.Add(addReportButton);
            this.Controls.Add(removeReportButton);
            this.Controls.Add(updateReportButton);
            this.Controls.Add(reportsListBox);
        }

        private void ReportsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (reportsListBox.SelectedIndex != -1)
            {
                var selectedReport = GetSelectedReport();
                if (selectedReport != null)
                {
                    titleTextBox.Text = selectedReport.Title;
                    contentTextBox.Text = selectedReport.Content;
                }
            }
        }

        private void UpdateReportsList()
        {
            reportsListBox.Items.Clear();
            foreach (var report in reportManager.Reports)
            {
                reportsListBox.Items.Add($"{report.Title} - {report.CreationDate:yyyy-MM-dd HH:mm}");
            }
        }

        private Report GetSelectedReport()
        {
            if (reportsListBox.SelectedIndex == -1)
                return null!;

            string selectedItem = reportsListBox.SelectedItem.ToString()!;
            int separatorIndex = selectedItem.LastIndexOf(" - ");

            if (separatorIndex != -1)
            {
                string title = selectedItem.Substring(0, separatorIndex);
                string dateStr = selectedItem.Substring(separatorIndex + 3);

                if (DateTime.TryParse(dateStr, out DateTime date))
                {
                    return reportManager.Reports.FirstOrDefault(r =>
                        r.Title == title && r.CreationDate.ToString("yyyy-MM-dd HH:mm") == dateStr);
                }
            }

            return null!;
        }

        private void AddReportButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(titleTextBox.Text))
            {
                MessageBox.Show("Введите название отчёта!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(contentTextBox.Text))
            {
                MessageBox.Show("Введите содержание отчёта!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (reportManager.Reports.Any(r => r.Title.Equals(titleTextBox.Text, StringComparison.OrdinalIgnoreCase)))
            {
                DialogResult result = MessageBox.Show("Отчёт с таким названием уже существует. Продолжить?",
                    "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return;
            }

            Report newReport = new Report(titleTextBox.Text.Trim(), contentTextBox.Text.Trim(), DateTime.Now);

            try
            {
                reportManager.AddReport(newReport);
                titleTextBox.Clear();
                contentTextBox.Clear();
                UpdateReportsList();
                MessageBox.Show("Отчёт успешно добавлен!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RemoveReportButton_Click(object sender, EventArgs e)
        {
            if (reportsListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите отчёт для удаления!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить этот отчёт?",
                "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var reportToRemove = GetSelectedReport();
                if (reportToRemove != null)
                {
                    try
                    {
                        reportManager.RemoveReport(reportToRemove);
                        titleTextBox.Clear();
                        contentTextBox.Clear();
                        UpdateReportsList();
                        MessageBox.Show("Отчёт успешно удалён!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void UpdateReportButton_Click(object sender, EventArgs e)
        {
            if (reportsListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите отчёт для обновления!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(titleTextBox.Text))
            {
                MessageBox.Show("Введите название отчёта!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(contentTextBox.Text))
            {
                MessageBox.Show("Введите содержание отчёта!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var reportToUpdate = GetSelectedReport();
            if (reportToUpdate != null)
            {
                try
                {
                    reportManager.UpdateReport(reportToUpdate, titleTextBox.Text.Trim(), contentTextBox.Text.Trim());
                    UpdateReportsList();
                    MessageBox.Show("Отчёт успешно обновлён!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обновлении: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}