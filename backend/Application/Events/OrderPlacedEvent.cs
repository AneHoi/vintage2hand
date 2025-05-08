using MediatR;

namespace domain;

public record OrderPlacedEvent(Guid OrderId, string BuyerId): INotification;