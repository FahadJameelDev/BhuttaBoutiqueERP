using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.Inventory.Entities;
using BhuttaBoutiqueERP.Models.Inventory.ViewModels;

namespace BhuttaBoutiqueERP.Services.Inventory
{
    public class SupplierService : ISupplierService
    {
        private readonly AppDbContext _context;

        public SupplierService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            return await _context.Suppliers.ToListAsync();
        }

        public async Task<Supplier?> GetByIdAsync(int id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public async Task<Supplier> CreateAsync(SupplierViewModel model)
        {
            var supplier = new Supplier
            {
                Name = model.Name,
                Contact = model.Contact,
                Email = model.Email,
                Address = model.Address,
                Status = model.Status
            };

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
            return supplier;
        }

        public async Task<Supplier> UpdateAsync(SupplierViewModel model)
        {
            var supplier = await _context.Suppliers.FindAsync(model.Id);
            if (supplier == null)
                throw new KeyNotFoundException($"Supplier with ID {model.Id} not found.");

            supplier.Name = model.Name;
            supplier.Contact = model.Contact;
            supplier.Email = model.Email;
            supplier.Address = model.Address;
            supplier.Status = model.Status;

            await _context.SaveChangesAsync();
            return supplier;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
                return false;

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Suppliers.AnyAsync(s => s.Id == id);
        }
    }
}