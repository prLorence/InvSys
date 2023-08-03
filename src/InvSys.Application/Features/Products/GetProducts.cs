using FluentResults;

using InvSys.Application.Common;
using InvSys.Application.Entities;
using InvSys.Application.Infrastructure;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvSys.Application.Features.Products;

[Route("/api/products")]
public class GetProductsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetAll([FromQuery] GetProductsQuery query)
    {
        var queryResult = await Mediator.Send(query);

        return Ok(queryResult.Value);

        // return Problem(queryResult.Errors.Select(e => e.Metadata));
    }
}

public record GetProductsQuery(int PageNumber = 1, int PageSize = 10) : IRequest<Result<List<Product>>>;
// TODO: implement paginated list

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<List<Product>>>
{
    private readonly InvSysDbContext _dbContext;

    public GetProductsQueryHandler(InvSysDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<Product>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _dbContext.Products
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        if (products is null)
        {
            return Result.Fail("No products.");
        }

        return products;
    }
}