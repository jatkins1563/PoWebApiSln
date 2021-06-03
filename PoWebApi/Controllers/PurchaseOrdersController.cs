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
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly PoContext _context;

        public PurchaseOrdersController(PoContext context)
        {
            _context = context;
        }

        // PUT: api/PurchaseOrders/5/edit
        [HttpPut("{id}/edit")]
        public async Task<IActionResult> PutPurchaseOrderToEdit(int id)
        {
            var po = await _context.PurchaseOrders.FindAsync(id);
            if(po == null)
            {
                return NotFound();
            }
            po.Status = "EDIT";
            
            return await PutPurchaseOrder(id, po);
        }

        // PUT: api/PurchaseOrders/5/review
        [HttpPut("{id}/review")]
        public async Task<IActionResult> PutPurchaseOrderToReviewOrApproved(int id)
        {
            var po = await _context.PurchaseOrders.FindAsync(id);
            if (po == null)
            {
                return NotFound();
            }
            po.Status = (po.Total > 0 && po.Total <= 100) ? "APPROVED" : "REVIEW";  //ternary operator! good to use here

            return await PutPurchaseOrder(id, po);
        }

        // GET: api/PurchaseOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseOrder>>> GetPurchaseOrders()
        {
            return await _context.PurchaseOrders
                .Include(x => x.Employee)   //include this line to pull Employee class for foreign key attachment
                .ToListAsync();
        }

        // GET: api/PurchaseOrders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseOrder>> GetPurchaseOrder(int id)
        {
            var purchaseOrder = await _context.PurchaseOrders.FindAsync(id); //GetByPk without employee class info pulled

            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return purchaseOrder;
        }

        // GET: api/PurchaseOrders/5/empl
        [HttpGet("{id}/empl")]
        public async Task<ActionResult<PurchaseOrder>> GetPurchaseOrderWithEmpl(int id)
        {
            var purchaseOrder = await _context.PurchaseOrders
                .Include(x => x.Employee)   //include this line to pull Employee class for foreign key attachment
                .SingleOrDefaultAsync(x => x.Id == id); //changed to SODAsync from FindAsync

            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return purchaseOrder;
        }

        // GET: api/PurchaseOrders/review
        [HttpGet("review")]
        public async Task<ActionResult<IEnumerable<PurchaseOrder>>> GetPurchaseOrdersForReview()
        {
            return await _context.PurchaseOrders
                .Where(x => x.Status == "REVIEW")
                .Include(x => x.Employee)   //include this line to pull Employee class for foreign key attachment
                .ToListAsync();
        }

        // PUT: api/PurchaseOrders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchaseOrder(int id, PurchaseOrder purchaseOrder)
        {
            if (id != purchaseOrder.Id)
            {
                return BadRequest();
            }

            _context.Entry(purchaseOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseOrderExists(id))
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

        // POST: api/PurchaseOrders
        [HttpPost]
        public async Task<ActionResult<PurchaseOrder>> PostPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            _context.PurchaseOrders.Add(purchaseOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPurchaseOrder", new { id = purchaseOrder.Id }, purchaseOrder);
        }

        // DELETE: api/PurchaseOrders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PurchaseOrder>> DeletePurchaseOrder(int id)
        {
            var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            _context.PurchaseOrders.Remove(purchaseOrder);
            await _context.SaveChangesAsync();

            return purchaseOrder;
        }

        private bool PurchaseOrderExists(int id)
        {
            return _context.PurchaseOrders.Any(e => e.Id == id);
        }
    }
}
