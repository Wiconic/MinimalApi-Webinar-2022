using MinimalApi.CustomerServices.Models.Customers;

namespace MinimalApi.CustomerServices.Services.Customers
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetAll();
        Customer GetById(int id);

        void Add(Customer customer);
        void Update(Customer customer);
        void Delete(int customerId);
    }
}
