﻿namespace FinTracker.Core.Models
{
    public class Product
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public short ProductDuration { get; set; }
        public bool IsActive { get; set; }
        public decimal Price { get; set; }
    }
}
