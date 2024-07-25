using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Catalog.API.Products.GetProductById;
using Marten;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductResult(bool IsSuccess);
    public record UpdateProductCommand(Guid Id,string Name, List<string> Category, string Imagefile, decimal Price, string Description) : ICommand<UpdateProductResult>;

    public class UpdateProductHandler(IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async  Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(request.Id, cancellationToken);
            if (product != null) {

                Product UpdateProduct = new Product
                {   Id= product.Id, 
                    Name= request.Name, 
                    Category= request.Category,
                    Description= request.Description, 
                    Price= request.Price,
                    ImageFile= request.Imagefile

                };
                       session.Update(UpdateProduct);
                await session.SaveChangesAsync(cancellationToken);

                return new UpdateProductResult(true);
            }
            return new UpdateProductResult(false);

        }
    }
}
