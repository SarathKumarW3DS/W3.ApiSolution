using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using W3.Domain.Entities.UserDetails;
using Microsoft.EntityFrameworkCore;
using W3.Infrastructure.DataContext;
using W3.WebApi.DTOs.ResponseDTO;
using System.Linq;

namespace W3.WebApi.Middleware
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThrowError : ControllerBase
    {
        private readonly Context _context;
        public ThrowError(Context context)
        {
            _context = context;
        }
        [HttpGet("not-found")]
        public ActionResult<Status> GetNotFound()
        {
            try
            {
                var x = _context.users.ToListAsync();
                if (x == null) return NotFound();
                return Ok(x);
            }
            catch (Exception ex)
            {
                return new Status(StatusCode(500))
                {
                    success = false,
                    Message = ex.Message.ToString()
                };
            }

        }

        [HttpGet("server-error")]
        public ActionResult<Status> GetServerError()
        {
            try
            {
                var thing = _context.users.Find();
                var thingToReturn = thing.ToString();
                return Ok(thingToReturn);
            }
            catch (Exception ex)
            {
                return new Status(StatusCode(500))
                {
                    success = false,
                    Message = ex.Message.ToString()
                };
            }
        }
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return "This is not a good request";
        }
    }
}