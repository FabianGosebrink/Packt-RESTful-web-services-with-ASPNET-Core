using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using packt_webapp.Dtos;
using packt_webapp.Entities;
using packt_webapp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace packt_webapp.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            var allCustomers = _customerRepository.GetAll().ToList();

            var allCustomersDto = allCustomers.Select(x => Mapper.Map<CustomerDto>(x));

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
        public IActionResult AddCustomer([FromBody] CustomerCreateDto customerCreateDto)
        {
            Customer toAdd = Mapper.Map<Customer>(customerCreateDto);

            _customerRepository.Add(toAdd);

            bool result = _customerRepository.Save();

            if (!result)
            {
                return new StatusCodeResult(500);
            }

            //return Ok(Mapper.Map<CustomerDto>(toAdd));
            return CreatedAtRoute("GetSingleCustomer", new { id = toAdd.Id }, Mapper.Map<CustomerDto>(toAdd));
        }

        // PUT api/customers/{id}

        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateCustomer(Guid id, [FromBody] CustomerUpdateDto updateDto)
        {
            var existingCustomer = _customerRepository.GetSingle(id);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            Mapper.Map(updateDto, existingCustomer);

            _customerRepository.Update(existingCustomer);

            bool result = _customerRepository.Save();

            if (!result)
            {
                return new StatusCodeResult(500);
            }

            return Ok(Mapper.Map<CustomerDto>(existingCustomer));
        }

        [HttpPatch]
        [Route("{id}")]
        public IActionResult PartiallyUpdate(Guid id, [FromBody] JsonPatchDocument<CustomerUpdateDto> customerPatchDoc)
        {
            if(customerPatchDoc == null)
            {
                return BadRequest();
            }

            var existingCustomer = _customerRepository.GetSingle(id);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            var customerToPatch = Mapper.Map<CustomerUpdateDto>(existingCustomer);
            customerPatchDoc.ApplyTo(customerToPatch);

            Mapper.Map(customerToPatch, existingCustomer);

            _customerRepository.Update(existingCustomer);

            bool result = _customerRepository.Save();

            if (!result)
            {
                return new StatusCodeResult(500);
            }

            return Ok(Mapper.Map<CustomerDto>(existingCustomer));
        }


        [HttpDelete]
        [Route("{id}")]
        public IActionResult Remove(Guid id)
        {
            var existingCustomer = _customerRepository.GetSingle(id);

            if(existingCustomer == null)
            {
                return NotFound();
            }

            _customerRepository.Delete(id);

            bool result = _customerRepository.Save();

            if (!result)
            {
                return new StatusCodeResult(500);
            }

            return NoContent();
        }

    }
}
