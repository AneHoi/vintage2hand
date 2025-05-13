using domain.Interfaces;

namespace a;

// Unit of Work would utilize EF core's tracking capabilities
public class UnitOfWork: IUnitOfWork
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}