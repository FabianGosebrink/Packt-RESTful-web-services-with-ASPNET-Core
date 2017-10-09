using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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

namespace PacktWebapp.Controllers
{
    //[ApiVersion("1.0", Deprecated = true)]
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private readonly ILogger<CustomersController> _logger;
        private ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository, ILogger<CustomersController> logger)
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

        [HttpGet]
        [Route("{id}", Name = "GetSingleCustomer")]
        public IActionResult GetSingleCustomer(Guid id)
        {
            Customer customerFromRepo = _customerRepository.GetSingle(id);

            if (customerFromRepo == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<CustomerDto>(customerFromRepo));
        }

        // POST api/customers

        [HttpPost]
        [ProducesResponseType(typeof(CustomerDto), 201)]
        [ProducesResponseType(typeof(CustomerDto), 400)]
        public IActionResult AddCustomer([FromBody] CustomerCreateDto customerCreateDto)
        {
            if (customerCreateDto == null)
            {
                return BadRequest("customercreate object was null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Customer toAdd = Mapper.Map<Customer>(customerCreateDto);

            _customerRepository.Add(toAdd);

            bool result = _customerRepository.Save();

            if (!result)
            {
                throw new Exception("something went wrong when adding a new customer");
            }

            //return Ok(Mapper.Map<CustomerDto>(toAdd));
            return CreatedAtRoute("GetSingleCustomer", new { id = toAdd.Id }, Mapper.Map<CustomerDto>(toAdd));
        }

        // PUT api/customers/{id}

        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateCustomer(Guid id, [FromBody] CustomerUpdateDto updateDto)
        {
            if (updateDto == null)
            {
                return BadRequest();
            }

            var existingCustomer = _customerRepository.GetSingle(id);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(updateDto, existingCustomer);

            _customerRepository.Update(existingCustomer);

            bool result = _customerRepository.Save();

            if (!result)
            {
                throw new Exception($"something went wrong when updating the customer with id: {id}");
            }

            return Ok(Mapper.Map<CustomerDto>(existingCustomer));
        }

        [HttpPatch]
        [Route("{id}")]
        public IActionResult PartiallyUpdate(Guid id, [FromBody] JsonPatchDocument<CustomerUpdateDto> customerPatchDoc)
        {
            if (customerPatchDoc == null)
            {
                return BadRequest();
            }

            var existingCustomer = _customerRepository.GetSingle(id);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            var customerToPatch = Mapper.Map<CustomerUpdateDto>(existingCustomer);
            customerPatchDoc.ApplyTo(customerToPatch, ModelState);

            TryValidateModel(customerToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(customerToPatch, existingCustomer);

            _customerRepository.Update(existingCustomer);

            bool result = _customerRepository.Save();

            if (!result)
            {
                throw new Exception($"something went wrong when updating the customer with id: {id}");
            }

            return Ok(Mapper.Map<CustomerDto>(existingCustomer));
        }


        [HttpDelete]
        [Route("{id}")]
        public IActionResult Remove(Guid id)
        {
            var existingCustomer = _customerRepository.GetSingle(id);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            _customerRepository.Delete(id);

            bool result = _customerRepository.Save();

            if (!result)
            {
                throw new Exception($"something went wrong when deleting the customer with id: {id}");
            }

            return NoContent();
        }

    }
}
