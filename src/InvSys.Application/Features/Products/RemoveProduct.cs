using FluentResults;

using InvSys.Application.Common;
using InvSys.Application.Entities;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace InvSys.Application.Features.Products;

[Route("/api/product")]
public class RemoveProductController : ApiControllerBase
{
    [HttpDelete]
    public async Task<ActionResult> Remove([FromQuery] ProductRemoveCommand command)
    {
        // Mediator.Send()
        await Task.CompletedTask;
        return Ok();
    }
}

public record ProductRemoveCommand(
            string SKU) : IRequest;

public class ProductRemoveCommandHandler : IRequestHandler<ProductRemoveCommand>
{
    public Task<Unit> Handle(ProductRemoveCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

public class ProductRemovedEvent : DomainEvent
{
    public Product product { get; }

    protected ProductRemovedEvent(Product product)
    {
        this.product = product;
    }
}