using Core.BusinessObject;

namespace Core.Services
{
    /// <summary>
    /// WFH service contract.
    /// </summary>
    public interface IWFHService
    {
        OperationResult Add(WFH wfh);

        List<WFH> GetAll();
    }
}