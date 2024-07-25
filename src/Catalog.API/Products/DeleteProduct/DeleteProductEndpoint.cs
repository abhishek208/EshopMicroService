
using BuildingBlocks.CQRS;
using Catalog.API.Products.CreateProduct;
using Mapster;

namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductRequest(Guid Id):ICommand<DeleteProductResponse>;
    public record DeleteProductResponse(bool IsSuccess);

    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/Products/{id}", async (string id, ISender sender) =>
            {
                if (!Guid.TryParse(id, out var guid))
                {
                    return Results.BadRequest(new { error = "Invalid GUID format. Please provide a valid GUID." });
                }

                var result = await sender.Send(new DeleteProductCommand(guid));
                var response = result.Adapt<DeleteProductResponse>();
                return Results.Ok( response);

            }).WithName("DeleteProduct").Produces<DeleteProductResponse>(StatusCodes.Status200OK).ProducesProblem(StatusCodes.Status400BadRequest).WithSummary("Delete Product").WithDescription("Delete Product");
        }
    }
}
