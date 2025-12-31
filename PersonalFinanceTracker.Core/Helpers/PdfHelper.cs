using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using PersonalFinanceTracker.Core.Models;

namespace PersonalFinanceTracker.Infrastructure.Helpers
{
    public static class PdfHelper
    {
        public static byte[] CreatePdf(decimal income, decimal expense, decimal balance, List<Transaction> transactions)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(20);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    // Header
                    page.Header().Column(header =>
                    {
                        header.Item().PaddingBottom(5).AlignCenter().Text("Personal Finance Tracker")
                            .Bold().FontSize(20).FontColor(Colors.Blue.Medium);

                        header.Item().AlignCenter().Text("Finance Report")
                            .FontSize(16).Bold();
                        header.Item().PaddingVertical(10).Element(x => x.LineHorizontal(1).LineColor(Colors.Grey.Lighten2));

                        header.Item().AlignCenter().Row(row =>
                        {
                            row.Spacing(25);
                            row.ConstantColumn(180).Text($"💰 Income: ₹{income:F2}")
                                .FontColor(Colors.Green.Medium);
                            row.ConstantColumn(200).Text($"💸 Expense: ₹{expense:F2}")
                                .FontColor(Colors.Red.Medium);
                            row.ConstantColumn(200).Text($"🧾 Balance: ₹{balance:F2}")
                                .FontColor(Colors.Blue.Medium);
                        });
                    });

                    // Main Content - Styled Table Card
                    page.Content().Element(container =>
                    {
                        container
                            .PaddingVertical(10)
                            .PaddingHorizontal(15)
                            .Background(Colors.Grey.Lighten4)
                            .CornerRadius(8)
                            .Table(table =>
                            {
                                // Define columns
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(); // Date
                                    columns.RelativeColumn(2); // Description
                                    columns.RelativeColumn(); // Type
                                    columns.RelativeColumn(); // Amount
                                });

                                // Header row
                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text("Date").Bold();
                                    header.Cell().Element(CellStyle).Text("Description").Bold();
                                    header.Cell().Element(CellStyle).Text("Type").Bold();
                                    header.Cell().Element(CellStyle).Text("Amount").Bold();

                                    static IContainer CellStyle(IContainer container)
                                    {
                                        return container
                                            .PaddingVertical(5)
                                            .PaddingHorizontal(10)
                                            .Background(Colors.Blue.Lighten3)
                                            .BorderBottom(1)
                                            .BorderColor(Colors.Grey.Medium);
                                    }
                                });

                                // Data rows
                                foreach (var tx in transactions)
                                {
                                    table.Cell().Element(CellStyle).Text(tx.Date.ToString("dd-MM-yyyy"));
                                    table.Cell().Element(CellStyle).Text(tx.Description);
                                    table.Cell().Element(CellStyle).Text(tx.TransactionType)
                                        .FontColor(tx.TransactionType == "Income" ? Colors.Green.Medium : Colors.Red.Medium);
                                    table.Cell().Element(CellStyle).Text($"₹{tx.Amount:F2}")
                                        .FontColor(tx.TransactionType == "Income" ? Colors.Green.Medium : Colors.Red.Medium);
                                }

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container
                                        .PaddingVertical(4)
                                        .PaddingHorizontal(10)
                                        .Background(Colors.White);
                                }
                            });
                    });

                    // Footer
                    page.Footer()
                        .AlignCenter()
                        .Text($"Generated by Personal Finance Tracker | {DateTime.Now:dd MMM yyyy}")
                        .FontSize(10)
                        .FontColor(Colors.Grey.Darken2);
                });
            });
            // Generate PDF
            using var stream = new MemoryStream();
            document.GeneratePdf(stream);
            return stream.ToArray();

        }
    }
}
