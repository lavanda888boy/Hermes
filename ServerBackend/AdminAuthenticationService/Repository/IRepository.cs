namespace AdminAuthenticationService.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByUserNameAsync(string userName);

        Task AddAsync(T entity);
    }
}
