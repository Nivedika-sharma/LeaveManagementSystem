using Core.BusinessObject;

namespace Core.Services
{
    /// <summary>
    /// Designation service contract.
    /// </summary>
    public interface IDesignationService
    {
        /// <summary>
        /// Adds designation.
        /// </summary>
        OperationResult Add(Designation designation);

        /// <summary>
        /// Gets all designations.
        /// </summary>
        List<Designation> GetAll();

        /// <summary>
        /// Gets designation by id.
        /// </summary>
        Designation? GetById(int id);

        /// <summary>
        /// Gets designation by name.
        /// </summary>
        Designation? GetByName(string name);
    }
}