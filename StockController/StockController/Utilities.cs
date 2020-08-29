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
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string PostCode { get; set; }
        public string Telephone { get; set; }
        public string ContactName { get; set; }
        public string Fax { get; set; }
        public string eMail { get; set; }
        public string WebsiteAddress { get; set; }
        public string Memo { get; set; }
        public decimal VATAmount { get; set; }
        public DateTime MovementDate { get; set; }
        public string DeliveryType { get; set; }
        public string Reference { get; set; }
        public int Qty { get; set; }
        public int TotalGainItems { get; set; }
        public int TotalLossItems { get; set; }
        public decimal Value { get; set; }
        public static string GetConnString(int ID)
        {
            // Returns the correct Database connection string for the identity record.
            if (ID == 1)
            {
                return @"Initial Catalog=FYPv2;Data Source=.\SQLEXPRESS;Persist Security Info=False;Integrated Security=true;";
            }
            else if (ID == 2)
            {
                return @"Initial Catalog=FYPv2;Data Source=192.168.1.200;Persist Security Info=False;Integrated Security=false;User ID=FYPUser;Password=@35Adc@*K9z&QJ";
            }
            else if (ID == 3)
            {
                return @"Initial Catalog=master;Data Source=.\SQLEXPRESS;Persist Security Info=False;Integrated Security=true;";
            }
            else
            {
                return @"Initial Catalog=master;Data Source=192.168.1.200;Persist Security Info=False;Integrated Security=false;User ID=FYPUser;Password=@35Adc@*K9z&QJ";
            }
        }
        public static DateTime GetSundayDate(DateTime DatePicked, int DateType)
        {
            // Gives the correct Sunday's Date depending on the function that is calling the function.
            if (DateType == 1)
            {
                return DatePicked.AddDays(0 - DatePicked.DayOfWeek);
            }
            else
            {
                return DatePicked.AddDays(0 - DatePicked.DayOfWeek + 7);
            }
        }
        public static string ChangeCase(string TextToConvert, int TextType)
        {
            if (TextType == 1)
            {
                return TextToConvert.ToUpper();
            }
            else if (TextType == 2)
            {
                return TextToConvert.ToLower();
            }
            else
            {
                TextInfo textInfo = new CultureInfo("en-GB", false).TextInfo;
                return textInfo.ToTitleCase(TextToConvert);
            }
        }     
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));
                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();
                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);
                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
