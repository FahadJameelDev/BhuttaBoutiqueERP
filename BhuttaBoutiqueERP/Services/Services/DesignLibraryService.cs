using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.Services.Entities;
using BhuttaBoutiqueERP.Models.Services.ViewModels;

namespace BhuttaBoutiqueERP.Services.Services
{
    public class DesignLibraryService : IDesignLibraryService
    {
        private readonly AppDbContext _context;

        public DesignLibraryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DesignLibrary>> GetAllAsync()
        {
            return await _context.DesignLibraries.ToListAsync();
        }

        public async Task<DesignLibrary?> GetByIdAsync(int id)
        {
            return await _context.DesignLibraries.FindAsync(id);
        }

        public async Task<DesignLibrary> CreateAsync(DesignLibraryViewModel model)
        {
            var design = new DesignLibrary
            {
                Name = model.Name,
                Category = model.Category,
                Type = model.Type,
                Price = model.Price,
                Complexity = model.Complexity,
                Image = model.Image,
                Status = model.Status
            };

            _context.DesignLibraries.Add(design);
            await _context.SaveChangesAsync();
            return design;
        }

        public async Task<DesignLibrary> UpdateAsync(DesignLibraryViewModel model)
        {
            var design = await _context.DesignLibraries.FindAsync(model.Id);
            if (design == null)
                throw new KeyNotFoundException($"Design with ID {model.Id} not found.");

            design.Name = model.Name;
            design.Category = model.Category;
            design.Type = model.Type;
            design.Price = model.Price;
            design.Complexity = model.Complexity;
            design.Image = model.Image;
            design.Status = model.Status;

            await _context.SaveChangesAsync();
            return design;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var design = await _context.DesignLibraries.FindAsync(id);
            if (design == null)
                return false;

            _context.DesignLibraries.Remove(design);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.DesignLibraries.AnyAsync(d => d.Id == id);
        }
    }
}