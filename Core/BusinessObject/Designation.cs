using System.Data;

namespace Core.BusinessObject
{
    /// <summary>
    /// Designation business object.
    /// </summary>
    public class Designation : BaseObject
    {
        /// <summary>
        /// Gets or sets designation name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets designation description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Initializes designation.
        /// </summary>
        public Designation()
        {
        }

        /// <summary>
        /// Initializes designation from database reader.
        /// </summary>
        public Designation(IDataReader reader) : base(reader)
        {
            Name = reader["Name"] != DBNull.Value
                ? reader["Name"].ToString()!
                : string.Empty;

            Description = reader["Description"] != DBNull.Value
                ? reader["Description"].ToString()
                : null;
        }
    }
}