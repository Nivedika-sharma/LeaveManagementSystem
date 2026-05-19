using System.Data;

namespace Core.BusinessObject
{
    /// <summary>
    /// Department business object.
    /// </summary>
    public class Department : BaseObject
    {
        /// <summary>
        /// Gets or sets department name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets department description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Initializes a new department.
        /// </summary>
        public Department()
        {
        }

        /// <summary>
        /// Initializes department from database reader.
        /// </summary>
        public Department(IDataReader reader) : base(reader)
        {
            Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString()! : string.Empty;
            Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null;
        }
    }
}