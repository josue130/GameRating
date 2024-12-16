
using FluentValidation;

namespace GameRatingAPI.Filter
{
    public class ValidationFilter<T> : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();

            if (validator is null)
            {
                return await next(context);
            }

            var entitie = context.Arguments.OfType<T>().FirstOrDefault();

            if (entitie is null)
            {
                return TypedResults.Problem("The entity to be validated could not be found");
            }

            var result = await validator.ValidateAsync(entitie);

            if (!result.IsValid)
            {
                return TypedResults.ValidationProblem(result.ToDictionary());
            }

            return await next(context);
        }
    }
}
