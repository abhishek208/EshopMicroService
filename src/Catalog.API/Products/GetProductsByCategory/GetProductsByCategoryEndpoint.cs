using Catalog.API.Models;
using Mapster;

namespace Catalog.API.Products.CreateProduct.GetProductsByCategory
{
    public record GetProductsByCategoryRequest(string Category);
    public record GetProductsByCategoryResponse(IEnumerable<Product> Products);
    public class GetProductsByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/GetProductsByCategory/{category}", async (ISender sender, string category) =>
            {
                var result = await sender.Send(new GetProductByCategoryQuery(category));

                if(result.Products == null) {
                    return Results.BadRequest(new { error = $"No products are available for category={category}" });

                }
                var products = result.Adapt<GetProductsByCategoryResponse>();
                return Results.Ok(products);

            });
        }
    }
}
