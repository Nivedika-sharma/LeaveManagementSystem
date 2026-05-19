using Core.BusinessObject;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NLog;
using System.Data;
using System.Data.Common;

namespace Data
{
    /// <summary>
    /// Handles designation database operations.
    /// </summary>
    public class DesignationDB
    {
        private static readonly Logger Log =
            LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Adds designation.
        /// </summary>
        public static void Add(Designation designation)
        {
            Database db = ConnectionFactory.CreateDatabase();

            using DbCommand cmd = db.GetSqlStringCommand(
                @"INSERT INTO designation
                (
                    Name,
                    Description,
                    CreatedBy,
                    UpdatedBy,
                    CreatedOnUTC,
                    UpdatedOnUTC,
                    IPAddress
                )
                VALUES
                (
                    @Name,
                    @Description,
                    @CreatedBy,
                    @UpdatedBy,
                    @CreatedOnUTC,
                    @UpdatedOnUTC,
                    @IPAddress
                );

                SELECT LAST_INSERT_ID();"
            );

            db.AddInParameter(cmd, "@Name",
                DbType.String, designation.Name);

            db.AddInParameter(cmd, "@Description",
                DbType.String, designation.Description);

            db.AddInParameter(cmd, "@CreatedBy",
                DbType.String, designation.CreatedBy);

            db.AddInParameter(cmd, "@UpdatedBy",
                DbType.String, designation.UpdatedBy);

            db.AddInParameter(cmd, "@CreatedOnUTC",
                DbType.DateTime, designation.CreatedOnUTC);

            db.AddInParameter(cmd, "@UpdatedOnUTC",
                DbType.DateTime, designation.UpdatedOnUTC);

            db.AddInParameter(cmd, "@IPAddress",
                DbType.String, designation.IPAddress);

            designation.Id =
                Convert.ToInt32(db.ExecuteScalar(cmd));

            Log.Debug($"Designation added. Id: {designation.Id}");
        }

        /// <summary>
        /// Gets all designations.
        /// </summary>
        public static List<Designation> GetAll()
        {
            Database db = ConnectionFactory.CreateDatabase();

            using DbCommand cmd =
                db.GetSqlStringCommand(
                    "SELECT * FROM designation ORDER BY Id DESC"
                );

            List<Designation> designationList = new();

            using IDataReader reader = db.ExecuteReader(cmd);

            while (reader.Read())
            {
                designationList.Add(new Designation(reader));
            }

            return designationList;
        }

        /// <summary>
        /// Gets designation by id.
        /// </summary>
        public static Designation? GetById(int id)
        {
            Database db = ConnectionFactory.CreateDatabase();

            using DbCommand cmd =
                db.GetSqlStringCommand(
                    "SELECT * FROM designation WHERE Id=@Id"
                );

            db.AddInParameter(cmd, "@Id",
                DbType.Int32, id);

            using IDataReader reader = db.ExecuteReader(cmd);

            if (reader.Read())
            {
                return new Designation(reader);
            }

            return null;
        }

        /// <summary>
        /// Gets designation by name.
        /// </summary>
        public static Designation? GetByName(string name)
        {
            Database db = ConnectionFactory.CreateDatabase();

            using DbCommand cmd =
                db.GetSqlStringCommand(
                    "SELECT * FROM designation WHERE Name=@Name"
                );

            db.AddInParameter(cmd, "@Name",
                DbType.String, name);

            using IDataReader reader = db.ExecuteReader(cmd);

            if (reader.Read())
            {
                return new Designation(reader);
            }

            return null;
        }
    }
}