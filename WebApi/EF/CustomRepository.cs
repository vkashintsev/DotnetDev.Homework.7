using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.EF
{
    public class CustomRepository : ICustomerRepository
    {
        private readonly DataContext _dataContext;

        public CustomRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
         
        public async Task<Customer?> GetCustomer(long id)
        {
            return await _dataContext.Customers.SingleOrDefaultAsync(x => x.Id == id);
        } 

        public async Task<long> CreateCustomer(Customer customer)
        {
            var entity = _dataContext.Customers.Add(customer);
            await _dataContext.SaveChangesAsync();
            return (long)entity.Entity.Id;
        }
    }
}
