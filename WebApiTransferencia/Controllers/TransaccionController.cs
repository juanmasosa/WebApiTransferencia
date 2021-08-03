using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiTransferencia.DAL;
using WebApiTransferencia.Model;

namespace WebApiTransferencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransaccionController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public TransaccionController(ApplicationDBContext context)
        {
            _context = context;
        }
                
        // GET: api/Transaccion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaccion>>> GetTransacciones()
        {
            return await _context.Transacciones.ToListAsync();
        }

        // GET: api/Transaccion/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaccion>> GetTransaccion(int id)
        {
            var transaccion = await _context.Transacciones.FindAsync(id);

            if (transaccion == null)
            {
                return NotFound();
            }

            return transaccion;
        }

        // PUT: api/Transaccion/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaccion(int id, Transaccion transaccion)
        {
            if (id != transaccion.Id)
            {
                return BadRequest();
            }

            _context.Entry(transaccion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransaccionExists(id))
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

        // POST: api/Transaccion
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Transaccion>> PostTransaccion(Transaccion transaccion)
        {
            var existeOrigen = await _context.Cuentas.FindAsync(transaccion.cuentaOrigen);
            var existeDestino = await _context.Cuentas.FindAsync(transaccion.cuentaDestino);
            
            if ((existeOrigen == null) || ( existeDestino == null))
            {
                return BadRequest("La cuenta no existe");
            }


            var saldo = (from a in _context.Cuentas
                        where a.Id == transaccion.cuentaOrigen
                        select a.Saldo).FirstOrDefault();

            if (saldo < transaccion.montoTransferencia)
            {
                return BadRequest("La cuenta no posee saldo suficiente");
            }

            Cuenta cuentaDestino = _context.Cuentas.Find(transaccion.cuentaDestino);
            cuentaDestino.Saldo = cuentaDestino.Saldo + transaccion.montoTransferencia;

            Cuenta cuentaOrigen = _context.Cuentas.Find(transaccion.cuentaOrigen);
            cuentaOrigen.Saldo = cuentaOrigen.Saldo - transaccion.montoTransferencia;

            _context.SaveChanges();
            _context.Transacciones.Add(transaccion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransaccion", new { id = transaccion.Id }, transaccion);
        }

        // DELETE: api/Transaccion/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Transaccion>> DeleteTransaccion(int id)
        {
            var transaccion = await _context.Transacciones.FindAsync(id);
            if (transaccion == null)
            {
                return NotFound();
            }

            _context.Transacciones.Remove(transaccion);
            await _context.SaveChangesAsync();

            return transaccion;
        }

        private bool TransaccionExists(int id)
        {
            return _context.Transacciones.Any(e => e.Id == id);
        }
    }
}
