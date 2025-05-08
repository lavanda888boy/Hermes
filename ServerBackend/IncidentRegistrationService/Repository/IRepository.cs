namespace IncidentRegistrationService.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync()
        {
            throw new NotImplementedException("GetAllAsync is not implemented.");
        }

        Task<T?> GetByIdAsync(string id)
        {
            throw new NotImplementedException("GetByIdAsync is not implemented.");
        }

        Task<List<T>> GetFilteredAsync(string filter)
        {
            throw new NotImplementedException("GetFilteredAsync is not implemented.");
        }

        Task<T> AddAsync(T entity)
        {
            throw new NotImplementedException("AddAsync is not implemented.");
        }

        Task UpdateAsync(T entity)
        {
            throw new NotImplementedException("UpdateAsync is not implemented.");
        }

        Task DeleteAsync(T entity)
        {
            throw new NotImplementedException("DeleteAsync is not implemented.");
        }
    }
}
