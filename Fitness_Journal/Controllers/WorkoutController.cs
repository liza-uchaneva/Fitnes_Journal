﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fitness_Journal.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Fitness_Journal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkouController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WorkouController(ApplicationDbContext context)
        {
            _context = context;
        }


        //// GET: api/profileId/workout
        //[Authorize]
        //[HttpGet("/profileId/workout")]
        //public async Task<ActionResult<int>> GetProfileIdFromUser()
        //{
        //    var IdentUser = await _userManager.GetUserAsync(User);
        //    if (IdentUser != null)
        //    {
        //        var user = _context.Users.FirstOrDefault(u => u.Email == IdentUser.Email);
        //        if(user !=null)
        //        {
        //            return Ok(user.ProfileId);

        //        }
        //        else
        //        {
        //            return NotFound("User id not found.");
        //        }
        //    }
        //    else
        //    {
        //        return NotFound("Profile id not found.");
        //    }
        //}
    }
}
