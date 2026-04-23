using Microsoft.VisualStudio.TestTools.UnitTesting;
using report_management;
using System;
using System.IO;

namespace Report_Management.Tests
{
    [TestClass]
    public class ReportManagerTests
    {
        private const string OriginalFile = "reports.txt";
        private const string BackupFile = "reports.txt.backup";

        [TestInitialize]
        public void Setup()
        {
            if (File.Exists(OriginalFile))
            {
                if (File.Exists(BackupFile))
                    File.Delete(BackupFile);
                File.Move(OriginalFile, BackupFile);
            }
        }

        [TestCleanup]
        public void Teardown()
        {
            // Удаляем файл, созданный тестами
            if (File.Exists(OriginalFile))
                File.Delete(OriginalFile);

            // Возвращаем оригинальный файл
            if (File.Exists(BackupFile))
                File.Move(BackupFile, OriginalFile);
        }

        [TestMethod]
        public void AddReport_AddsReportToList()
        {
            // Подготовка
            var manager = new ReportManager();
            var report = new Report("Название", "Содержание", DateTime.Now);

            // Действие
            manager.AddReport(report);

            // Проверка
            Assert.AreEqual(1, manager.Reports.Count);
            Assert.AreEqual("Название", manager.Reports[0].Title);
        }

        [TestMethod]
        public void RemoveReport_RemovesFromList()
        {
            // Подготовка
            var manager = new ReportManager();
            var report = new Report("Удалить", "Данные", DateTime.Now);
            manager.AddReport(report);
            Assert.AreEqual(1, manager.Reports.Count);

            // Действие
            manager.RemoveReport(report);

            // Проверка
            Assert.AreEqual(0, manager.Reports.Count);
        }
    }
}




