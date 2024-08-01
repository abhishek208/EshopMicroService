using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using Catalog.API.Products.GetProductById;
using FluentValidation;
using Marten;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductResult(bool IsSuccess);
    public record UpdateProductCommand(Guid Id,string Name, List<string> Category, string Imagefile, decimal Price, string Description) : ICommand<UpdateProductResult>;

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is Required");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is Required and cannot be empty");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price is Required and should be greater than 0");
        }

    }
    public class UpdateProductHandler(IDocumentSession session) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
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
            else
            {
                throw new ProductNotFoundException((Guid)request.Id);
            }
            return new UpdateProductResult(false);

        }
    }
}
