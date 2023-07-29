using InvSys.Application.Common.Models;

using MediatR;

using Microsoft.Extensions.Logging;

namespace InvSys.Application.Features.Products.EventHandlers;

public class ProductRemovedEventHandler : INotificationHandler<DomainEventNotification<ProductRemovedEvent>>
{
    private readonly ILogger<ProductRemovedEventHandler> _logger;

    public ProductRemovedEventHandler(ILogger<ProductRemovedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<ProductRemovedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogInformation($"[Product Removed][{domainEvent.DateOccurred}] {domainEvent.GetType().Name}");

        return Task.CompletedTask;
    }
}