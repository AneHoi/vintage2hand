namespace domain.Interfaces;

public interface IListingWriteRepository
{
    /*  This would be implemented in infrastructure layer.
     * It Writes to the NO-SQL database
     */
    Listing SaveListing(Listing listing);
    
}