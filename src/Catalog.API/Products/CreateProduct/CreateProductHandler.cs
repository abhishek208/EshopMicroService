using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten;


namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Category, string Imagefile, decimal Price, string Description) :ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);

    public class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            Product product = new Product()
            {
                Name = request.Name,
                Category = request.Category,
                Description = request.Description,
                ImageFile = request.Imagefile,
                Price = request.Price
            };
            // business logic to create a project
            //throw new NotImplementedException();
            session.Store(product);
            await session.SaveChangesAsync();
            return new CreateProductResult(product.Id);
        }
    }
}
