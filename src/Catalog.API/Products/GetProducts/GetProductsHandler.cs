using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Mapster;
using Marten;

namespace Catalog.API.Products.GetProducts
{
    public record GetProductsQuery:IQuery<GetProductsResult>;
    public record GetProductsResult(IEnumerable<Product> Products);
    public class GetProductsQueryHandler(IDocumentSession session,ILogger<GetProductsQueryHandler> logger) : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"GetProductsQuery==>{request}");
            var products=await session.Query<Product>().ToListAsync(cancellationToken);
         
            return new GetProductsResult(products);
        }
    }
}
