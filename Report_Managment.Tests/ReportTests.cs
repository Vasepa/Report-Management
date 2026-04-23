using Microsoft.VisualStudio.TestTools.UnitTesting;
using report_management;
using System;

namespace Report_Management.Tests
{
    //+ тесты
    [TestClass]
    public class ReportTests
    {
        [TestMethod]
        public void Constructor_SetsTitleAndContentAndDate()
        {
            // Подготовка
            string title = "Мой отчёт";
            string content = "Текст отчёта...";
            DateTime date = new DateTime(2026, 4, 23);

            // Действие
            Report report = new Report(title, content, date);

            // Проверка
            Assert.AreEqual(title, report.Title);
            Assert.AreEqual(content, report.Content);
            Assert.AreEqual(date, report.CreationDate);
        }

        [TestMethod]
        public void CanChangeTitleAndContent()
        {
            // Подготовка
            Report report = new Report("Старое", "Старое содержание", DateTime.Now);

            // Действие
            report.Title = "Новое название";
            report.Content = "Новое содержание";

            // Проверка
            Assert.AreEqual("Новое название", report.Title);
            Assert.AreEqual("Новое содержание", report.Content);
        }
    }
}