namespace FinTracker.Core.Requests.Stripe
{
    public class GetTransactionsByOrderNumberRequest
    {
        public string Number { get; set; } = string.Empty;
    }
}
