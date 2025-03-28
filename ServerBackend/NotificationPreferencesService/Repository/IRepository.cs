namespace NotificationPreferencesService.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();

        Task<List<T>> GetFilteredAsync(string filter)
        {
            throw new NotImplementedException("GetFilteredAsync is not implemented.");
        }

        Task<T?> GetByIdAsync(string id)
        {
            throw new NotImplementedException("GetByIdAsync is not implemented.");
        }

        Task AddAsync(T entity)
        {
            throw new NotImplementedException("AddAsync is not implemented.");
        }

        Task UpdateAsync(T entity)
        {
            throw new NotImplementedException("UpdateAsync is not implemented.");
        }
    }
}
