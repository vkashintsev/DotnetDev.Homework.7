using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("customers")] 
    public class CustomerController : Controller
    {
        private ICustomerRepository _customerRepository;
        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<Customer>> GetCustomer([FromRoute] long id)
        {
            var customer = await _customerRepository.GetCustomer(id);
            if (customer == null)
                return NotFound();
            return Ok(customer);
        }

        [HttpPost("create")]
        public async Task<ActionResult<long>> CreateCustomer([FromBody] Customer customer)
        {
            var id = await _customerRepository.CreateCustomer(customer);
            return Ok(id);
        }
    }
}