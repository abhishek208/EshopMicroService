using BuildingBlocks.CQRS;
using Mapster;


namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductRequest(string Name, List<string> Category, string Imagefile, decimal Price, string Description) : ICommand<CreateProductResponse>;
    public record CreateProductResponse(Guid Id);
    public class CreateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/Products", async (CreateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateProductCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<CreateProductResponse>();
                return Results.Created($"/Products/{response.Id}", response);

            }).WithName("CreateProduct").Produces<CreateProductResponse>(StatusCodes.Status201Created).ProducesProblem(StatusCodes.Status400BadRequest).WithSummary("Create Product").WithDescription("Create Product");
        }
    }
}
