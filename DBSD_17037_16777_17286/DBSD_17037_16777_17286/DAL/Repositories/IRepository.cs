namespace DBSD_17037_16777_17286.DAL.Repositories
{
    public interface IRepository<T> where T : class
    {
        string ExportToXml(
            int Id, 
            string? FirstName, 
            string? LastName, 
            DateTime? HireDate, 
            Boolean IsMarried, 
            string? ManagerFirstName, string? ManagerLastName
            );
        string ExportToJson(
            int Id,
            string? FirstName,
            string? LastName,
            DateTime? HireDate,
            Boolean IsMarried,
            string? ManagerFirstName, string? ManagerLastName
            );
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        int Insert(T entity);
        void Update(T entity);
        Task<IEnumerable<T>> ? Filter(string firstName, string lastName, string departmentName, DateTime? hireDate, string sortField, bool sortDesc, int page, int pageSize);

        void Delete(int id);
    }
}
