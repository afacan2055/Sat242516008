namespace Sat242516008.Models; // Namespace'i kendi projene göre ayarla

public class Product
{
    // Veritabanındaki [Products] tablosundaki sütunlarla birebir aynı isimde olmalı
    public int ProductID { get; set; }
    public string ProductName { get; set; }
    public string ProductCode { get; set; }
    public string Category { get; set; }
    public bool IsActive { get; set; }

    // Extensions_DataTable logiği null gelirse patlamasın diye nullable yapabiliriz veya varsayılan değer atarız.
    public DateTime CreatedDate { get; set; }
}