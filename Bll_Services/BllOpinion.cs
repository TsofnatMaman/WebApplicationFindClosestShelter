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
    public class BllOpinion : IBllOpinion
    {
        private readonly IDalOpinion _dalOpinion;
        private IMapper _mapper;

        public BllOpinion(IDalOpinion dalOpinion, IMapper mapper)
        {
            _dalOpinion = dalOpinion;
            _mapper = mapper;
        }

        public async Task<List<OpinionDTO>> GetAllAsync()
        {
            return _mapper.Map<List<OpinionDTO>>(await _dalOpinion.GetAllAsync());
        }

        public async Task<List<OpinionDTO>> GetByAddressAsync(int addressCode)
        {
            var all = await _dalOpinion.GetAllAsync();
            return _mapper.Map<List<OpinionDTO>>(all.Where(x => x.Address.Code == addressCode));
        }

        public async Task<OpinionDTO> GetByIdAsync(int id)
        {
            return _mapper.Map<OpinionDTO>(await _dalOpinion.GetByIdAsync(id));
        }

        public async Task AddAsync(OpinionDTO opinion)
        {
            await _dalOpinion.AddAsync(_mapper.Map<Opinion>(opinion));
        }

        public async Task UpdateAsync(OpinionDTO opinion)
        {
            await _dalOpinion.UpdateAsync(_mapper.Map<Opinion>(opinion));
        }

        public async Task DeleteAsync(int id)
        {
            await _dalOpinion.DeleteByIdAsync(id);
        }

    }
}
