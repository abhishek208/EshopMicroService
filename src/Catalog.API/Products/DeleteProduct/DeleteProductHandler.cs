using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Catalog.API.Products.GetProductById;
using FluentValidation;
using Marten;

namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
    public record DeleteProductResult(bool IsSuccess);

    public class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is Required");

        }
    }

    public class DeleteProductHandler(IDocumentSession session) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {

            var product = await session.LoadAsync<Product>(request.Id, cancellationToken);
            if (product != null)
            {
                session.Delete(product);
                await session.SaveChangesAsync(cancellationToken);

                return new DeleteProductResult(true);
            }
            return new DeleteProductResult(false);
        }
    }
}
