using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using UserService.DTOs;
using UserService.Models;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;

        public UsersController(IMapper mapper)
        {
            _mapper = mapper;
        }
        
        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> Create(CreateUserDto createUserDto)
        {
            var existingUserInfo = await DB.Find<UserInfo>()
                .Match(u => u.Username == createUserDto.Username)
                .ExecuteFirstAsync();
                
            if (existingUserInfo != null)
            {
                return BadRequest("User already exists");
            }
            
            var userInfo = _mapper.Map<UserInfo>(createUserDto);
            await userInfo.SaveAsync();
            
            var responseDto = _mapper.Map<UserResponseDto>(userInfo);
            return CreatedAtAction(
                nameof(GetByUsername),
                new { username = userInfo.Username },
                responseDto
            );
        }
        
        [HttpGet("{username}")]
        public async Task<ActionResult<UserResponseDto>> GetByUsername(string username)
        {
            var userInfo = await DB.Find<UserInfo>()
                .Match(u => u.Username == username)
                .ExecuteFirstAsync();
                
            if (userInfo == null)
            {
                return NotFound();
            }
            
            var responseDto = _mapper.Map<UserResponseDto>(userInfo);
            return Ok(responseDto);
        }
        
        [HttpGet]
        public async Task<ActionResult<List<UserResponseDto>>> GetAll()
        {
            var userInfos = await DB.Find<UserInfo>().ExecuteAsync();
            var responseDtos = _mapper.Map<List<UserResponseDto>>(userInfos);
            return Ok(responseDtos);
        }
        
        [HttpPut("{username}")]
        public async Task<IActionResult> Update(string username, UpdateUserDto updateUserDto)
        {
            var userInfo = await DB.Find<UserInfo>()
                .Match(u => u.Username == username)
                .ExecuteFirstAsync();
                
            if (userInfo == null)
            {
                return NotFound();
            }

            if (userInfo.Username != User.Identity.Name)
            {
                return Forbid();
            }

            // Update only the properties that are defined in the UpdateUserDto
            userInfo.Description = updateUserDto.Description;
            userInfo.Fullname = updateUserDto.Fullname;
            userInfo.WhoCanMessage = updateUserDto.WhoCanMessage;
            
            if (updateUserDto.ChatsId != null)
            {
                userInfo.ChatsId = updateUserDto.ChatsId;
            }

            await userInfo.SaveAsync();
            return NoContent();
        }
        
        [HttpDelete("{username}")]
        public async Task<IActionResult> Delete(string username)
        {
            var userInfo = await DB.Find<UserInfo>()
                .Match(u => u.Username == username)
                .ExecuteFirstAsync();
                
            if (userInfo == null)
            {
                return NotFound();
            }

            if (userInfo.Username != User.Identity.Name)
            {
                return Forbid();
            }

            await userInfo.DeleteAsync();
            return NoContent();
        }
    }
}
