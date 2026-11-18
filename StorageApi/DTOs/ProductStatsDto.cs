namespace StorageApi.DTOs
{
    public class ProductStatsDto
    {
        public long TotalProducts { get; set; }

        public long TotalInventoryValue { get; set; }

        public double AveragePrice { get; set; }
    }
}
