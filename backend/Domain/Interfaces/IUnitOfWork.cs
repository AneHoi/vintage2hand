namespace domain.Interfaces;

public interface IUnitOfWork: IDisposable
{
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    // SaveChangesAsync implicitly covers BeginTransaction, CommitTransaction, RollbackTransaction
    // Separate methods could be added if needed
}