using Core.BusinessObject;

namespace Core.Services
{
    public interface IEmployeeLeaveService
    {
        OperationResult Add(EmployeeLeave employeeLeave);

        List<EmployeeLeave> GetAll();

        OperationResult UpdateStatus(int id, string status);

        EmployeeLeave? GetById(int id);
    }
}