using System.Data;

namespace Core.BusinessObject
{
    /// <summary>
    /// Employee business object.
    /// </summary>
    public class Employee : BaseObject
    {
        /// <summary>
        /// Gets or sets employee name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets employee email.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets employee password.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets employee phone.
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// Gets or sets employee date of birth.
        /// </summary>
        public DateTime? DOB { get; set; }

        /// <summary>
        /// Gets or sets employee status.
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Gets or sets profile image file name.
        /// </summary>
        public string? ProfilePic { get; set; }

        /// <summary>
        /// Gets or sets department id.
        /// </summary>
        public int? DepartmentId { get; set; }

        /// <summary>
        /// Gets or sets designation id.
        /// </summary>
        public int? DesignationId { get; set; }

        /// <summary>
        /// Gets or sets department name from join query.
        /// </summary>
        public string? DepartmentName { get; set; }

        /// <summary>
        /// Gets or sets designation name from join query.
        /// </summary>
        public string? DesignationName { get; set; }

        /// <summary>
        /// Initializes employee.
        /// </summary>
        public Employee()
        {
        }

        /// <summary>
        /// Initializes employee from database reader.
        /// </summary>
        public Employee(IDataReader reader) : base(reader)
        {
            Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString()! : string.Empty;
            Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString()! : string.Empty;
            Password = reader["Password"] != DBNull.Value ? reader["Password"].ToString()! : string.Empty;
            Phone = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : null;
            DOB = reader["DOB"] != DBNull.Value ? Convert.ToDateTime(reader["DOB"]) : null;
            Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : null;
            ProfilePic = reader["ProfilePic"] != DBNull.Value ? reader["ProfilePic"].ToString() : null;

            DepartmentId = reader["DepartmentId"] != DBNull.Value ? Convert.ToInt32(reader["DepartmentId"]) : null;
            DesignationId = reader["DesignationId"] != DBNull.Value ? Convert.ToInt32(reader["DesignationId"]) : null;

            if (HasColumn(reader, "DepartmentName") && reader["DepartmentName"] != DBNull.Value)
            {
                DepartmentName = reader["DepartmentName"].ToString();
            }

            if (HasColumn(reader, "DesignationName") && reader["DesignationName"] != DBNull.Value)
            {
                DesignationName = reader["DesignationName"].ToString();
            }
        }

        /// <summary>
        /// Checks if reader has column.
        /// </summary>
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