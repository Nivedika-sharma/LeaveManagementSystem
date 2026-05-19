using Core.BusinessObject;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NLog;
using System.Data;
using System.Data.Common;

namespace Data
{
    /// <summary>
    /// Handles department database operations.
    /// </summary>
    public class DepartmentDB
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Adds department into database.
        /// </summary>
        public static void Add(Department department)
        {
            Database db = ConnectionFactory.CreateDatabase();

            using DbCommand cmd = db.GetSqlStringCommand(
                "INSERT INTO department(Name, Description, CreatedBy, UpdatedBy, CreatedOnUTC, UpdatedOnUTC, IPAddress) " +
                "VALUES(@Name, @Description, @CreatedBy, @UpdatedBy, @CreatedOnUTC, @UpdatedOnUTC, @IPAddress); " +
                "SELECT LAST_INSERT_ID();"
            );

            db.AddInParameter(cmd, "@Name", DbType.String, department.Name);
            db.AddInParameter(cmd, "@Description", DbType.String, department.Description);
            db.AddInParameter(cmd, "@CreatedBy", DbType.String, department.CreatedBy);
            db.AddInParameter(cmd, "@UpdatedBy", DbType.String, department.UpdatedBy);
            db.AddInParameter(cmd, "@CreatedOnUTC", DbType.DateTime, department.CreatedOnUTC);
            db.AddInParameter(cmd, "@UpdatedOnUTC", DbType.DateTime, department.UpdatedOnUTC);
            db.AddInParameter(cmd, "@IPAddress", DbType.String, department.IPAddress);

            department.Id = Convert.ToInt32(db.ExecuteScalar(cmd));

            Log.Debug($"Department added in DB. Id: {department.Id}");
        }

        /// <summary>
        /// Gets all departments from database.
        /// </summary>
        public static List<Department> GetAll()
        {
            Database db = ConnectionFactory.CreateDatabase();

            using DbCommand cmd = db.GetSqlStringCommand(
                "SELECT Id, Name, Description, CreatedBy, UpdatedBy, CreatedOnUTC, UpdatedOnUTC, IPAddress " +
                "FROM department ORDER BY Name"
            );

            List<Department> departments = new();

            using IDataReader reader = db.ExecuteReader(cmd);
            while (reader.Read())
            {
                departments.Add(new Department(reader));
            }

            Log.Debug("Departments fetched from DB.");

            return departments;
        }

        /// <summary>
        /// Gets department by name.
        /// </summary>
        public static Department? GetByName(string name)
        {
            Database db = ConnectionFactory.CreateDatabase();

            using DbCommand cmd = db.GetSqlStringCommand(
                "SELECT Id, Name, Description, CreatedBy, UpdatedBy, CreatedOnUTC, UpdatedOnUTC, IPAddress " +
                "FROM department WHERE Name=@Name"
            );

            db.AddInParameter(cmd, "@Name", DbType.String, name);

            using IDataReader reader = db.ExecuteReader(cmd);
            if (reader.Read())
            {
                return new Department(reader);
            }

            return null;
        }
    }
}