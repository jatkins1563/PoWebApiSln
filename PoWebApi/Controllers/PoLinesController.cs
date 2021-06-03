using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoWebApi.Data;
using PoWebApi.Models;

namespace PoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoLinesController : ControllerBase
    {
        private readonly PoContext _context;

        public PoLinesController(PoContext context)
        {
            _context = context;
        }

        // GET: api/PoLines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PoLine>>> GetPoLine()
        {
            return await _context.PoLine.ToListAsync();
        }

        // GET: api/PoLines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PoLine>> GetPoLine(int id)
        {
            var poLine = await _context.PoLine.FindAsync(id);

            if (poLine == null)
            {
                return NotFound();
            }

            return poLine;
        }

        // PUT: api/PoLines/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPoLine(int id, PoLine poLine)
        {
            if (id != poLine.Id)
            {
                return BadRequest();
            }

            _context.Entry(poLine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PoLineExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PoLines
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PoLine>> PostPoLine(PoLine poLine)
        {
            _context.PoLine.Add(poLine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPoLine", new { id = poLine.Id }, poLine);
        }

        // DELETE: api/PoLines/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PoLine>> DeletePoLine(int id)
        {
            var poLine = await _context.PoLine.FindAsync(id);
            if (poLine == null)
            {
                return NotFound();
            }

            _context.PoLine.Remove(poLine);
            await _context.SaveChangesAsync();

            return poLine;
        }

        private bool PoLineExists(int id)
        {
            return _context.PoLine.Any(e => e.Id == id);
        }
    }
}
