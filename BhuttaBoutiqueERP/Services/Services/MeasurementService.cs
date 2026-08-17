using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.Services.Entities;
using BhuttaBoutiqueERP.Models.Services.ViewModels;

namespace BhuttaBoutiqueERP.Services.Services
{
    public class MeasurementService : IMeasurementService
    {
        private readonly AppDbContext _context;

        public MeasurementService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Measurement>> GetAllAsync()
        {
            return await _context.Measurements
                .Include(m => m.Customer)
                .Include(m => m.Order)
                .ToListAsync();
        }

        public async Task<Measurement?> GetByIdAsync(int id)
        {
            return await _context.Measurements
                .Include(m => m.Customer)
                .Include(m => m.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Measurement> CreateAsync(MeasurementViewModel model)
        {
            var measurement = new Measurement
            {
                CustomerId = model.CustomerId,
                OrderId = model.OrderId,
                Chest = model.Chest,
                Waist = model.Waist,
                Hips = model.Hips,
                Shoulder = model.Shoulder,
                Armhole = model.Armhole,
                Neck = model.Neck,
                KurtaLength = model.KurtaLength,
                ShalwarLength = model.ShalwarLength,
                Paicha = model.Paicha,
                SleeveLength = model.SleeveLength,
                Bust = model.Bust,
                KameezLength = model.KameezLength,
                DupattaLength = model.DupattaLength,
                Damman = model.Damman,
                Date = model.Date,
                Notes = model.Notes
            };

            _context.Measurements.Add(measurement);
            await _context.SaveChangesAsync();
            return measurement;
        }

        public async Task<Measurement> UpdateAsync(MeasurementViewModel model)
        {
            var measurement = await _context.Measurements.FindAsync(model.Id);
            if (measurement == null)
                throw new KeyNotFoundException($"Measurement with ID {model.Id} not found.");

            measurement.Chest = model.Chest;
            measurement.Waist = model.Waist;
            measurement.Hips = model.Hips;
            measurement.Shoulder = model.Shoulder;
            measurement.Armhole = model.Armhole;
            measurement.Neck = model.Neck;
            measurement.KurtaLength = model.KurtaLength;
            measurement.ShalwarLength = model.ShalwarLength;
            measurement.Paicha = model.Paicha;
            measurement.SleeveLength = model.SleeveLength;
            measurement.Bust = model.Bust;
            measurement.KameezLength = model.KameezLength;
            measurement.DupattaLength = model.DupattaLength;
            measurement.Damman = model.Damman;
            measurement.Notes = model.Notes;

            await _context.SaveChangesAsync();
            return measurement;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var measurement = await _context.Measurements.FindAsync(id);
            if (measurement == null)
                return false;

            _context.Measurements.Remove(measurement);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Measurement>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Measurements
                .Where(m => m.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Measurement>> GetByOrderIdAsync(int orderId)
        {
            return await _context.Measurements
                .Where(m => m.OrderId == orderId)
                .ToListAsync();
        }
    }
}