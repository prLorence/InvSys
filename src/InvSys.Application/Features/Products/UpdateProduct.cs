using FluentResults;

using InvSys.Application.Common;
using InvSys.Application.Entities;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace InvSys.Application.Features.Products;

[Route("/api/product")]
public class UpdateProductController : ApiControllerBase
{
    [HttpPut]
    public async Task<ActionResult> Update(string sku, ProductUpdateCommand command)
    {
        // Mediator.Send()
        await Task.CompletedTask;
        return Ok();
    }
}

public record ProductUpdateResult(Product Product);

public record ProductUpdateCommand(
        string? Name = default,
        int? Quantity = default,
        int? StockQuantity = default,
        double? Price = default) : IRequest<Result<ProductUpdateResult>>;

public class ProductUpdateCommandHandler : IRequestHandler<ProductUpdateCommand, Result<ProductUpdateResult>>
{
    public Task<Result<ProductUpdateResult>> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
    {
        // find the entity
        // update the properties
        //      - name
        //      - quantity
        //      - stock quantity
        //      - price
        //      - updated date time
        throw new NotImplementedException();
    }
}