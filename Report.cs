using System;

namespace report_management
{
    public class Report
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }

        public Report(string title, string content, DateTime creationDate)
        {
            Title = title;
            Content = content;
            CreationDate = creationDate;
        }
    }
}
