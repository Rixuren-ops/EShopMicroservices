﻿using Carter;
using Mapster;
using MediatR;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Queries.GetOrderByName;

namespace Ordering.API.EndPoints
{

    public record GetOrdersByNameResponse(IEnumerable<OrderDto> Orders);

    public class GetOrdersByName : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders/{name}", async (string name, ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersByNameQuery(name));
                var response = result.Adapt<GetOrdersByNameResponse>();
                return Results.Ok(response);
            })
            .WithName("GetOrdersByName")
            .Produces<GetOrdersByNameResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Orders By Name")
            .WithDescription("Get Orders By Name");
        }
    }
}
