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
    public class DalOpinion : IDalOpinion
    {
        private readonly ShelteredPlacesDb _db;
        public DalOpinion(ShelteredPlacesDb db)
        {
            _db = db;
        }

        public async Task AddAsync(Opinion entity)
        {
            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var opinion = await GetByIdAsync(id);
            if (opinion != null)
            {
                _db.Opinsiones.Remove(opinion);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<List<Opinion>> GetAllAsync()
        {
            return await _db.Opinsiones.Include(o=>o.Address).Include(o=>o.Address.Shelter).ToListAsync();
        }

        public async Task<Opinion> GetByIdAsync(int id)
        {
            var all = await GetAllAsync();
            return all.FirstOrDefault(o=>o.Code == id);
        }

        public async Task UpdateAsync(Opinion entity)
        {
            _db.Opinsiones.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
