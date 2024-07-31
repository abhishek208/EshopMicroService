using BuildingBlocks.CQRS;
using Catalog.API.Models;
using FluentValidation;
using Marten;


namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Category, string Imagefile, decimal Price, string Description) :ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
                RuleFor(x=>x.Name).NotEmpty().WithMessage("Name is Required and cannot be empty");
                RuleFor(x=>x.Category).NotEmpty().WithMessage("Category is Required and cannot be empty");
                RuleFor(x=>x.Description).NotEmpty().WithMessage("Name is Required and cannot be empty");
                RuleFor(x=>x.Price).GreaterThan(0).WithMessage("Price is Required and should be greater than 0");
        }
    }
    public class CreateProductCommandHandler(IDocumentSession session,IValidator<CreateProductCommand> validator) : ICommandHandler<CreateProductCommand, CreateProductResult>
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
