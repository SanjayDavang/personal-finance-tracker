using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PersonalFinanceTracker.Core.Models;

namespace PersonalFinanceTracker.Infrastructure.Helpers
{
    public static class PdfHelper
    {
        public static byte[] CreatePdf(decimal income, decimal expense, decimal balance, List<Transaction> transactions)
        {
            using var stream = new MemoryStream();

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Text("Finance Report").FontSize(20).Bold().AlignCenter();
                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Income: ₹{income}");
                        col.Item().Text($"Expense: ₹{expense}");
                        col.Item().Text($"Balance: ₹{balance}");

                        col.Item().LineHorizontal(1);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(100);
                                columns.RelativeColumn();
                                columns.ConstantColumn(100);
                                columns.ConstantColumn(80);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Date").Bold();
                                header.Cell().Text("Description").Bold();
                                header.Cell().Text("Type").Bold();
                                header.Cell().Text("Amount").Bold();
                            });

                            foreach (var t in transactions)
                            {
                                table.Cell().Text(t.Date.ToShortDateString());
                                table.Cell().Text(t.Description);
                                table.Cell().Text(t.TransactionType);
                                table.Cell().Text($"₹{t.Amount}");
                            }
                        });
                    });
                });
            }).GeneratePdf(stream);

            return stream.ToArray();
        }
    }
}
