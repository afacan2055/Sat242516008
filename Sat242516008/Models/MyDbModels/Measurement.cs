namespace Sat242516008.Models.MyDbModels
{
    public class Measurement
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } // SQL Join ile gelecek
        public decimal MeasuredValue { get; set; }
        public decimal TargetValue { get; set; }
        public decimal Tolerance { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
