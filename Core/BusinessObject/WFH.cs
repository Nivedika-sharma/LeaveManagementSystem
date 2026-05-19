using System.Data;

namespace Core.BusinessObject
{
    /// <summary>
    /// Work from home business object.
    /// </summary>
    public class WFH : BaseObject
    {
        public int EmployeeId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string? Reason { get; set; }

        public decimal Duration { get; set; }

        public string? SupportingDocument { get; set; }

        public string? EmployeeName { get; set; }

        public WFH()
        {
        }

        public WFH(IDataReader reader) : base(reader)
        {
            EmployeeId = Convert.ToInt32(reader["EmployeeId"]);
            StartDate = Convert.ToDateTime(reader["StartDate"]);
            EndDate = Convert.ToDateTime(reader["EndDate"]);
            Reason = reader["Reason"] != DBNull.Value ? reader["Reason"].ToString() : null;
            Duration = reader["Duration"] != DBNull.Value ? Convert.ToDecimal(reader["Duration"]) : 0;
            SupportingDocument = reader["SupportingDocument"] != DBNull.Value ? reader["SupportingDocument"].ToString() : null;

            if (HasColumn(reader, "EmployeeName") && reader["EmployeeName"] != DBNull.Value)
            {
                EmployeeName = reader["EmployeeName"].ToString();
            }
        }

        private static bool HasColumn(IDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}