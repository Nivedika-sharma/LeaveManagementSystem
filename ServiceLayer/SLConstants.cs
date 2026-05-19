namespace ServiceLayer
{
    /// <summary>
    /// Service layer constants.
    /// </summary>
    public static class SLConstants
    {
        /// <summary>
        /// Service layer messages.
        /// </summary>
        public static class Messages
        {
            public const string DepartmentAddedSuccess = "Department has been added successfully.";
            public const string DepartmentNameRequired = "Department name is required.";
            public const string DepartmentNameUnique = "Department name should be unique.";

            public const string EmployeeAddedSuccess = "Employee has been added successfully.";
            public const string EmployeeNameRequired = "Employee name is required.";
            public const string EmployeeEmailRequired = "Employee email is required.";
            public const string EmployeePasswordRequired = "Employee password is required.";
            public const string EmployeeEmailUnique = "Employee email should be unique.";
            public const string EmployeeNameUnique = "Employee name should be unique.";
            public const string EmployeePhoneLength = "Phone number should not exceed 12 digits.";

            public const string LeaveCategoryAddedSuccess = "Leave category has been added successfully.";
            public const string LeaveCategoryTitleRequired = "Leave category title is required.";
            public const string LeaveCategoryTitleUnique = "Leave category title should be unique.";

            public const string EmployeeLeaveAddedSuccess = "Leave has been applied successfully.";
            public const string EmployeeRequired = "Employee is required.";
            public const string LeaveCategoryRequired = "Leave category is required.";
            public const string LeaveStartDateRequired = "Start date is required.";
            public const string LeaveEndDateRequired = "End date is required.";
            public const string LeaveInvalidDateRange = "End date should be greater than or equal to start date.";
            public const string LeaveReasonRequired = "Reason is required.";

            public const string WFHAddedSuccess = "WFH request has been applied successfully.";
            public const string WFHInvalidDateRange = "End date should be greater than or equal to start date.";
            public const string WFHReasonRequired = "Reason is required.";

            public const string OnDutyAddedSuccess = "OnDuty request has been applied successfully.";
            public const string OnDutyInvalidDateRange = "End date should be greater than or equal to start date.";
            public const string OnDutyReasonRequired = "Reason is required.";

            public const string Pending = "Pending";
            public const string Approved = "Approved";
            public const string Rejected = "Rejected";

        }

    }
}