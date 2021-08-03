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
    public class CuentaController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public CuentaController(ApplicationDBContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Lista todas las cuentas
        /// </summary>
        /// <returns></returns>
        /// GET: api/Cuenta/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cuenta>>> GetCuentas()
        {
            return await _context.Cuentas.ToListAsync();
        }

        /// <summary>
        /// Lista datos de la cuenta solicitada
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// GET: api/Cuenta/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Cuenta>> GetCuenta(int id)
        {
            var cuenta = await _context.Cuentas.FindAsync(id);

            if (cuenta == null)
            {
                return NotFound();
            }

            return cuenta;
        }

        /// <summary>
        /// Metodo utilizado para dar de alta cuentas.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cuenta"></param>
        /// <returns></returns>
        /// PUT: api/Cuenta/5
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCuenta(int id, Cuenta cuenta)
        {
            if (id != cuenta.Id)
            {
                return BadRequest();
            }

            _context.Entry(cuenta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CuentaExists(id))
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

        /// <summary>
        /// Metodo utilizado para dar de alta cuentas.
        /// </summary>
        /// <param name="cuenta"></param>
        /// <returns></returns>
        /// POST: api/Cuenta

        [HttpPost]
        public async Task<ActionResult<Cuenta>> PostCuenta(Cuenta cuenta)
        {
            _context.Cuentas.Add(cuenta);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCuenta", new { id = cuenta.Id }, cuenta);
        }
        /// <summary>
        /// Metodo utilizado para dar de baja cuentas.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// DELETE: api/Cuenta/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Cuenta>> DeleteCuenta(int id)
        {
            var cuenta = await _context.Cuentas.FindAsync(id);
            if (cuenta == null)
            {
                return NotFound();
            }

            _context.Cuentas.Remove(cuenta);
            await _context.SaveChangesAsync();

            return cuenta;
        }

        private bool CuentaExists(int id)
        {
            return _context.Cuentas.Any(e => e.Id == id);
        }
    }
}
