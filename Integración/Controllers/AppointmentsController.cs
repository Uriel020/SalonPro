using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Data.Context;

namespace Integracion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly SalonDbContext _context;

        public AppointmentsController(SalonDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var appointments = await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Service)
                .ToListAsync();
            return Ok(appointments);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Appointment appointment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = appointment.Id }, appointment);
        }

    }
}
