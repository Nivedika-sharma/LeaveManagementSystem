using System.Data;

namespace Core.BusinessObject
{
    /// <summary>
    /// Leave category business object.
    /// </summary>
    public class LeaveCategory : BaseObject
    {
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsDocumentRequired { get; set; }

        public LeaveCategory()
        {
        }

        public LeaveCategory(IDataReader reader) : base(reader)
        {
            Title = reader["Title"] != DBNull.Value
                ? reader["Title"].ToString()!
                : string.Empty;

            Description = reader["Description"] != DBNull.Value
                ? reader["Description"].ToString()
                : null;

            IsDocumentRequired =
                reader["IsDocumentRequired"] != DBNull.Value
                && Convert.ToBoolean(reader["IsDocumentRequired"]);
        }
    }
}