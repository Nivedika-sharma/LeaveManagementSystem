using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagementSystem.AppCode
{
    public static class HtmlHelpers
    {
        public static HtmlString DisplayDate(this IHtmlHelper helper, DateTime date)
        {
            if (date == DateTime.MinValue)
            {
                return new HtmlString(string.Empty);
            }

            return new HtmlString(date.ToString(Constants.DateFormat));
        }

        public static HtmlString DisplayDate(this IHtmlHelper helper, DateTime? date)
        {
            if (!date.HasValue)
            {
                return new HtmlString(string.Empty);
            }

            return new HtmlString(date.Value.ToString(Constants.DateFormat));
        }

        public static HtmlString DisplayStatus(this IHtmlHelper helper, string? status)
        {
            if (status == Constants.Active)
            {
                return new HtmlString($"<span class='badge bg-success'>{Constants.Active}</span>");
            }

            return new HtmlString($"<span class='badge bg-danger'>{Constants.InActive}</span>");
        }
    }
}