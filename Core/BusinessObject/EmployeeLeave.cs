using System.Data;

namespace Core.BusinessObject
{
    /// <summary>
    /// Employee leave business object.
    /// </summary>
    public class EmployeeLeave : BaseObject
    {
        public int EmployeeId { get; set; }

        public int LeaveCategoryId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string? Reason { get; set; }

        public decimal Duration { get; set; }

        public string? SupportingDocument { get; set; }

        public string? EmployeeName { get; set; }

        public string? LeaveCategoryTitle { get; set; }

        /// <summary>
        /// Gets or sets leave status.
        /// </summary>
        public string Status { get; set; } = string.Empty;

        public EmployeeLeave()
        {
        }

        public EmployeeLeave(IDataReader reader) : base(reader)
        {
            EmployeeId =
                Convert.ToInt32(reader["EmployeeId"]);

            LeaveCategoryId =
                Convert.ToInt32(reader["LeaveCategoryId"]);

            StartDate =
                Convert.ToDateTime(reader["StartDate"]);

            EndDate =
                Convert.ToDateTime(reader["EndDate"]);

            Reason =
                reader["Reason"] != DBNull.Value
                ? reader["Reason"].ToString()
                : null;

            Duration =
                reader["Duration"] != DBNull.Value
                ? Convert.ToDecimal(reader["Duration"])
                : 0;

            SupportingDocument =
                reader["SupportingDocument"] != DBNull.Value
                ? reader["SupportingDocument"].ToString()
                : null;

            Status = reader["Status"] != DBNull.Value
    ? reader["Status"].ToString()!
    : string.Empty;

            if (HasColumn(reader, "EmployeeName"))
            {
                EmployeeName =
                    reader["EmployeeName"].ToString();
            }

            if (HasColumn(reader, "LeaveCategoryTitle"))
            {
                LeaveCategoryTitle =
                    reader["LeaveCategoryTitle"].ToString();
            }
        }

        private static bool HasColumn(
            IDataReader reader,
            string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i)
                    .Equals(columnName,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}