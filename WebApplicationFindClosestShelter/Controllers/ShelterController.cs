using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationFindClosestShelter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShelterController : ControllerBase
    {
        private readonly IBllShelter bllShelter;
        public ShelterController(IBllShelter bllShelter) {
            this.bllShelter = bllShelter;
        }

        [HttpGet]
        public async Task<List<ShelterDTO>> GetAllSheltersAsync()
        {
            return await bllShelter.GetAllSheltersAsync();
        }

        [HttpGet("byId/{id}")]
        public async Task<ShelterDTO> GetByIdAsync(int id)
        {
            return await bllShelter.GetByIdAsync(id);
        }

        [HttpGet("byName/{name}")]
        public async Task<List<ShelterDTO>> GetByNameAsync(string name)
        {
            return await bllShelter.GetByNameAsync(name);
        }


    }
}
