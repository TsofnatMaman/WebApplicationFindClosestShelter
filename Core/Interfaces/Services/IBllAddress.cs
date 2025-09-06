using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Models;
using Core.Resources_DTO;

namespace Core.Interfaces.Services
{
    public interface IBllAddress
    {
        Task AddAsync(AddressDTO address);
        Task RemoveAsync(int idAddress);
        Task UpdateAsync(AddressDTO address);
        Task<List<AddressDTO>> GetClosestAsync(string location);
        Task<List<Address>> GetLastMonthAddedAddressAsync();
        Task<List<AddressDTO>> GetAllAsync();
    }
}
