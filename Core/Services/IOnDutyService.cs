using Core.BusinessObject;

namespace Core.Services
{
    /// <summary>
    /// OnDuty service contract.
    /// </summary>
    public interface IOnDutyService
    {
        OperationResult Add(OnDuty onDuty);

        List<OnDuty> GetAll();
    }
}