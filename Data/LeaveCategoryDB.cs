using Core.BusinessObject;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NLog;
using System.Data;
using System.Data.Common;

namespace Data
{
    /// <summary>
    /// Handles leave category database operations.
    /// </summary>
    public class LeaveCategoryDB
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Adds leave category.
        /// </summary>
        public static void Add(LeaveCategory leaveCategory)
        {
            Database db = ConnectionFactory.CreateDatabase();

            using DbCommand cmd = db.GetSqlStringCommand(
                @"INSERT INTO leavecategory
                (Title, Description, IsDocumentRequired, CreatedBy, UpdatedBy, CreatedOnUTC, UpdatedOnUTC, IPAddress)
                VALUES
                (@Title, @Description, @IsDocumentRequired, @CreatedBy, @UpdatedBy, @CreatedOnUTC, @UpdatedOnUTC, @IPAddress);
                SELECT LAST_INSERT_ID();"
            );

            db.AddInParameter(cmd, "@Title", DbType.String, leaveCategory.Title);
            db.AddInParameter(cmd, "@Description", DbType.String, leaveCategory.Description);
            db.AddInParameter(cmd, "@IsDocumentRequired", DbType.Boolean, leaveCategory.IsDocumentRequired);
            db.AddInParameter(cmd, "@CreatedBy", DbType.String, leaveCategory.CreatedBy);
            db.AddInParameter(cmd, "@UpdatedBy", DbType.String, leaveCategory.UpdatedBy);
            db.AddInParameter(cmd, "@CreatedOnUTC", DbType.DateTime, leaveCategory.CreatedOnUTC);
            db.AddInParameter(cmd, "@UpdatedOnUTC", DbType.DateTime, leaveCategory.UpdatedOnUTC);
            db.AddInParameter(cmd, "@IPAddress", DbType.String, leaveCategory.IPAddress);

            leaveCategory.Id = Convert.ToInt32(db.ExecuteScalar(cmd));
            Log.Debug($"Leave category added. Id: {leaveCategory.Id}");
        }

        /// <summary>
        /// Gets all leave categories.
        /// </summary>
        public static List<LeaveCategory> GetAll()
        {
            Database db = ConnectionFactory.CreateDatabase();

            using DbCommand cmd = db.GetSqlStringCommand(
                "SELECT * FROM leavecategory ORDER BY Id DESC"
            );

            List<LeaveCategory> leaveCategoryList = new();

            using IDataReader reader = db.ExecuteReader(cmd);
            while (reader.Read())
            {
                leaveCategoryList.Add(new LeaveCategory(reader));
            }

            return leaveCategoryList;
        }

        /// <summary>
        /// Gets leave category by title.
        /// </summary>
        public static LeaveCategory? GetByTitle(string title)
        {
            Database db = ConnectionFactory.CreateDatabase();

            using DbCommand cmd = db.GetSqlStringCommand(
                "SELECT * FROM leavecategory WHERE Title=@Title"
            );

            db.AddInParameter(cmd, "@Title", DbType.String, title);

            using IDataReader reader = db.ExecuteReader(cmd);
            if (reader.Read())
            {
                return new LeaveCategory(reader);
            }

            return null;
        }
    }
}