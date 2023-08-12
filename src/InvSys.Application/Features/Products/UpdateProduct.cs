using FluentResults;

using InvSys.Application.Common;
using InvSys.Application.Entities;
using InvSys.Application.Infrastructure;
using InvSys.Application.ValueObjects;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace InvSys.Application.Features.Products;

[Route("/api/product")]
public class UpdateProductController : ApiControllerBase
{
    [HttpPut]
    public async Task<ActionResult> Update([FromQuery] Guid productId, ProductUpdateCommand command)
    {
        command.ProductId = productId;
        Result<ProductUpdateResult> updateResult = await Mediator.Send(command);

        return updateResult.IsFailed ? Problem(updateResult.Errors.Select(e => e.Metadata)) : Ok(updateResult.Value);
    }
}

public record ProductUpdateResult(Product Product);

public record ProductUpdateCommand(
        Guid ProductId,
        string Condition,
        string Name,
        int AvailableQuantity,
        int StockQuantity,
        double Price)
    : IRequest<Result<ProductUpdateResult>>
{
    public Guid ProductId { get; set; } = ProductId;
    public string Condition { get; set; } = Condition;
    public string Name { get; set; } = Name;
    public int AvailableQuantity { get; set; } = AvailableQuantity;
    public int StockQuantity { get; set; } = StockQuantity;
    public double Price { get; set; } = Price;
}

public class ProductUpdateCommandHandler : IRequestHandler<ProductUpdateCommand, Result<ProductUpdateResult>>
{
    private readonly InvSysDbContext _dbContext;

    public ProductUpdateCommandHandler(InvSysDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<ProductUpdateResult>> Handle(ProductUpdateCommand request,
        CancellationToken cancellationToken)
    {
        Product? product = await _dbContext.Products
            .FindAsync(new object?[] { ProductId.Create(request.ProductId), cancellationToken }
                , cancellationToken);

        // how can i improve this?
        UpdateProduct(request, product);

        product!.DomainEvents.Add(new ProductUpdatedEvent(product));

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ProductUpdateResult(product);
    }

    private void UpdateProduct(ProductUpdateCommand request, Product? product)
    {
        product!.Condition = request.Condition;
        product.Name = request.Name;
        product.AvailableQuantity = request.AvailableQuantity == default
            ? product.AvailableQuantity
            : ProductQuantity.Create(request.AvailableQuantity);
        product.StockQuantity = request.StockQuantity == default
            ? product.StockQuantity
            : ProductQuantity.Create(request.StockQuantity);
        product.Price = request.Price == default ? product.Price : ProductPrice.Create(request.Price);
    }
}

public class ProductUpdatedEvent : DomainEvent
{
    public ProductUpdatedEvent(Product product)
    {
        Product = product;
    }

    public Product Product { get; }
}