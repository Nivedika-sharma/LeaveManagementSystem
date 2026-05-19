using Core.BusinessObject;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NLog;
using System.Data;
using System.Data.Common;

namespace Data
{
    /// <summary>
    /// Handles employee leave database operations.
    /// </summary>
    public class EmployeeLeaveDB
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Adds employee leave.
        /// </summary>
        public static void Add(EmployeeLeave employeeLeave)
        {
            Database db = ConnectionFactory.CreateDatabase();

            using DbCommand cmd = db.GetSqlStringCommand(
                @"INSERT INTO employeeleave
                (
                    EmployeeId,
                    LeaveCategoryId,
                    StartDate,
                    EndDate,
                    Reason,
                    Duration,
                    SupportingDocument,
                    CreatedBy,
                    UpdatedBy,
                    CreatedOnUTC,
                    UpdatedOnUTC,
                    IPAddress
                )
                VALUES
                (
                    @EmployeeId,
                    @LeaveCategoryId,
                    @StartDate,
                    @EndDate,
                    @Reason,
                    @Duration,
                    @SupportingDocument,
                    @CreatedBy,
                    @UpdatedBy,
                    @CreatedOnUTC,
                    @UpdatedOnUTC,
                    @IPAddress
                );

                SELECT LAST_INSERT_ID();"
            );

            db.AddInParameter(cmd, "@EmployeeId", DbType.Int32, employeeLeave.EmployeeId);
            db.AddInParameter(cmd, "@LeaveCategoryId", DbType.Int32, employeeLeave.LeaveCategoryId);
            db.AddInParameter(cmd, "@StartDate", DbType.Date, employeeLeave.StartDate);
            db.AddInParameter(cmd, "@EndDate", DbType.Date, employeeLeave.EndDate);
            db.AddInParameter(cmd, "@Reason", DbType.String, employeeLeave.Reason);
            db.AddInParameter(cmd, "@Duration", DbType.Decimal, employeeLeave.Duration);
            db.AddInParameter(cmd, "@SupportingDocument", DbType.String, employeeLeave.SupportingDocument);
            db.AddInParameter(cmd, "@CreatedBy", DbType.String, employeeLeave.CreatedBy);
            db.AddInParameter(cmd, "@UpdatedBy", DbType.String, employeeLeave.UpdatedBy);
            db.AddInParameter(cmd, "@CreatedOnUTC", DbType.DateTime, employeeLeave.CreatedOnUTC);
            db.AddInParameter(cmd, "@UpdatedOnUTC", DbType.DateTime, employeeLeave.UpdatedOnUTC);
            db.AddInParameter(cmd, "@IPAddress", DbType.String, employeeLeave.IPAddress);

            employeeLeave.Id = Convert.ToInt32(db.ExecuteScalar(cmd));

            Log.Debug($"Employee leave added. Id: {employeeLeave.Id}");
        }

        /// <summary>
        /// Gets all employee leaves using join.
        /// </summary>
        public static List<EmployeeLeave> GetAll()
        {
            Database db = ConnectionFactory.CreateDatabase();

            using DbCommand cmd = db.GetSqlStringCommand(
                @"SELECT 
                    el.*,
                    e.Name AS EmployeeName,
                    lc.Title AS LeaveCategoryTitle
                  FROM employeeleave el
                  INNER JOIN employee e ON el.EmployeeId = e.Id
                  INNER JOIN leavecategory lc ON el.LeaveCategoryId = lc.Id
                  ORDER BY el.Id DESC"
            );



            List<EmployeeLeave> employeeLeaveList = new();

            using IDataReader reader = db.ExecuteReader(cmd);

            while (reader.Read())
            {
                employeeLeaveList.Add(new EmployeeLeave(reader));
            }

            return employeeLeaveList;
        }


        public static EmployeeLeave? GetById(int id)
        {
            Database db = ConnectionFactory.CreateDatabase();

            using DbCommand cmd = db.GetSqlStringCommand(
                @"SELECT 
            el.*,
            e.Name AS EmployeeName,
            lc.Title AS LeaveCategoryTitle
          FROM employeeleave el
          INNER JOIN employee e ON el.EmployeeId = e.Id
          INNER JOIN leavecategory lc ON el.LeaveCategoryId = lc.Id
          WHERE el.Id=@Id"
            );

            db.AddInParameter(cmd, "@Id", DbType.Int32, id);

            using IDataReader reader = db.ExecuteReader(cmd);

            if (reader.Read())
            {
                return new EmployeeLeave(reader);
            }

            return null;
        }

        public static void UpdateStatus(int id, string status)
        {
            Database db = ConnectionFactory.CreateDatabase();

            using DbCommand cmd = db.GetSqlStringCommand(
                "UPDATE employeeleave SET Status=@Status, UpdatedOnUTC=@UpdatedOnUTC WHERE Id=@Id"
            );

            db.AddInParameter(cmd, "@Id", DbType.Int32, id);
            db.AddInParameter(cmd, "@Status", DbType.String, status);
            db.AddInParameter(cmd, "@UpdatedOnUTC", DbType.DateTime, DateTime.UtcNow);

            db.ExecuteNonQuery(cmd);
        }
    }
}