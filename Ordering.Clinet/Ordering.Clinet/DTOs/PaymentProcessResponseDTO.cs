namespace Ordering.Clinet.DTOs
{
    public class PaymentProcessResponseDTO
    {
        public bool PaymentStatus { get; set; } = false;
        public string? PaymentMessage { get; set; }
    }
}
