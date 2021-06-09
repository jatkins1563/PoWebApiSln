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

        //**************************************************************************
        //homework - post/put/delete calls this private method -
        //pass PO ID - read PO - get all PoLines attached to PO(search by PoId FK) -
        //iterate through each PoLine, multiply poline.quantity * item.price
        //(find item.price through itemid FK) for linetotal,
        //sum up all line-totals for all PoLines
        //TIP: you will have to join polines to item to combine price and quantity
        //sum method in EF - point to data, sum up line-totals
        //**************************************************************************

        private async Task SumLineTotals(int PoId)
        {
            var po = await _context.PurchaseOrders.FindAsync(PoId);
            if(po == null)
            {
                throw new Exception("FATAL: PO not found. Unable to recalculate total.");
            }
            var lines = await _context.PoLines
                                .Join(
                                _context.Items,
                                PoLine => PoLine.ItemId,
                                Item => Item.Id,
                                (PoLine, Item) => new
                                {
                                    PoId = PoLine.PurchaseOrderId,
                                    Quantity = PoLine.Quantity,
                                    Price = Item.Price
                                })
                                .Where(i => i.PoId == PoId)
                                .ToListAsync();
            var sum = 0.00m;
            foreach(var line in lines)
            {
                sum += line.Quantity * line.Price;
            }
            po.Total = sum;
            await _context.SaveChangesAsync();
            return;
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
            await SumLineTotals(poLine.PurchaseOrderId);
            return NoContent();
        }

        // POST: api/PoLines
        [HttpPost]
        public async Task<ActionResult<PoLine>> PostPoLine(PoLine poLine)
        {
            _context.PoLine.Add(poLine);
            await _context.SaveChangesAsync();

            await SumLineTotals(poLine.PurchaseOrderId);
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

            await SumLineTotals(poLine.PurchaseOrderId);
            return poLine;
        }

        private bool PoLineExists(int id)
        {
            return _context.PoLine.Any(e => e.Id == id);
        }
    }
}
