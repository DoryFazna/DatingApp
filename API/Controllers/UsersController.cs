using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers(){
            return Ok(await _userRepository.GetMembersAsync());
            
        }

        // api/users/3
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username){
            return await _userRepository.GetMemberAsync(username);
  
            
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto){
            //This will give the username from the token that the API used to authenticate.. this is the user we are gonna update. 
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userRepository.GetUserByUsernameAsync(username);

            //automapper will automatically maps the values to user object. we dont need to specify individually
            _mapper.Map(memberUpdateDto, user);

            //Now this user is flagged. that it needs to be updated. 
            _userRepository.Update(user);

            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");






        }
    }
}