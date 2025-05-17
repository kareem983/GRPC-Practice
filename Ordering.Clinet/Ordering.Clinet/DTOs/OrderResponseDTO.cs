namespace Ordering.Clinet.DTOs
{
    public class OrderResponseDTO
    {
        public int OrderID { get; set; }
        public bool OrderFinalStatus { get; set; } = false;
        public PaymentProcessResponseDTO? PaymentProcessResponse { get; set; }
        public InventoryProcessResponseDTO? InventoryProcessResponse { get; set; }
    }
}
