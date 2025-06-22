using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Models;

namespace Core.Interfaces.Services
{
    public interface IBllOpinion
    {
        Task<List<OpinionDTO>> GetAllAsync();
        Task<OpinionDTO> GetByIdAsync(int id);
        Task<List<OpinionDTO>> GetByAddressAsync(AddressDTO address);
        Task AddAsync(OpinionDTO opinion);
        Task UpdateAsync(OpinionDTO opinion);
        Task DeleteAsync(int id);

    }
}
