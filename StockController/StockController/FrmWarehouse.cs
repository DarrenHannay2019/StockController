using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockController
{
    public partial class FrmWarehouse : Form
    {
        #region Properties
        public int UserID { get; set; } // to hold the user's UserID
        public string FormMode { get; set; }    // to set the mode of the form
        public string WarehouseRef { get; set; } // to hold the first column of the selected record
        #endregion
        public FrmWarehouse()
        {
            InitializeComponent();
        }
        #region ControlCodes
        private void FrmWarehouse_Load(object sender, EventArgs e)
        {
            // Sets up the form
            // new = new record to be saved
            // old = record to be updated
            if (FormMode == "New")
            {
                CmdOK.Text = "Save"; // change the saving/updating button to reflect the mode of the form
                Text = "New Warehouse"; // change the text of the form to reflect the mode of the form
            }
            else
            {
                // updating form code
                CmdOK.Text = "OK";
                LoadData(); // gets the data from the database into the form.
            }
        }

        private void TxtWarehouseRef_Leave(object sender, EventArgs e)
        {

        }

        private void TxtWarehouseName_Leave(object sender, EventArgs e)
        {

        }

        private void TxtContactName_Leave(object sender, EventArgs e)
        {

        }

        private void TxtAddress1_Leave(object sender, EventArgs e)
        {

        }

        private void TxtAddress2_Leave(object sender, EventArgs e)
        {

        }

        private void TxtAddress3_Leave(object sender, EventArgs e)
        {

        }

        private void TxtAddress4_Leave(object sender, EventArgs e)
        {

        }

        private void TxtPostCode_Leave(object sender, EventArgs e)
        {

        }

        private void TxtWebsite_Leave(object sender, EventArgs e)
        {
            // check to see if website address is in the correct format
            if (TxtWebsite.TextLength != 0)
            {
                // https://stackoverflow.com/questions/3228984/a-better-way-to-validate-url-in-c-sharp-than-try-catch 
                // User https://stackoverflow.com/users/626273/stema
                string regular = @"^(ht|f|sf)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$";
                string regular123 = @"^(www.)[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$";

                string myString = TxtWebsite.Text.Trim();
                if (Regex.IsMatch(myString, regular))
                {
                    MessageBox.Show("It is valid url  " + myString);
                }
                else if (Regex.IsMatch(myString, regular123))
                {
                    MessageBox.Show("Valid url with www. " + myString);
                }
                else
                {
                    MessageBox.Show("InValid URL  " + myString);
                }
            }
            else
            {
                // do nothing
            }
        }

        private void TxteMail_Leave(object sender, EventArgs e)
        {
            // check the email address to see if correct format
            if (TxteMail.TextLength != 0)
            {
                if (Utilities.IsValidEmail(TxteMail.Text))
                    TxteMail.Text = Utilities.ChangeCase(TxteMail.Text, 2);
                else
                { TxteMail.Text = "Please Try Again"; }
            }
            else
            {
                // do nothing if email address has not been supplied.
            }
        }

        private void CmdOK_Click(object sender, EventArgs e)
        {
            // Saving the record to the database tblWarehouses and tblSysLog tables
            // New form = Save 
            // old form = update
            bool bResult;
            if (TxteMail.Text != "Please Try Again")
            {
                Warehouse objWarehouse = new Warehouse
                {
                    WarehouseRef = TxtWarehouseRef.Text.TrimEnd(),
                    WarehouseName = TxtWarehouseName.Text.TrimEnd(),
                    AddressLine1 = TxtAddress1.Text.TrimEnd(),
                    AddressLine2 = TxtAddress2.Text.TrimEnd(),
                    AddressLine3 = TxtAddress3.Text.TrimEnd(),
                    AddressLine4 = TxtAddress4.Text.TrimEnd(),
                    PostCode = TxtPostCode.Text.TrimEnd(),
                    Telephone = TxtTelephone1.Text.TrimEnd(),
                    ContactName = TxtContactName.Text.TrimEnd(),
                    eMail = TxteMail.Text.TrimEnd(),
                    Fax = TxtFax.Text.TrimEnd(),
                    Memo = TxtMemo.Text.TrimEnd(),
                    WarehouseType = cboWType.Text.TrimEnd(),
                    WebsiteAddress = TxtWebsite.Text.TrimEnd(),
                    UserID = UserID
                };
                if (FormMode == "New")
                {
                    bResult = objWarehouse.SaveWarehouseRecordToDB();
                    if(bResult)
                    {
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Error in saving to database", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    bResult = objWarehouse.UpdateWarehouseRecordInDB();
                    if (bResult)
                    {
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Error in Updating to database", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            
            }
            else
            {
                MessageBox.Show("Please enter a valid email address!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                TxteMail.SelectAll();
            }
        }

        private void CmdCancel_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("Are you sure you wish to cancel input?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
                this.Close();
            else
            {

            }
        }

        private void CmdClear_Click(object sender, EventArgs e)
        {
            ClearTextBoxes(this);   // removes all the text from each of the boxes on the form
        }
        #endregion
        #region PrivateFunctions
        private void LoadData()
        {
            // properties used for loading the data into the form.
            int QtyInStock = 0;
            decimal ValueInStock = 0.0m;
            // Loading the head table tblWarehouses into the form
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = Utilities.GetConnString(1);
                conn.Open();
                DataTable dtk = new DataTable();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                using (SqlCommand SelectCmd = new SqlCommand())
                {
                    SelectCmd.Connection = conn;
                    SelectCmd.CommandText = "SELECT * from tblWarehouses Where WarehouseRef = @WarehouseRef";
                    SelectCmd.Parameters.AddWithValue("@WarehouseRef", TxtWarehouseRef.Text.TrimEnd());
                    sqlDataAdapter.SelectCommand = SelectCmd;
                    sqlDataAdapter.Fill(dtk);
                }
                TxtWarehouseRef.Text = dtk.Rows[0][0].ToString();
                TxtWarehouseName.Text = dtk.Rows[0][1].ToString();
                TxtAddress1.Text = dtk.Rows[0][2].ToString();
                TxtAddress2.Text = dtk.Rows[0][3].ToString();
                TxtAddress3.Text = dtk.Rows[0][4].ToString();
                TxtAddress4.Text = dtk.Rows[0][5].ToString();
                TxtPostCode.Text = dtk.Rows[0][6].ToString();
                TxtTelephone1.Text = dtk.Rows[0][7].ToString();
                TxtFax.Text = dtk.Rows[0][8].ToString();
                TxteMail.Text = dtk.Rows[0][9].ToString();
                TxtWebsite.Text = dtk.Rows[0][10].ToString();
                cboWType.Text = dtk.Rows[0][11].ToString();
                TxtMemo.Text = dtk.Rows[0][12].ToString();
                TxtContactName.Text = dtk.Rows[0][13].ToString();
            }
            // Loading all the stock items for the selected warehouse
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = Utilities.GetConnString(1);
                conn.Open();
                DataTable dt = new DataTable();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                using (SqlCommand SelectCmd = new SqlCommand())
                {
                    SelectCmd.Connection = conn;
                    SelectCmd.CommandText = "SELECT StockCode, QtyHangers, Value From QryWarehouseStock Where LocationRef = @LocationRef AND QtyHangers <> '0' ORDER BY StockCode";
                    SelectCmd.Parameters.AddWithValue("@LocationRef", TxtWarehouseRef.Text.TrimEnd());
                    sqlDataAdapter.SelectCommand = SelectCmd;
                    sqlDataAdapter.Fill(dt);
                }
                gridStock.DataSource = dt;
                gridStock.AutoGenerateColumns = true;
                gridStock.CellBorderStyle = DataGridViewCellBorderStyle.None;
                gridStock.BackgroundColor = Color.LightCoral;
                gridStock.DefaultCellStyle.SelectionBackColor = Color.Red;
                gridStock.DefaultCellStyle.SelectionForeColor = Color.Yellow;
                gridStock.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
                gridStock.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                gridStock.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                gridStock.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                gridStock.AllowUserToResizeColumns = false;
                gridStock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                gridStock.RowsDefaultCellStyle.BackColor = Color.LightSkyBlue;
                gridStock.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow;
                gridStock.Columns[0].HeaderText = "Stock Code";
                gridStock.Columns[1].HeaderText = "Qty";
                gridStock.Columns[2].HeaderText = "Value";
                gridStock.Columns[2].DefaultCellStyle.Format = "C2";
            }
            // Loading all the transactions for the selected warehouse
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = Utilities.GetConnString(1);
                conn.Open();
                DataTable dt = new DataTable();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                using (SqlCommand SelectCmd = new SqlCommand())
                {
                    SelectCmd.Connection = conn;
                    SelectCmd.CommandText = "SELECT StockCode, MovementType, MovementQtyHangers, MovementDate, MovementReference from qryWarehouseTransactions WHERE LocationRef = @LocationRef Order By MovementDate";
                    SelectCmd.Parameters.AddWithValue("@LocationRef", TxtWarehouseRef.Text.TrimEnd());
                    sqlDataAdapter.SelectCommand = SelectCmd;
                    sqlDataAdapter.Fill(dt);
                }
                gridTrans.DataSource = dt;
                gridTrans.AutoGenerateColumns = true;
                gridTrans.CellBorderStyle = DataGridViewCellBorderStyle.None;
                gridTrans.BackgroundColor = Color.LightCoral;
                gridTrans.DefaultCellStyle.SelectionBackColor = Color.Plum;
                gridTrans.DefaultCellStyle.SelectionForeColor = Color.Yellow;
                gridTrans.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
                gridTrans.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                gridTrans.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                gridTrans.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                gridTrans.AllowUserToResizeColumns = false;
                gridTrans.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                gridTrans.RowsDefaultCellStyle.BackColor = Color.LightSkyBlue;
                gridTrans.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow;
                gridTrans.Columns[0].HeaderText = "Stock Code";
                gridTrans.Columns[1].HeaderText = "Type";
                gridTrans.Columns[2].HeaderText = "Qty";
                gridTrans.Columns[3].HeaderText = "Date";
                gridTrans.Columns[4].HeaderText = "Reference";
            }
            // Calculating the value and qty of products in the selected warehouse
            for (int i = 0; i < gridStock.Rows.Count; i++)
            {
                QtyInStock += Convert.ToInt32(gridStock.Rows[i].Cells[1].Value);
                ValueInStock += Convert.ToDecimal(gridStock.Rows[i].Cells[2].Value);
            }
            label9.Text = QtyInStock.ToString();
            TxtTotalValue.Text = ValueInStock.ToString("C2");
            // Changing the title of the form to the details of the warehouse
            this.Text = "Warehouse Details for [" + TxtWarehouseRef.Text.TrimEnd() + "] " + TxtWarehouseName.Text.TrimEnd();
        }
        private void ClearTextBoxes(Control control)
        {
            // empty all the text from the text boxes in the form.
            // Code from https://stackoverflow.com/questions/4811229/how-to-clear-the-text-of-all-textboxes-in-the-form
            foreach (Control c in control.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Clear();
                }
                if (c.HasChildren)
                {
                    ClearTextBoxes(c);
                }
            }
        }
        #endregion
    }
}
