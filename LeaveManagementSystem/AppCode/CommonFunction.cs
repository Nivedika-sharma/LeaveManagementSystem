namespace LeaveManagementSystem.AppCode
{
    public static class CommonFunction
    {
        public static string GetCurrentUserName(HttpContext httpContext)
        {
            return httpContext.User.Identity?.Name ?? Constants.SystemUser;
        }

        public static string GetIpAddress(HttpContext httpContext)
        {
            return httpContext.Connection.RemoteIpAddress?.ToString() ?? Constants.UnknownIpAddress;
        }

        public static string GetUploadFolderPath()
        {
            return Path.Combine(
                Directory.GetCurrentDirectory(),
                Constants.WwwRoot,
                Constants.UploadFolder
            );
        }
    }
}