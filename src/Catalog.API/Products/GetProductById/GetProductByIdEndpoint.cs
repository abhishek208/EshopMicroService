using Catalog.API.Models;
using Catalog.API.Products.GetProducts;
using Mapster;

namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdRequest(Guid? id);
    public record GetProductByIdResponse(Product Product);
    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/GetProductById/{id}", async (ISender sender,string id) => {

                if (!Guid.TryParse(id, out var guid))
                {
                    return Results.BadRequest(new { error = "Invalid GUID format. Please provide a valid GUID." });
                }
                var result = await sender.Send(new GetProductByIdQuery(guid));
                if (result.Product == null)
                {
                    return Results.BadRequest(new { error = $"Product With this ID:{id} , Does not exists." });

                }
                var product = result.Adapt<GetProductByIdResponse>();

                return Results.Ok(product);

            }).WithName("GetProductById").Produces<GetProductResponse>(StatusCodes.Status200OK).ProducesProblem(StatusCodes.Status400BadRequest).WithSummary("GetProductById").WithDescription("GetProductById");
        }
    }
}
