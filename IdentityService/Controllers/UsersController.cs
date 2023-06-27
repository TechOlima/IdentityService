using IdentityService.Models;
using IdentityService.Models.Operations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IdentityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IdentityContext _context;

        public UsersController(IdentityContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserGet>>> GetUser()
        {
            if (_context.User == null)
            {
                return NotFound();
            }
            return await _context.User
                .Include(i => i.role)
                .ThenInclude(i => i.scopes)
                .ThenInclude(i => i.scope)
                .Select(i=> new UserGet(i)).ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserGet>> GetUser(int id)
        {
            if (_context.User == null)
            {
                return NotFound();
            }
            var user = await _context.User
                .Include(i=> i.role)
                .ThenInclude(i=> i.scopes)
                .ThenInclude(i => i.scope)
                .FirstOrDefaultAsync(i=> i.id ==id);

            if (user == null)
            {
                return NotFound();
            }

            return new UserGet(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserPut userPut)
        {
            if (id != userPut.id)
            {
                return BadRequest();
            }
            var user = await _context.User.FindAsync(id);

            user.name = userPut.name;
            user.email = userPut.email;
            user.role = _context.Role.FirstOrDefault(i=> i.name == userPut.role);
            user.login = userPut.login;
            user.phone = userPut.phone;

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.User == null)
            {
                return NotFound();
            }
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.User?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
