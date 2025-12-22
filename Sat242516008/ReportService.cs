using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using MyDbModels;

namespace Services
{
    public class ReportService
    {
        public byte[] GenerateProductReport(List<Product> products, string title)
        {
            // QuestPDF Lisans ayarı (Community sürümü için gerekli)
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(50);
                    page.Header().Text(title).FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        // Sütun tanımları
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(100); // Ürün Kodu
                            columns.RelativeColumn();    // Ürün Adı
                            columns.ConstantColumn(100); // Tarih
                        });

                        // Başlık satırı
                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Kod");
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Ürün Adı");
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Tarih");
                        });

                        // Veri satırları
                        foreach (var item in products)
                        {
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.ProductCode);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.ProductName);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.CreatedDate.ToShortDateString());
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Sayfa ");
                        x.CurrentPageNumber();
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}