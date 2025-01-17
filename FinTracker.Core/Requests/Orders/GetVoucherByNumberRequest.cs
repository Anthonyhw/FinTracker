namespace FinTracker.Core.Requests.Orders
{
    public class GetVoucherByNumberRequest : Request
    {
        public string Code { get; set; } = string.Empty;
    }
}
