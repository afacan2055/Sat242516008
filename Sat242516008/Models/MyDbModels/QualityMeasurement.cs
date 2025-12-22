public class QualityMeasurement
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int ControlPointId { get; set; }
    public decimal MeasuredValue { get; set; }
    public bool IsOk { get; set; }
    public DateTime MeasuredDate { get; set; }
}