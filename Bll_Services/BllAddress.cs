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
using Core.Resources_DTO;

namespace Bll_Services
{
    public class BllAddress : IBllAddress
    {
        private readonly IDalAddress _dalAddress;
        private readonly IMapper _mapper;

        public BllAddress(IDalAddress dalAddress, IMapper mapper)
        {
            _dalAddress = dalAddress;
            _mapper = mapper;
        }

        public static int CloseLevel(Location src, Location dest)
        {
            double deltaLat = src.Latitude - dest.Latitude;
            double deltaLng = src.Longitude - dest.Longitude;
            return (int)(Math.Sqrt(deltaLat * deltaLat + deltaLng * deltaLng) * 100000); // יחידה מקרבת
        }

        public async Task<List<AddressDTO>> GetClosestAsync(string location)
        {
            List<AddressDTO> all = _mapper.Map<List<AddressDTO>>(await _dalAddress.GetAllAsync());

            return (all
                .Select(a=> { a.Distance = CloseLevel(_mapper.Map<Location>(location), _mapper.Map<Location>(a.Location)); return a; })
                .OrderBy(a => a.Distance)
                .Take(10)
                .ToList());
        }

        public async Task<List<Address>> GetLastMonthAddedAddressAsync()
        {
            var l = await _dalAddress.GetAllAsync();
            DateTime lastMonth = DateTime.Today.AddMonths(-1);
            return l.Where(a => a.AddedSystem >= lastMonth).ToList();
        }

        public async Task AddAsync(AddressDTO address)
        {
            await _dalAddress.AddAsync(_mapper.Map<Address>(address));
        }

        public async Task RemoveAsync(int idAddress)
        {
            await _dalAddress.DeleteByIdAsync(idAddress);
        }

        public async Task UpdateAsync(AddressDTO address)
        {
            await _dalAddress.UpdateAsync(_mapper.Map<Address>(address));
        }

        public async Task<List<AddressDTO>> GetAllAsync()
        {
            return _mapper.Map<List<AddressDTO>>(await _dalAddress.GetAllAsync());
        }
    }
}
