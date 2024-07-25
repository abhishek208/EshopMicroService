using BuildingBlocks.CQRS;
using Catalog.API.Products.CreateProduct;
using Mapster;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductResponse(bool IsSuccess);
    public record UpdateProductRequest(Guid Id, string Name, List<string> Category, string Imagefile, decimal Price, string Description) : ICommand<UpdateProductResponse>;
    public class UpdateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/UpdateProduct", async (UpdateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateProductCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<UpdateProductResponse>();
                if (response.IsSuccess == false) {
                    return Results.BadRequest(new { error = $"Product With this ID:{request.Id} , Does not exists." });
                }
                return Results.Ok( response);

            }).WithName("UpdateProduct").Produces<UpdateProductResponse>(StatusCodes.Status200OK).ProducesProblem(StatusCodes.Status400BadRequest).WithSummary("Update Product").WithDescription("Update Product");
        }
    }
}
