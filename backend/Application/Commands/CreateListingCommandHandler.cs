using System.Reflection.Metadata;
using domain;
using domain.Interfaces;
using MediatR;

namespace application.Commands;

public class CreateListingCommandHandler : IRequestHandler<CreateListingCommand, Guid>
{
    private readonly IListingWriteRepository _listingWriteRepository;
    private readonly ICloudStorageService _cloudStorageService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _mediator;
    
    public CreateListingCommandHandler(
        IListingWriteRepository listingWriteRepository,
        ICloudStorageService cloudStorageService,
        IUnitOfWork unitOfWork,
        IPublisher mediator)
    {
        _listingWriteRepository = listingWriteRepository;
        _cloudStorageService = cloudStorageService;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }
    
    public async Task<Guid> Handle(CreateListingCommand request, CancellationToken cancellationToken)
    {
        var listing = new Listing
        {
            Id = Guid.NewGuid(),
            SellerId = request.SellerId,
            Title = request.Title,
            Description = request.Description,
            Address = request.Address,
            Price = request.Price,
            ListingStatus = ListingStatus.Available
        };
        
        // 2. For each imageL
                // upload to cloud:
        // If one upload fails:
            // Execute compensating action (delete the previously uploaded files)
        // Create a listing in db using repo, associating the image URLs
        // 4. Save changes transactionally (unit of work).
        //      If performing multiple actions and one fails, transaction will be rolled back and compensating action for images will be executed.
        // 5. Publish domain event (for read model sync and other side effects / subscribers) using Mediatr
        throw new NotImplementedException();
    }

}