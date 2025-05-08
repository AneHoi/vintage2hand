using domain.Interfaces;
using MediatR;

namespace application.Commands;

public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IListingWriteRepository _listingWriteRepository; 
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _mediator;
    
    public PlaceOrderCommandHandler(IOrderRepository orderRepository, IListingWriteRepository listingWriteRepository, IUnitOfWork unitOfWork, IPublisher mediator)
    {
        _orderRepository = orderRepository;
        _listingWriteRepository = listingWriteRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }
    
    public async Task<Guid> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        // Fetch listings, check availability, and create order & update listing status
        // Persist to db.
        // Commit transaction using unit of work (critical for order and listing updates)
        // Publish event: OrderPlacedEvent
        
        // return order ID
        throw new NotImplementedException();
    }
}