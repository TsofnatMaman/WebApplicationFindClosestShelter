using System.ComponentModel;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationFindClosestShelter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpinionController : ControllerBase
    {
        private readonly IBllOpinion bllOpinion;
        public OpinionController(IBllOpinion bllOpinion) {
            this.bllOpinion = bllOpinion;
        }

        [HttpGet]
        public async Task<List<OpinionDTO>> GetAll()
        {
            return await bllOpinion.GetAllAsync();
        }

        [HttpGet("byAddress")]
        public async Task<List<OpinionDTO>> GetByAddressAsync(AddressDTO address)
        {
            return await bllOpinion.GetByAddressAsync(address);
        }

        [HttpGet("{id}")]
        public async Task<OpinionDTO> GetByIdAsync(int id)
        {
            return await bllOpinion.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task AddAsync(OpinionDTO opinion)
        {
            await bllOpinion.AddAsync(opinion);
        }

        [HttpPut]
        public async Task UpdateAsync(OpinionDTO opinion)
        {
            await bllOpinion.UpdateAsync(opinion);
        }

        [HttpDelete]
        public async Task DeleteAsync(int id)
        {
            await bllOpinion.DeleteAsync(id);
        }

    }
}
