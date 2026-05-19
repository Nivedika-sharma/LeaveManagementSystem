using System.Data;

namespace Core
{
    public class BaseObject
    {
        public int Id { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime CreatedOnUTC { get; set; }

        public DateTime UpdatedOnUTC { get; set; }

        public string? IPAddress { get; set; }

        public BaseObject()
        {
        }

        public BaseObject(IDataReader reader)
        {
            Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0;
            CreatedBy = reader["CreatedBy"] != DBNull.Value ? reader["CreatedBy"].ToString() : null;
            UpdatedBy = reader["UpdatedBy"] != DBNull.Value ? reader["UpdatedBy"].ToString() : null;
            CreatedOnUTC = reader["CreatedOnUTC"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedOnUTC"]) : DateTime.MinValue;
            UpdatedOnUTC = reader["UpdatedOnUTC"] != DBNull.Value ? Convert.ToDateTime(reader["UpdatedOnUTC"]) : DateTime.MinValue;
            IPAddress = reader["IPAddress"] != DBNull.Value ? reader["IPAddress"].ToString() : null;
        }
    }
}