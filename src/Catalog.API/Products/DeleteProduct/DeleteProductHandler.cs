﻿using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Catalog.API.Products.GetProductById;
using Marten;

namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
    public record DeleteProductResult(bool IsSuccess);
    public class DeleteProductHandler(IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
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