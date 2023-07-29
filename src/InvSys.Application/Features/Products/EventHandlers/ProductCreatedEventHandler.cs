using InvSys.Application.Common.Models;

using MediatR;

using Microsoft.Extensions.Logging;

namespace InvSys.Application.Features.Products.EventHandlers;

public class ProductCreatedEventHandler : INotificationHandler<DomainEventNotification<ProductCreatedEvent>>
{
    private readonly ILogger<ProductCreatedEventHandler> _logger;

    public ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<ProductCreatedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogInformation($"[Product Created][{domainEvent.DateOccurred}] {domainEvent.GetType().Name}");

        return Task.CompletedTask;
    }
}