using MinimalApi.CustomerServices.Models.Customers;
using MinimalApi.CustomerServices.Services.Customers;

namespace MinimalApi.Structured.Extensions.Customers
{
    public static class CustomerEndpoints
    {
        public static void RegisterCustomerEndpoints(this WebApplication app)
        {
            app.MapGet("/customers", GetAllCustomers);
            app.MapGet("/customers/{id}", GetCustomerById);
            app.MapPost("/customers", AddCustomer);
            app.MapPut("/Customers", UpdateCustomer);
            app.MapDelete("/Customers/{id}", DeleteCustomer);
        }

        public static void ConfigureCustomerServices(this IServiceCollection services)
        {
            services.AddSingleton<ICustomerService, CustomerService>();
        }

        private static List<Customer> GetAllCustomers(ICustomerService service)
        {
            return service.GetAll().ToList();
        }

        private static IResult GetCustomerById(ICustomerService service, int id)
        {
            var customer = service.GetById(id);
            return customer is not null ? Results.Ok(customer) : Results.NotFound();
        }

        private static IResult AddCustomer(ICustomerService service, Customer customer)
        {
            service.Add(customer);
            return Results.Created($"/Customer/{customer.Id}", customer);
        }

        private static IResult UpdateCustomer(ICustomerService service, Customer customer)
        {
            service.Update(customer);
            return Results.Ok(customer);
        }

        private static IResult DeleteCustomer(ICustomerService service, int id)
        {
            service.Delete(id);
            return Results.Ok();
        }
    }
}
