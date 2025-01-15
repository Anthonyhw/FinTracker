using FinTracker.Core.Enums;

namespace FinTracker.Core.Models
{
    public class Order
    {
        public long Id { get; set; }
        public string Code { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; } = String.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public string? ExternalReference { get; set; }
        public EPaymentGateway Gateway { get; set; } = EPaymentGateway.Stripe;

        public EOrderStatus Status { get; set; } = EOrderStatus.WaitingPayment;

        public long ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public long? VoucherId { get; set; }
        public Voucher? Voucher { get; set; }

        public decimal Total => Product.Price - (Voucher?.Amount ?? 0);

    }
}
