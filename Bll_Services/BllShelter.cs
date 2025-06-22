using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;

namespace Bll_Services
{
    public class BllShelter : IBllShelter
    {
        private readonly IDalShelter _shelter;
        private readonly IMapper _mapper;

        public BllShelter(IDalShelter shelter, IMapper mapper)
        {
            _shelter = shelter;
            _mapper = mapper;
        }

        public async Task<List<ShelterDTO>> GetAllSheltersAsync()
        {
            return _mapper.Map<List<ShelterDTO>>(await _shelter.GetAllAsync());
        }

        public async Task<ShelterDTO> GetByIdAsync(int id)
        {
            return _mapper.Map<ShelterDTO>(await _shelter.GetByIdAsync(id));
        }

        public async Task<List<ShelterDTO>> GetByNameAsync(string name)
        {
            var all = await _shelter.GetAllAsync();
            return _mapper.Map<List<ShelterDTO>>(all.Where(s => s.Name == name.ToUpper()));
        }

        protected async Task AddAsync(ShelterDTO shelter)
        {
            await _shelter.AddAsync(_mapper.Map<Shelter>(shelter));
        }

        protected async Task DeleteAsync(int id)
        {
            await _shelter.DeleteByIdAsync(id);
        }

    }
}
