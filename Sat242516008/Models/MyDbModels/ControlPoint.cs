public class ControlPoint
{
    public int Id { get; set; }
    public string PointName { get; set; } = string.Empty;
    public decimal MinValue { get; set; }
    public decimal MaxValue { get; set; }
    public string Unit { get; set; } = string.Empty;
}