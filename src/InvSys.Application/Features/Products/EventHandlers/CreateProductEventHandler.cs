using InvSys.Application.Common.Models;

using MediatR;

using Microsoft.Extensions.Logging;

namespace InvSys.Application.Features.Products.EventHandlers;

public class CreateProductEventHandler : INotificationHandler<DomainEventNotification<ProductCreatedEvent>>
{
    private readonly ILogger<CreateProductEventHandler> _logger;

    public CreateProductEventHandler(ILogger<CreateProductEventHandler> logger)
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