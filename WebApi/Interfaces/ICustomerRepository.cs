using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface ICustomerRepository
    { 
        public Task<Customer?> GetCustomer(long id); 

        public Task<long> CreateCustomer(Customer customer);
    }
}
