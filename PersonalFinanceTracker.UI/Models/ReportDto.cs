namespace PersonalFinanceTracker.UI.Models
{
    public class ReportDto
    {
        public string ReportType { get; set; }
        public int? Month {  get; set; }
        public int? Year { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

    }
}
