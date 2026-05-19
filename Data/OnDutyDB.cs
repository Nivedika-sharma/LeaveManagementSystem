using Core.BusinessObject;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NLog;
using System.Data;
using System.Data.Common;

namespace Data
{
    /// <summary>
    /// Handles OnDuty database operations.
    /// </summary>
    public class OnDutyDB
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Adds OnDuty request.
        /// </summary>
        public static void Add(OnDuty onDuty)
        {
            Database db = ConnectionFactory.CreateDatabase();

            using DbCommand cmd = db.GetSqlStringCommand(
                @"INSERT INTO onduty
                (EmployeeId, StartDate, EndDate, Reason, Duration, SupportingDocument,
                 CreatedBy, UpdatedBy, CreatedOnUTC, UpdatedOnUTC, IPAddress)
                VALUES
                (@EmployeeId, @StartDate, @EndDate, @Reason, @Duration, @SupportingDocument,
                 @CreatedBy, @UpdatedBy, @CreatedOnUTC, @UpdatedOnUTC, @IPAddress);
                SELECT LAST_INSERT_ID();"
            );

            db.AddInParameter(cmd, "@EmployeeId", DbType.Int32, onDuty.EmployeeId);
            db.AddInParameter(cmd, "@StartDate", DbType.Date, onDuty.StartDate);
            db.AddInParameter(cmd, "@EndDate", DbType.Date, onDuty.EndDate);
            db.AddInParameter(cmd, "@Reason", DbType.String, onDuty.Reason);
            db.AddInParameter(cmd, "@Duration", DbType.Decimal, onDuty.Duration);
            db.AddInParameter(cmd, "@SupportingDocument", DbType.String, onDuty.SupportingDocument);
            db.AddInParameter(cmd, "@CreatedBy", DbType.String, onDuty.CreatedBy);
            db.AddInParameter(cmd, "@UpdatedBy", DbType.String, onDuty.UpdatedBy);
            db.AddInParameter(cmd, "@CreatedOnUTC", DbType.DateTime, onDuty.CreatedOnUTC);
            db.AddInParameter(cmd, "@UpdatedOnUTC", DbType.DateTime, onDuty.UpdatedOnUTC);
            db.AddInParameter(cmd, "@IPAddress", DbType.String, onDuty.IPAddress);

            onDuty.Id = Convert.ToInt32(db.ExecuteScalar(cmd));

            Log.Debug($"OnDuty request added. Id: {onDuty.Id}");
        }

        /// <summary>
        /// Gets all OnDuty requests with employee name.
        /// </summary>
        public static List<OnDuty> GetAll()
        {
            Database db = ConnectionFactory.CreateDatabase();

            using DbCommand cmd = db.GetSqlStringCommand(
                @"SELECT 
                    od.*,
                    e.Name AS EmployeeName
                  FROM onduty od
                  INNER JOIN employee e ON od.EmployeeId = e.Id
                  ORDER BY od.Id DESC"
            );

            List<OnDuty> onDutyList = new();

            using IDataReader reader = db.ExecuteReader(cmd);
            while (reader.Read())
            {
                onDutyList.Add(new OnDuty(reader));
            }

            return onDutyList;
        }
    }
}