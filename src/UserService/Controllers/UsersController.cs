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
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<UserInfoDto>> Create(UserInfoDto userInfo)
        {
            var existingUserInfo = await DB.Find<UserInfo>()
                .Match(u => u.Username == userInfo.Username)
                .ExecuteFirstAsync();
            if (existingUserInfo != null)
            {
                return BadRequest("User already exists");
            }
            var createdUser = new UserInfo();
            createdUser.Username = userInfo.Username;
            createdUser.Fullname = userInfo.Fullname;
            createdUser.Description = userInfo.Description;

            await createdUser.SaveAsync();
            return CreatedAtAction(
                nameof(GetByUsername),
                new { username = userInfo.Username },
                userInfo
            );
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<UserInfo>> GetByUsername(string username)
        {
            var userInfo = await DB.Find<UserInfo>()
                .Match(u => u.Username == username)
                .ExecuteFirstAsync();
            if (userInfo == null)
            {
                return NotFound();
            }
            return Ok(userInfo);
        }

        [HttpGet]
        public async Task<ActionResult<List<UserInfo>>> GetAll()
        {
            var userInfos = await DB.Find<UserInfo>().ExecuteAsync();
            return Ok(userInfos);
        }

        [Authorize]
        [HttpPut("{username}")]
        public async Task<IActionResult> Update(string username, UserInfo updatedUserInfo)
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

            userInfo.Description = updatedUserInfo.Description;
            userInfo.Fullname = updatedUserInfo.Fullname;
            userInfo.WhoCanMessage = updatedUserInfo.WhoCanMessage;
            userInfo.ChatsId = updatedUserInfo.ChatsId;

            await userInfo.SaveAsync();
            return NoContent();
        }

        [Authorize]
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
