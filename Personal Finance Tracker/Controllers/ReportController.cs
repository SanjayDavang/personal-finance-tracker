using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceTracker.Core.DTOs;
using PersonalFinanceTracker.Core.Interfaces;

namespace Personal_Finance_Tracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService; 
        public ReportController(IReportService reportService) 
        { 
            _reportService = reportService;
        }

        [HttpPost("Export")]
        [Authorize]
        public async Task<IActionResult> ExportPdf([FromQuery] string userName, [FromBody] ReportDto reportDto)
        {
            var pdf = await _reportService.GenerateReportPdfAsync(reportDto, userName);
            string fileName = GenerateFileName(reportDto);
            return File(pdf, "application/pdf", fileName);
        }

        private string GenerateFileName(ReportDto reportDto)
        {
            string fileName = "Finance_Report.pdf";

            if (reportDto.ReportType == "year" && reportDto.Year > 0)
            {
                fileName = $"{reportDto.Year}_Finance_Report.pdf";
            }
            else if (reportDto.ReportType == "month" && reportDto.Month.HasValue)
            {
                int year = reportDto.Year ?? DateTime.Now.Year;
                var monthName = new DateTime(year, reportDto.Month.Value, 1).ToString("MMMM");
                fileName = $"{monthName}_Finance_Report.pdf";
            }
            else if (reportDto.ReportType == "custom" && reportDto.FromDate.HasValue && reportDto.ToDate.HasValue)
            {
                var start = reportDto.FromDate.Value.ToString("dd_MMMM_yyyy");
                var end = reportDto.ToDate.Value.ToString("dd_MMMM_yyyy");
                fileName = $"{start}_to_{end}_Finance_Report.pdf";
            }

            return fileName;
        }
    }
}
