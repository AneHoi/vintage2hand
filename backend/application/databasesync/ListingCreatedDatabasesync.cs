using domain;
using MediatR;

namespace application.databasesync;

public class ListingCreatedDatabasesync : INotificationHandler<ListingCreatedEvent>
{
    private readonly ReadRepository readRepository;
    
    //This will sync the data into MongoDB
    public Task Handle(ListingCreatedEvent notification, CancellationToken cancellationToken)
    
        // write to the read repo
        throw new NotImplementedException();
    }
}