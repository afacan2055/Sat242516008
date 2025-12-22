namespace MyDbModels;

public class Category
{
    // SQL'deki "Id" sütunu ile eşleşir
    public int Id { get; set; }

    // SQL'deki "CategoryName" sütunu ile birebir AYNI isimde olmalı
    public string CategoryName { get; set; } = string.Empty;


    // Provider'ın hata vermemesi için bu alanın olması gerekebilir
    public int TotalRecordCount { get; set; }
}