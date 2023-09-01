using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using W3.Domain.Entities.SideMenu;
using W3.Infrastructure.DataContext;
using W3.WebApi.DTOs.ResponseDTO;

namespace W3.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class sidemenusController : ControllerBase
    {
        private readonly Context _context;

        public sidemenusController(Context context)
        {
            _context = context;
        }

        // GET: api/sidemenus

        [HttpGet("GetMenu")]
        public ActionResult<Status> GetMenu(string Role)
        {
            try
            {
                var menuItems = _context.sidemenus
                                        .OrderBy(s => s.SortNummber)
                                        .Where(p => p.Role == Role)
                                        .Include(a => a.SubMenu)
                                        .Select(x => new
                                        {
                                            sidemenupath = x.Path,
                                            sidemenuname = x.Name,
                                            sidemenuIcon = x.Icon,
                                            subcategorypath = x.SubMenu
                                            .OrderBy(s => s.SortNummber)
                                            .Where(y => y.MenuId == x.MenuId)
                                            .Select(z => new
                                             {
                                                SubMenuPath = z.Path,
                                                SubMenuname = z.Name,
                                                SubMenuicon = z.Icon
                                             })
                                        }).ToList();

                if (menuItems.Count == 0)
                {
                    return NotFound();
                }
                else
                {
                    return new Status(StatusCode(200))
                    {
                        data = menuItems,
                        success = true,
                        Message = "Done"
                    };

                }
            }

            catch (Exception ex)
            {
                return new Status(StatusCode(500))
                {
                    success = false,
                    Message = ex.Message,

                };
            }

        }

        // PUT: api/sidemenus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putsidemenu(int id, sidemenu sidemenu)
        {
            if (id != sidemenu.MenuId)
            {
                return BadRequest();
            }

            _context.Entry(sidemenu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!sidemenuExists(id))
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

        // POST: api/sidemenus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<sidemenu>> Postsidemenu(sidemenu sidemenu)
        {
            _context.sidemenus.Add(sidemenu);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getsidemenu", new { id = sidemenu.MenuId }, sidemenu);
        }

        // DELETE: api/sidemenus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletesidemenu(int id)
        {
            var sidemenu = await _context.sidemenus.FindAsync(id);
            if (sidemenu == null)
            {
                return NotFound();
            }

            _context.sidemenus.Remove(sidemenu);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool sidemenuExists(int id)
        {
            return _context.sidemenus.Any(e => e.MenuId == id);
        }
    }
}
