﻿namespace FinTracker.Core.Requests.Stripe
{
    public class CreateSessionRequest : Request
    {
        public string OrderNumber { get; set; } = string.Empty;
        public string ProductTitle { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;

        public short ProductDuration { get; set; }
        public long OrderTotal { get; set; }
    }
}
