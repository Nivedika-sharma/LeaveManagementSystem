using Core.BusinessObject;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NLog;
using System.Data;
using System.Data.Common;

namespace Data
{
    /// <summary>
    /// Handles employee database operations.
    /// </summary>
    public class EmployeeDB
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Adds employee into database.
        /// </summary>
        public static void Add(Employee employee)
        {
            Database db = ConnectionFactory.CreateDatabase();

            using DbCommand cmd = db.GetSqlStringCommand(
                "INSERT INTO employee(Name, Email, Password, Phone, DOB, Status, ProfilePic, DepartmentId, DesignationId, CreatedBy, UpdatedBy, CreatedOnUTC, UpdatedOnUTC, IPAddress) " +
                "VALUES(@Name, @Email, @Password, @Phone, @DOB, @Status, @ProfilePic, @DepartmentId, @DesignationId, @CreatedBy, @UpdatedBy, @CreatedOnUTC, @UpdatedOnUTC, @IPAddress); " +
                "SELECT LAST_INSERT_ID();"
            );

            db.AddInParameter(cmd, "@Name", DbType.String, employee.Name);
            db.AddInParameter(cmd, "@Email", DbType.String, employee.Email);
            db.AddInParameter(cmd, "@Password", DbType.String, employee.Password);
            db.AddInParameter(cmd, "@Phone", DbType.String, employee.Phone);
            db.AddInParameter(cmd, "@DOB", DbType.Date, employee.DOB);
            db.AddInParameter(cmd, "@Status", DbType.String, employee.Status);
            db.AddInParameter(cmd, "@ProfilePic", DbType.String, employee.ProfilePic);
            db.AddInParameter(cmd, "@DepartmentId", DbType.Int32, employee.DepartmentId);
            db.AddInParameter(cmd, "@DesignationId", DbType.Int32, employee.DesignationId);
            db.AddInParameter(cmd, "@CreatedBy", DbType.String, employee.CreatedBy);
            db.AddInParameter(cmd, "@UpdatedBy", DbType.String, employee.UpdatedBy);
            db.AddInParameter(cmd, "@CreatedOnUTC", DbType.DateTime, employee.CreatedOnUTC);
            db.AddInParameter(cmd, "@UpdatedOnUTC", DbType.DateTime, employee.UpdatedOnUTC);
            db.AddInParameter(cmd, "@IPAddress", DbType.String, employee.IPAddress);

            employee.Id = Convert.ToInt32(db.ExecuteScalar(cmd));

            Log.Debug($"Employee added in DB. Id: {employee.Id}");
        }

        /// <summary>
        /// Gets all employees using join query.
        /// </summary>
        public static List<Employee> GetAll()
        {
            Database db = ConnectionFactory.CreateDatabase();

            using DbCommand cmd = db.GetSqlStringCommand(
                @"SELECT 
                    e.*,
                    d.Name AS DepartmentName,
                    dg.Name AS DesignationName
                  FROM employee e
                  LEFT JOIN department d ON e.DepartmentId = d.Id
                  LEFT JOIN designation dg ON e.DesignationId = dg.Id
                  ORDER BY e.Id DESC"
            );

            List<Employee> employees = new();

            using IDataReader reader = db.ExecuteReader(cmd);
            while (reader.Read())
            {
                employees.Add(new Employee(reader));
            }

            return employees;
        }

        /// <summary>
        /// Gets employee by email.
        /// </summary>
        public static Employee? GetByEmail(string email)
        {
            Database db = ConnectionFactory.CreateDatabase();

            using DbCommand cmd = db.GetSqlStringCommand(
                "SELECT * FROM employee WHERE Email=@Email"
            );

            db.AddInParameter(cmd, "@Email", DbType.String, email);

            using IDataReader reader = db.ExecuteReader(cmd);
            if (reader.Read())
            {
                return new Employee(reader);
            }

            return null;
        }

        /// <summary>
        /// Gets employee by name.
        /// </summary>
        public static Employee? GetByName(string name)
        {
            Database db = ConnectionFactory.CreateDatabase();

            using DbCommand cmd = db.GetSqlStringCommand(
                "SELECT * FROM employee WHERE Name=@Name"
            );

            db.AddInParameter(cmd, "@Name", DbType.String, name);

            using IDataReader reader = db.ExecuteReader(cmd);
            if (reader.Read())
            {
                return new Employee(reader);
            }

            return null;
        }
    }
}