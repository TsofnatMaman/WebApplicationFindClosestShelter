using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Models;

namespace Core.Interfaces.Services
{
    public interface IBllShelter
    {
        Task<List<ShelterDTO>> GetAllSheltersAsync();
        //Task AddAsync(ShelterDTO shelter);
        //Task DeleteAsync(int id);
        Task<ShelterDTO> GetByIdAsync(int id);
        Task<List<ShelterDTO>> GetByNameAsync(string name);
    }
}
