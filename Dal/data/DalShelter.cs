using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal.data
{
    public class DalShelter : IDalShelter
    {
        private readonly ShelteredPlacesDb _db;
        public DalShelter(ShelteredPlacesDb db) {
            _db = db;
        }
        public async Task AddAsync(Shelter entity)
        {
            await _db.Shelters.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var shelter = await GetByIdAsync(id);
            if (shelter != null)
            {
                _db.Shelters.Remove(shelter);
                 await _db.SaveChangesAsync();
            }
        }

        public async Task<List<Shelter>> GetAllAsync()
        {
            return await _db.Shelters.ToListAsync();
        }

        public async Task<Shelter> GetByIdAsync(int id)
        {
            return await _db.Shelters.FirstOrDefaultAsync(s => s.Code == id);
        }

        public async Task UpdateAsync(Shelter entity)
        {
            _db.Shelters.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
