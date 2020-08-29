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
        public bool SaveWarehouseRecordToDB()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = GetConnString(1);
                conn.Open();
                using (SqlCommand InsertCmd = new SqlCommand())
                {
                    InsertCmd.Connection = conn;
                    InsertCmd.CommandType = CommandType.Text;
                    InsertCmd.CommandText = "INSERT INTO tblWarehouses (WarehouseRef, WarehouseName, Address1, Address2, Address3, Address4 ,PostCode, ContactName, Telephone, WebSite, Fax, eMail, WarehouseType, Memo, CreatedBy, CreatedDate) VALUES (@WarehouseRef, @WarehouseName, @Address1, @Address2, @Address3, @Address4 , @PostCode, @ContactName, @Telephone, @WebSite, @Fax, @eMail, @WarehouseType, @Memo, @CreatedBy, @CreatedDate)";
                    InsertCmd.Parameters.AddWithValue("@WarehouseRef", WarehouseRef);
                    InsertCmd.Parameters.AddWithValue("@WarehouseName", WarehouseName);
                    InsertCmd.Parameters.AddWithValue("@Address1", AddressLine1);
                    InsertCmd.Parameters.AddWithValue("@Address2", AddressLine2);
                    InsertCmd.Parameters.AddWithValue("@Address3", AddressLine3);
                    InsertCmd.Parameters.AddWithValue("@Address4", AddressLine4);
                    InsertCmd.Parameters.AddWithValue("@PostCode", PostCode);
                    InsertCmd.Parameters.AddWithValue("@ContactName", ContactName);
                    InsertCmd.Parameters.AddWithValue("@Telephone", Telephone);
                    InsertCmd.Parameters.AddWithValue("@WebSite", WebsiteAddress);
                    InsertCmd.Parameters.AddWithValue("@Fax", Fax);
                    InsertCmd.Parameters.AddWithValue("@eMail", eMail);
                    InsertCmd.Parameters.AddWithValue("@WarehouseType", WarehouseType);
                    InsertCmd.Parameters.AddWithValue("@Memo", Memo);
                    InsertCmd.Parameters.AddWithValue("@CreatedBy", UserID);
                    InsertCmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    Result = (int)InsertCmd.ExecuteNonQuery();
                }
            }
            if (Result != 1)
            {
                SaveToDB = false;
            }
            else
            {
                SaveToDB = true;
            }
            return SaveToDB;
        }
        public bool UpdateWarehouseRecordInDB()
        {

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = GetConnString(1);
                conn.Open();
                using (SqlCommand UpdateCmd = new SqlCommand())
                {
                    UpdateCmd.Connection = conn;
                    UpdateCmd.CommandType = CommandType.Text;
                    UpdateCmd.CommandText = "UPDATE tblWarehouses SET WarehouseName = @WarehouseName, Address1 = @Address1, Address2 = @Address2, Address3 = @Address3, Address4 = @Address4, PostCode = @PostCode, ContactName = @ContactName, Telephone = Telephone, WebSite = @WebSite, Fax = @Fax, eMail = @eMail, Memo = @Memo, WarehouseType = @WarehouseType WHERE WarehouseRef = @WarehouseRef";
                    UpdateCmd.Parameters.AddWithValue("@WarehouseRef", WarehouseRef);
                    UpdateCmd.Parameters.AddWithValue("@WarehouseName", WarehouseName);
                    UpdateCmd.Parameters.AddWithValue("@Address1", AddressLine1);
                    UpdateCmd.Parameters.AddWithValue("@Address2", AddressLine1);
                    UpdateCmd.Parameters.AddWithValue("@Address3", AddressLine1);
                    UpdateCmd.Parameters.AddWithValue("@Address4", AddressLine1);
                    UpdateCmd.Parameters.AddWithValue("@PostCode", PostCode);
                    UpdateCmd.Parameters.AddWithValue("@ContactName", ContactName);
                    UpdateCmd.Parameters.AddWithValue("@Telephone", Telephone);
                    UpdateCmd.Parameters.AddWithValue("@WebSite", WebsiteAddress);
                    UpdateCmd.Parameters.AddWithValue("@Fax", Fax);
                    UpdateCmd.Parameters.AddWithValue("@eMail", eMail);
                    UpdateCmd.Parameters.AddWithValue("@WarehouseType", WarehouseType);
                    UpdateCmd.Parameters.AddWithValue("@Memo", Memo);
                    Result = (int)UpdateCmd.ExecuteNonQuery();
                }
            }
            if (Result != 1)
            {
                UpdateToDB = false;
            }
            else
            {
                UpdateToDB = true;
            }
            return UpdateToDB;
        }
        public bool DeleteWarehouseRecordFromDB()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = GetConnString(1);
                conn.Open();
                using (SqlCommand DeleteCmd = new SqlCommand())
                {
                    DeleteCmd.Connection = conn;
                    DeleteCmd.CommandType = CommandType.Text;
                    DeleteCmd.CommandText = "DELETE FROM tblWarehouses where WarehouseRef = @WarehouseRef";
                    DeleteCmd.Parameters.AddWithValue("@WarehouseRef", WarehouseRef);
                    Result = (int)DeleteCmd.ExecuteNonQuery();
                }
            }
            if (Result == 1)
            {
                DeleteFromDB = true;
            }
            else
            {
                DeleteFromDB = false;
            }
            return DeleteFromDB;
        }
    }
}
