using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Sat242516008.Models; // Model klasör yolun

namespace Sat242516008.Services;

public class ReportService
{
    public byte[] GenerateReport(List<Product> products, List<Category> categories)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Text("Kalite Kontrol Sistem Raporu")
                    .SemiBold().FontSize(24).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(x =>
                    {
                        // 1. BÖLÜM: KATEGORİLER TABLOSU
                        x.Item().Text("Tablo 1: Kategori Listesi").Bold().FontSize(16);

                        // HATA DÜZELTİLDİ: Colors.Grey.Light -> Colors.Grey.Lighten1
                        x.Item().PaddingBottom(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);

                        x.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            // Başlıklar
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("ID");
                                header.Cell().Element(CellStyle).Text("Kategori Adı");
                                header.Cell().Element(CellStyle).Text("Açıklama");
                            });

                            // Veriler
                            foreach (var item in categories)
                            {
                                table.Cell().Element(CellStyle).Text(item.CategoryID);
                                table.Cell().Element(CellStyle).Text(item.CategoryName);
                                table.Cell().Element(CellStyle).Text(item.Description);
                            }
                        });

                        x.Item().PaddingVertical(20); // Boşluk

                        // 2. BÖLÜM: ÜRÜNLER TABLOSU
                        x.Item().Text("Tablo 2: Ürün Listesi").Bold().FontSize(16);

                        // HATA DÜZELTİLDİ: Colors.Grey.Light -> Colors.Grey.Lighten1
                        x.Item().PaddingBottom(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);

                        x.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("ID");
                                header.Cell().Element(CellStyle).Text("Ürün Adı");
                                header.Cell().Element(CellStyle).Text("Kod");
                                header.Cell().Element(CellStyle).Text("Kategori");
                            });

                            foreach (var item in products)
                            {
                                table.Cell().Element(CellStyle).Text(item.ProductID);
                                table.Cell().Element(CellStyle).Text(item.ProductName);
                                table.Cell().Element(CellStyle).Text(item.ProductCode);
                                table.Cell().Element(CellStyle).Text(item.Category);
                            }
                        });
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Sayfa ");
                        x.CurrentPageNumber();
                    });
            });
        })
        .GeneratePdf();
    }

    // Tablo Hücre Stili
    static IContainer CellStyle(IContainer container)
    {
        // HATA DÜZELTİLDİ: Colors.Grey.Lighten2 (veya Lighten1) kullanılabilir
        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
    }
}