using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dal.data
{
    public class DalAddress : IDalAddress
    {
        private readonly ShelteredPlacesDb _context;
        public DalAddress(ShelteredPlacesDb context)
        {
            _context = context;
        }

        public async Task AddAsync(Address entity)
        {
            await _context.Addresses.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            Address address = await GetByIdAsync(id);

            if (address != null)
            {
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Address>> GetAllAsync()
        {
            return await _context.Addresses.Include(a=>a.Shelter).ToListAsync();
        }

        public async Task<Address> GetByIdAsync(int id)
        {
            return await _context.Addresses
                .Include(a => a.Shelter)
                .FirstOrDefaultAsync(a => a.Code == id);
        }

        public async Task UpdateAsync(Address entity)
        {
            _context.Addresses.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
