namespace DBSD_17037_16777_17286.DAL.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        int Insert(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
