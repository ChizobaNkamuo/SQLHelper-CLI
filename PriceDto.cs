public class PriceDto
{
    public DateTime PriceDate { get; set; }
    public decimal OpenPrice { get; set; }
    public decimal HighPrice { get; set; }
    public decimal LowPrice { get; set; }
    public decimal ClosePrice { get; set; }
    public int Volume { get; set; }
}
