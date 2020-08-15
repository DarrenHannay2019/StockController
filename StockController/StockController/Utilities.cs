namespace StockController
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    public class Utilities
    {
        // Global properties for use for the whole application
        public bool SaveToDB { get; set; }
        public bool UpdateToDB { get; set; }
        public bool DeleteFromDB { get; set; }
        public int Result { get; set; }
        public string WarehouseRef { get; set; }
        public string SupplierRef { get; set; }
        public string ShopRef { get; set; }
        public string SMovementType { get; set; }
        public string StockCode { get; set; }
        public int UserID { get; set; }

    }
}
