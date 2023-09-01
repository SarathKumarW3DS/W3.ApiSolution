using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using W3.Domain.Entities.Role;
using W3.Domain.Entities.UserDetails;
using W3.Domain.Interfaces;
using W3.Infrastructure.DataContext;
using W3.WebApi.DTOs.RequestDTO;
using W3.WebApi.DTOs.ResponseDTO;
using W3.WebApi.Services.Mail;

namespace W3.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Context _context;
        private readonly ITokenService _tokenService;
        private readonly UserManager<Users> _userManager1;
        private readonly SignInManager<Users> _signInManager1;
        private readonly MailService _mailService;
        private readonly ILoggerManager _logger;
        public UsersController(UserManager<Users> userManager, RoleManager<AppRole> roleManager,ILoggerManager logger,
            SignInManager<Users> signInManager, Context context, ITokenService tokenService,
            MailService mailService)
        {
            _mailService = mailService;
            _logger = logger;
            _tokenService = tokenService;
            _context = context;
            _userManager1 = userManager;
            _signInManager1 = signInManager;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<Status>> Register(RegisterDTO register)
        {
            try
            {
                if (await UserExists(register.userName)) return Unauthorized("Username Is Taken");
                var user = new Users
                {
                    UserName = register.userName.ToLower(),
                    Email = register.email,
                    firstName = register.firstName,
                    lastName = register.lastName,
                    PhoneNumber = register.mobile,
                    alternateMobile = register.alternateMobile,
                    address = register.address
                };
                var result = await _userManager1.CreateAsync(user, register.password);
                if (!result.Succeeded) return Unauthorized("Password Must Has AtLeast 6 Characters.That Includes Uppsercase and Numbers");
                var roleresult = await _userManager1.AddToRoleAsync(user, register.role);
                if (!roleresult.Succeeded) return BadRequest(result.Errors);
                return new Status(StatusCode(200))
                {
                    data=null,
                    Message=user.UserName+" User Added Succeccfully",
                    success=true,
                };
            }
            catch(Exception ex)
            {
                return new Status(StatusCode(500))
                {
                    success=false,
                    Message=ex.Message
                };
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO login)
        {
            try
            {
                var user1 = await _context.users.SingleOrDefaultAsync(x => x.Email == login.Email);
                if (user1 == null) return Unauthorized("Invalid Email Address");
                var result = await _signInManager1.CheckPasswordSignInAsync(user1, login.Password, false);
                if (!result.Succeeded) return Unauthorized("Invalid Password");
                _logger.LogInfo(user1.UserName,"Logged In");

            return new UserDTO
            {
                Token = await _tokenService.CreateToken(user1)
            };
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpGet("getotp")]
        public async Task<ActionResult<string>> GenerateRandomOTP(string mail)
        {
            Random generator = new Random();
            var OTP = generator.Next(0, 1000000).ToString("D6");
            var user1 = await _context.users.SingleOrDefaultAsync(x => x.Email == mail);
            if (user1 == null)
            {
                return Unauthorized("Invalid Email Address. Please Enter Corrected One");
            }
            else
            {
                await _mailService.SendEmailAsync(mail, OTP, "OTP Configuration", "<h1>Your OTP is</h1>");
            }
            return Ok(OTP);
        }

        [HttpPost("updatePassword")]
        public async Task<ActionResult<Object>> Passwordupdate(string email, string password)
        {
            var ChechMail = await _context.users.SingleOrDefaultAsync(x => x.Email == email);
            if (ChechMail == null)
            {
                return Unauthorized("Invalid Email Address. Please Enter Corrected One");
            }
            var hashPassword = _userManager1.PasswordHasher.HashPassword(ChechMail, password);
            var Result = await _context.Users.FromSqlRaw("updatePassword {0},{1}", ChechMail.Id, hashPassword).ToListAsync();
            return Result;
        }

        [Authorize(Policy="RequireHRRole")]
        [HttpGet("GetUsersWithRoles")]
        public async Task<ActionResult> getusers()
        {
            var users = await _userManager1.Users
                .Include(r => r.UserRoles)
                .ThenInclude(r => r.Role)
                .OrderBy(u => u.UserName)
                .Select(u => new
                {
                    u.Id,
                    username = u.UserName,
                    Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                })
                .ToListAsync();
            return Ok(users);
        }
        //Check User validation
        private async Task<bool> UserExists(string username)
        {
            return await _context.users.AnyAsync(x => x.UserName == username.ToLower());
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> Getusers()
        {
            return await _context.users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUsers(int id)
        {
            var users = await _context.users.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            return users;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers(int id, Users users)
        {
            if (id != users.Id)
            {
                return BadRequest();
            }

            _context.Entry(users).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists1(id))
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
        public async Task<IActionResult> DeleteUsers(int id)
        {
            var users = await _context.users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }

            _context.users.Remove(users);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //validate user exist or not 
        private bool UsersExists1(int id)
        {
            return _context.users.Any(e => e.Id == id);
        }
    }
}
