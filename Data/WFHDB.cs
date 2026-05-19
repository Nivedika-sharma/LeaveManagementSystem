using Core.BusinessObject;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NLog;
using System.Data;
using System.Data.Common;

namespace Data
{
    /// <summary>
    /// Handles WFH database operations.
    /// </summary>
    public class WFHDB
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Adds WFH request.
        /// </summary>
        public static void Add(WFH wfh)
        {
            Database db = ConnectionFactory.CreateDatabase();

            using DbCommand cmd = db.GetSqlStringCommand(
                @"INSERT INTO wfh
                (EmployeeId, StartDate, EndDate, Reason, Duration, SupportingDocument,
                 CreatedBy, UpdatedBy, CreatedOnUTC, UpdatedOnUTC, IPAddress)
                VALUES
                (@EmployeeId, @StartDate, @EndDate, @Reason, @Duration, @SupportingDocument,
                 @CreatedBy, @UpdatedBy, @CreatedOnUTC, @UpdatedOnUTC, @IPAddress);
                SELECT LAST_INSERT_ID();"
            );

            db.AddInParameter(cmd, "@EmployeeId", DbType.Int32, wfh.EmployeeId);
            db.AddInParameter(cmd, "@StartDate", DbType.Date, wfh.StartDate);
            db.AddInParameter(cmd, "@EndDate", DbType.Date, wfh.EndDate);
            db.AddInParameter(cmd, "@Reason", DbType.String, wfh.Reason);
            db.AddInParameter(cmd, "@Duration", DbType.Decimal, wfh.Duration);
            db.AddInParameter(cmd, "@SupportingDocument", DbType.String, wfh.SupportingDocument);
            db.AddInParameter(cmd, "@CreatedBy", DbType.String, wfh.CreatedBy);
            db.AddInParameter(cmd, "@UpdatedBy", DbType.String, wfh.UpdatedBy);
            db.AddInParameter(cmd, "@CreatedOnUTC", DbType.DateTime, wfh.CreatedOnUTC);
            db.AddInParameter(cmd, "@UpdatedOnUTC", DbType.DateTime, wfh.UpdatedOnUTC);
            db.AddInParameter(cmd, "@IPAddress", DbType.String, wfh.IPAddress);

            wfh.Id = Convert.ToInt32(db.ExecuteScalar(cmd));

            Log.Debug($"WFH request added. Id: {wfh.Id}");
        }

        /// <summary>
        /// Gets all WFH requests with employee name.
        /// </summary>
        public static List<WFH> GetAll()
        {
            Database db = ConnectionFactory.CreateDatabase();

            using DbCommand cmd = db.GetSqlStringCommand(
                @"SELECT 
                    w.*,
                    e.Name AS EmployeeName
                  FROM wfh w
                  INNER JOIN employee e ON w.EmployeeId = e.Id
                  ORDER BY w.Id DESC"
            );

            List<WFH> wfhList = new();

            using IDataReader reader = db.ExecuteReader(cmd);
            while (reader.Read())
            {
                wfhList.Add(new WFH(reader));
            }

            return wfhList;
        }
    }
}