using Core.BusinessObject;

namespace Core.Services
{
    public interface ILeaveCategoryService
    {
        OperationResult Add(LeaveCategory leaveCategory);

        List<LeaveCategory> GetAll();

        LeaveCategory? GetByTitle(string title);
    }
}