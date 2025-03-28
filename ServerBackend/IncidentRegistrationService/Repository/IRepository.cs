namespace IncidentRegistrationService.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();

        Task<T?> GetByIdAsync(string id);

        Task<List<T>> GetFilteredAsync();

        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}
