
using System;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        public readonly DataContext _context;
        public BuggyController(DataContext context)
        {
            _context = context;
        }

    [Authorize]
    [HttpGet("auth")]
    public ActionResult<string> GetSecret()
    {
        return "Secret Text";
    }

    [HttpGet("not-found")]
    public ActionResult<AppUser> GetNotFound()
    {
        var thing = _context.Users.Find(-1);
        
        if(thing==null) return NotFound();
        return thing;
    }

    [HttpGet("server-Error")]
    public ActionResult<string> GetServerError()
    {
       
        var thing = _context.Users.Find(-1); //thing will have null value
        var thingToReturn = thing.ToString(); //we try to convert thing that is null to string
        return thingToReturn;   // which will result in null reference exception
       
    }

    [HttpGet("bad-request")]
    public ActionResult<string> GetBadRequest()
    {

        return BadRequest("This was not a good request"); //return 400
    }
    }
}