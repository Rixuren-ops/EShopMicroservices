
namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketResquest(ShoppingCart Cart);
    public record StoreBasketResponse(string UserName);


    public class StoreBasketEndPoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async (StoreBasketResquest request, ISender sender) =>
            {
                var command = request.Adapt<StoreBasketCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<StoreBasketResponse>();
                return Results.Created($"/basket/{response.UserName}", response);
            })
            .WithName("CreateProduct")
            .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Product")
            .WithDescription("Create Product");
        }
    }
}
