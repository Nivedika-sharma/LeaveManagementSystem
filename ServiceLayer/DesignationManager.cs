using Core;
using Core.BusinessObject;
using Core.Services;
using Data;
using NLog;

namespace ServiceLayer
{
    /// <summary>
    /// Handles designation business logic.
    /// </summary>
    public class DesignationManager : IDesignationService
    {
        private static readonly Logger Log =
            LogManager.GetLogger("Service Layer");

        /// <summary>
        /// Adds designation.
        /// </summary>
        public OperationResult Add(Designation designation)
        {
            Log.Debug("Adding designation.");

            Designation? existingDesignation =
                GetByName(designation.Name);

            if (existingDesignation != null)
            {
                return new OperationResult(
                    (int)OperationStatus.Failure,
                    "Designation already exists."
                );
            }

            DesignationDB.Add(designation);

            return new OperationResult(
                (int)OperationStatus.Success,
                "Designation added successfully."
            );
        }

        /// <summary>
        /// Gets all designations.
        /// </summary>
        public List<Designation> GetAll()
        {
            return DesignationDB.GetAll();
        }

        /// <summary>
        /// Gets designation by id.
        /// </summary>
        public Designation? GetById(int id)
        {
            return DesignationDB.GetById(id);
        }

        /// <summary>
        /// Gets designation by name.
        /// </summary>
        public Designation? GetByName(string name)
        {
            return DesignationDB.GetByName(name);
        }
    }
}