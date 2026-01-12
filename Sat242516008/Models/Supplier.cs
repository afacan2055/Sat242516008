namespace Sat242516008.Models;

public class Supplier
{
    public int SupplierID { get; set; }
    public string CompanyName { get; set; } = "";
    public string ContactName { get; set; } = "";
    public string Phone { get; set; } = "";
    public bool IsActive { get; set; } = true;
}