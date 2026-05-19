using Core.BusinessObject;

namespace Core.Services
{
    /// <summary>
    /// Department service contract.
    /// </summary>
    public interface IDepartmentService
    {
        OperationResult Add(Department department);

        List<Department> GetAll();

        Department? GetByName(string name);
    }
}