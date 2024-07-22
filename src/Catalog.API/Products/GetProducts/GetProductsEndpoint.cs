using Catalog.API.Models;
using Catalog.API.Products.CreateProduct;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Catalog.API.Products.GetProducts
{   //public record GetProductrequest();
    public record GetProductResponse(IEnumerable<Product> Products);
    public class GetProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/Products", async (ISender sender) => {

                var result = await sender.Send(new GetProductsQuery());
                var products = result.Adapt<GetProductResponse>();
               
                return Results.Ok(products);
            
            }).WithName("GetProducts").Produces<GetProductResponse>(StatusCodes.Status200OK).ProducesProblem(StatusCodes.Status400BadRequest).WithSummary("Get Products").WithDescription("Get Products");
        }
    }
}
