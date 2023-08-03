using FluentResults;

using InvSys.Application.Common;
using InvSys.Application.Entities;
using InvSys.Application.Infrastructure;
using InvSys.Application.ValueObjects;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace InvSys.Application.Features.Products;

[Route("/api/product")]
public class CreateProductController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create(CreateProductCommand command)
    {
        var commandResult = await Mediator.Send(command);

        if (commandResult.IsFailed)
        {
            return Problem();
        }

        return Ok(commandResult.Value);
    }
}

public record CreateProductCommand(
        string Name,
        string SKU,
        string Condition,
        string Location,
        int AvailableQuantity,
        int StockQuantity,
        double Price) : IRequest<Result<ProductResult>>;

public record ProductResult(Product Product);

internal sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductResult>>
{
    private readonly InvSysDbContext _dbContext;

    public CreateProductCommandHandler(InvSysDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<ProductResult>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        var product = Product.Create(
                request.Name,
                SKU.Create(request.SKU),
                request.Condition,
                ProductLocation.WAREHOUSE1,
                ProductQuantity.Create(request.AvailableQuantity),
                ProductQuantity.Create(request.StockQuantity),
                ProductPrice.Create(request.Price));

        product.DomainEvents.Add(new ProductCreatedEvent(product));

        await _dbContext.AddAsync(product);

        await _dbContext.SaveChangesAsync();

        return new ProductResult(product);
    }
}

public class ProductCreatedEvent : DomainEvent
{
    public ProductCreatedEvent(Product product)
    {
        Product = product;
    }

    public Product Product { get; }
}