namespace Infrastructure.Filters
{
    // public class UserAccessLogFilter : IAsyncActionFilter
    // {
    //     private readonly AccessLogService _accessLogService;
    //
    //     public UserAccessLogFilter(AccessLogService accessLogService)
    //     {
    //         _accessLogService = accessLogService;
    //     }
    //     public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    //     {
    //         // Do something before the action executes.
    //
    //         // next() calls the action method.
    //         await _accessLogService.AddAccessLogAsync();
    //
    //         await next();
    //         // resultContext.IsSigned is set.
    //         // Do something after the action executes.
    //     }
    // }
}
