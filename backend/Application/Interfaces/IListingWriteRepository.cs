namespace domain.Interfaces;

public interface IListingWriteRepository
{
    /*  Update data in repository
     *  Unit of work research – in repositories.
     * Transaction starts
     * Write to SQL database
     * Write to NoSQL Repository
     * Transaction finish
     */
    Listing AddItemToStorrage(Listing listing);
    
}