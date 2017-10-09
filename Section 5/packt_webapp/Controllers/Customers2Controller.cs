using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PacktWebapp.Dtos;
using PacktWebapp.Entities;
using PacktWebapp.QueryParameters;
using PacktWebapp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PacktWebapp.Controllers
{
   // [ApiVersion("2.0")] 
    [Route("api/[controller]")]
    public class Customers2Controller : Controller
    {
        private readonly ILogger<CustomersController> _logger;
        private ICustomerRepository _customerRepository;

        public Customers2Controller(ICustomerRepository customerRepository, ILogger<CustomersController> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(List<Customer>), 200)]
        public IActionResult GetAllCustomers(CustomerQueryParameters customerQueryParameters)
        {
            var allCustomers = _customerRepository.GetAll(customerQueryParameters).ToList();

            var allCustomersDto = allCustomers.Select(x => Mapper.Map<CustomerDto>(x));

            Response.Headers.Add("X-Pagination",
                JsonConvert.SerializeObject(new { totalCount = _customerRepository.Count() }));

            return Ok(allCustomersDto);
        }
    }
}
