using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten;
using Marten.Linq.QueryHandlers;
using System.Linq;

namespace Catalog.API.Products.CreateProduct.GetProductsByCategory
{
    public record GetProductByCategoryQuery(string category):IQuery<GetProductsByCategoryResult>;
    public record GetProductsByCategoryResult(IEnumerable<Product> Products);
    public class GetProductsByCategoryHandlerQuery(IDocumentSession session) : IQueryHandler<GetProductByCategoryQuery, GetProductsByCategoryResult>
    {
        public async  Task<GetProductsByCategoryResult> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
        {


            var Products = await session.Query<Product>().Where(p=>p.Category.Contains(request.category)).ToListAsync(cancellationToken);

            return new GetProductsByCategoryResult(Products);


        }
    }
}
