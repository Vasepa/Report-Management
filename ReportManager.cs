using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace report_management
{
    public class ReportManager
    {
        public List<Report> Reports { get; private set; }
        private readonly string filePath = "reports.txt";

        public ReportManager()
        {
            Reports = new List<Report>();
            LoadReports();
        }

        public void AddReport(Report report)
        {
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }
            Reports.Add(report);
            SaveReports();
        }

        public void RemoveReport(Report report)
        {
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }
            Reports.Remove(report);
            SaveReports();
        }

        public void UpdateReport(Report report, string newTitle, string newContent)
        {
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }
            report.Title = newTitle;
            report.Content = newContent;
            SaveReports();
        }

        private void SaveReports()
        {
            try
            {
                var lines = Reports.Select(r => 
                    $"{EscapeForFile(r.Title)}|{EscapeForFile(r.Content)}|{r.CreationDate:yyyy-MM-dd HH:mm:ss}");
                File.WriteAllLines(filePath, lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }

        private void LoadReports()
        {
            if (!File.Exists(filePath)) return;
            
            try
            {
                foreach (var line in File.ReadAllLines(filePath))
                {
                    var parts = line.Split('|');
                    if (parts.Length == 3 && DateTime.TryParse(parts[2], out var date))
                    {
                        Reports.Add(new Report(UnescapeFromFile(parts[0]), UnescapeFromFile(parts[1]), date));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке: {ex.Message}");
            }
        }

        private string EscapeForFile(string text)
        {
            return text.Replace("|", "\\|").Replace("\n", "\\n").Replace("\r", "\\r");
        }

        private string UnescapeFromFile(string text)
        {
            return text.Replace("\\|", "|").Replace("\\n", "\n").Replace("\\r", "\r");
        }
    }
}