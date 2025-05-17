namespace Ordering.Clinet.DTOs
{
    public class OrderRequestDTO
    {
        public int OrderId { get; set; }
        public int UserID { get; set; }
        public List<ItemDTO> Items { get; set; } = [];
    }
}
