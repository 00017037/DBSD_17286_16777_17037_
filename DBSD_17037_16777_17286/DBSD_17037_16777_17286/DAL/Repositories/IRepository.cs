namespace DBSD_17037_16777_17286.DAL.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        int Insert(T entity);
        void Update(T entity);
        Task<IEnumerable<T>> ? Filter(string firstName, string lastName, string departmentName, DateTime? hireDate, string sortField, bool sortDesc, int page, int pageSize);

        void Delete(int id);
    }
}
