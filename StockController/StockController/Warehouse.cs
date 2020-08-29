namespace StockController
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
 
    public class Warehouse : Utilities
    {
        public string WarehouseName { get; set; }
        public string WarehouseType { get; set; }
        public Warehouse()
        {
            SaveToDB = false;
            UpdateToDB = false;
            DeleteFromDB = false;
        }
        ~Warehouse()
        {
            SaveToDB = false;
            UpdateToDB = false;
            DeleteFromDB = false;
        }

    }
}
