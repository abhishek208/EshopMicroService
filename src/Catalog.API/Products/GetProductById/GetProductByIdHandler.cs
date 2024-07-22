using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using Marten;
using Marten.Linq.QueryHandlers;

namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdQuery(Guid? id):IQuery<GetProductByIdResult>;
        public record GetProductByIdResult(Product Product);
    public class GetProductByIdQueryHandler(IDocumentSession session,ILogger<GetProductByIdQueryHandler> logger) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"GetProductByIdQuery===>{request}");
            var product = await session.LoadAsync<Product>(request.id,cancellationToken);
            //if(product == null)
            //{
            //    throw new ProductNotFoundException();
            //}

            return new GetProductByIdResult(product);
        }
    }
}
