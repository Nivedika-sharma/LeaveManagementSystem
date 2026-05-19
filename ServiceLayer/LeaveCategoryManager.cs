using Core;
using Core.BusinessObject;
using Core.Services;
using Data;
using NLog;

namespace ServiceLayer
{
    /// <summary>
    /// Handles leave category business logic.
    /// </summary>
    public class LeaveCategoryManager : ILeaveCategoryService
    {
        private static readonly Logger Log = LogManager.GetLogger("Service Layer");
        private static readonly Logger AuditLog = LogManager.GetLogger("Audit Log");

        /// <summary>
        /// Adds leave category after validation.
        /// </summary>
        public OperationResult Add(LeaveCategory leaveCategory)
        {
            Log.Debug("Leave category add process started.");

            if (string.IsNullOrWhiteSpace(leaveCategory.Title))
            {
                return new OperationResult(
                    (int)OperationStatus.Failure,
                    SLConstants.Messages.LeaveCategoryTitleRequired
                );
            }

            LeaveCategory? existingLeaveCategory = GetByTitle(leaveCategory.Title);

            if (existingLeaveCategory != null)
            {
                return new OperationResult(
                    (int)OperationStatus.Failure,
                    SLConstants.Messages.LeaveCategoryTitleUnique
                );
            }

            LeaveCategoryDB.Add(leaveCategory);

            Log.Info($"Leave category added. Id: {leaveCategory.Id}, Title: {leaveCategory.Title}");
            AuditLog.Info($"AUDIT: Leave category added. Id: {leaveCategory.Id}, By: {leaveCategory.CreatedBy}");

            return new OperationResult(
                (int)OperationStatus.Success,
                SLConstants.Messages.LeaveCategoryAddedSuccess,
                leaveCategory
            );
        }

        /// <summary>
        /// Gets all leave categories.
        /// </summary>
        public List<LeaveCategory> GetAll()
        {
            return LeaveCategoryDB.GetAll();
        }

        /// <summary>
        /// Gets leave category by title.
        /// </summary>
        public LeaveCategory? GetByTitle(string title)
        {
            return LeaveCategoryDB.GetByTitle(title);
        }
    }
}