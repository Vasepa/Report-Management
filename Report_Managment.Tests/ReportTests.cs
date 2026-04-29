using Microsoft.VisualStudio.TestTools.UnitTesting;
using report_management;
using System;
using System.IO;

namespace Report_Management.Tests
{
    
    [TestClass]
    public class ReportTests
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
            if (File.Exists(OriginalFile))
                File.Delete(OriginalFile);
            if (File.Exists(BackupFile))
                File.Move(BackupFile, OriginalFile);
        }

        // +
        [TestMethod]
        public void AddReport_AddsReportToList()
        {
            var manager = new ReportManager();
            var report = new Report("Название", "Содержание", DateTime.Now);
            manager.AddReport(report);
            Assert.AreEqual(1, manager.Reports.Count);
        }

        [TestMethod]
        public void RemoveReport_RemovesFromList()
        {
            var manager = new ReportManager();
            var report = new Report("Удалить", "Данные", DateTime.Now);
            manager.AddReport(report);
            manager.RemoveReport(report);
            Assert.AreEqual(0, manager.Reports.Count);
        }


        // -
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddReport_NullReport_ThrowsArgumentNullException()
        {
            var manager = new ReportManager();
            manager.AddReport(null);
        }

    }
}
