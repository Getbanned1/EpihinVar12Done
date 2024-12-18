using System;

namespace EpihinVar12
{
    internal class Product
    {
        public string ProductName { get; set; }
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        public string Description { get; set; }
        public int ProductID { get; internal set; }
        public string ImageUri { get; set; }
    }
}