using MinimalApi.CustomerServices.Models.Customers;

namespace MinimalApi.CustomerServices.Services.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly Dictionary<int, Customer> _customers = new();

        public void Add(Customer customer)
        {
            _customers.Add(customer.Id, customer);
        }

        public void Delete(int customerId)
        {
            _customers.Remove(customerId);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _customers.Values;
        }

        public Customer? GetById(int id)
        {
            if(_customers.ContainsKey(id))
            { 
                return _customers[id];
            }
            return null;
        }

        public void Update(Customer customer)
        {
            _customers[customer.Id] = customer;
        }
    }
}
