namespace Your_Ride.Repository.Generic
{
    public interface IRepository<T> where T : class
    {
        public Task<T> GetByIdAsync(int id);

        public Task<List<T>> GetAllAsync();

        public Task<T> AddAsync(T entity);

        public Task<int> DisposeAsync(int id);

        public Task<T> UpdateAsync(T entity);

        public Task SaveDB();




    }
}