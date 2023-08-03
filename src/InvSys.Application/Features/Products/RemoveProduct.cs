using FluentResults;

using InvSys.Application.Common;
using InvSys.Application.Entities;
using InvSys.Application.Infrastructure;
using InvSys.Application.ValueObjects;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvSys.Application.Features.Products;

[Route("/api/product")]
public class RemoveProductController : ApiControllerBase
{
    [HttpDelete]
    public async Task<ActionResult> Remove([FromQuery] ProductRemoveCommand command)
    {
        var removeResult = await Mediator.Send(command);

        return NoContent();
    }
}

public record ProductRemoveCommand(Guid ProductId) : IRequest;

public class ProductRemoveCommandHandler : IRequestHandler<ProductRemoveCommand>
{
    private readonly InvSysDbContext _dbContext;

    public ProductRemoveCommandHandler(InvSysDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(ProductRemoveCommand request, CancellationToken cancellationToken)
    {
        var productToRemove = await _dbContext.Products
            .FirstOrDefaultAsync(p => p.Id.Value == request.ProductId, cancellationToken);

        _dbContext.Products.Remove(productToRemove!);

        await _dbContext.SaveChangesAsync();

        return Unit.Value;
    }
}

public class ProductRemovedEvent : DomainEvent
{
    public ProductRemovedEvent(Product product)
    {
        Product = product;

    }

    public Product Product { get; }
}