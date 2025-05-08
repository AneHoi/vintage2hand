using domain;
using domain.Interfaces;
using MediatR;

namespace application.Commands;

public class SubmitSellerReviewCommandHandler : IRequestHandler<SubmitSellerReviewCommand, Guid>
{
    private readonly ISellerReviewRepository _sellerReviewRepository;
    private readonly IOrderRepository _orderRepository; // To validate order
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _mediator;
    
    public SubmitSellerReviewCommandHandler(ISellerReviewRepository sellerReviewRepository, IOrderRepository orderRepository, IUnitOfWork unitOfWork, IPublisher mediator)
    {
        _sellerReviewRepository = sellerReviewRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }
    
    public async Task<Guid> Handle(SubmitSellerReviewCommand request, CancellationToken cancellationToken)
    {
        var reviewId = Guid.NewGuid();
        
        await _sellerReviewRepository.AddAsync(new SellerReview("the data...");
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        await _mediator.Publish(new SellerReviewSubmittedEvent(reviewId, request.SellerUserId, request.Rating), cancellationToken);

        return reviewId;
    }