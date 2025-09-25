using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Core.DTOs
{
    public class ReportDto
    {
        public string ReportType { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
