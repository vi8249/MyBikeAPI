using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace YouBikeAPI.Extensions
{
    public static class ApiBehaviorExtensions
    {
        public static void AddValidationServices(this IServiceCollection services)
        {
            services.AddControllers().ConfigureApiBehaviorOptions(delegate (ApiBehaviorOptions setupAction)
            {
                setupAction.InvalidModelStateResponseFactory = delegate (ActionContext context)
                {
                    ValidationProblemDetails validationProblemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Title = "資料驗證失敗",
                        Status = 422,
                        Detail = "請看下方詳細說明",
                        Instance = (string)context.HttpContext.Request.Path
                    };
                    validationProblemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);
                    return new UnprocessableEntityObjectResult(validationProblemDetails)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            });
        }
    }
}