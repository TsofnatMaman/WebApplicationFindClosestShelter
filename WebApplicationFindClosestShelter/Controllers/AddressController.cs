using System.ComponentModel;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Models;
using Core.Resources_DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationFindClosestShelter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IBllAddress bllAddress;
        public AddressController(IBllAddress bllAddress) {
            this.bllAddress = bllAddress;
        }

        [HttpGet]
        public async Task<List<AddressDTO>> GetAllAsync()
        {
            return await bllAddress.GetAllAsync();
        }

        [HttpGet("Closest/{location}")]
        public async Task<List<AddressDTO>> GetClosestAsync(string location)
        {
            return await bllAddress.GetClosestAsync(location);
        }

        [HttpGet("lastMonth")]
        public async Task<List<Address>> GetLastMonthAddedAddressAsync()
        {
            return await bllAddress.GetLastMonthAddedAddressAsync();
        }

        [HttpPost]
        public async Task AddAsync(AddressDTO address)
        {
            await bllAddress.AddAsync(address);
        }

        [HttpPut]
        public async Task UpdateAsync(AddressDTO address)
        {
            await bllAddress.UpdateAsync(address);
        }

        [HttpDelete]
        public async Task RemoveAsync(int idAddress)
        {
            await bllAddress.RemoveAsync(idAddress);
        }
    }
}
