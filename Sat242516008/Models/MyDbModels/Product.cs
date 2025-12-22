using Attributes;
using System.ComponentModel.DataAnnotations;

namespace MyDbModels;

public class Product
{
    [Title("ID")]
    public int Id { get; set; }

    [Title("Kategori ID")]
    [Required]
    public int CategoryId { get; set; }

    [Title("Ürün Kodu")]
    [Required]
    [StringLength(50)]
    public string ProductCode { get; set; } = string.Empty;

    [Title("Ürün Adı")]
    [Required]
    [StringLength(200)]
    public string ProductName { get; set; } = string.Empty;

    [Title("Kayıt Tarihi")]
    public DateTime CreatedDate { get; set; }

    // SP'den dönen toplam kayıt sayısını yakalamak için
    public int TotalRecordCount { get; set; }
}