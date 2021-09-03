using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServerAspNetIdentity.Quickstart
{
    // https://identityserver4.readthedocs.io/en/latest/topics/add_apis.html
    [Route("[controller]")]
    public class LocalApiController : ControllerBase
    {

        /// <summary>
        /// test
        /// </summary>
        /// <returns></returns>
        [HttpGet("Test")]
        [Authorize(Policy = LocalApi.PolicyName)]
        public ActionResult<string> Test()
        {
            return "local api";
        }

        ///// <summary>
        ///// 发送消息，并在缓存中记录
        ///// </summary>
        ///// <param name="smsService"></param>
        ///// <param name="userService"></param>
        ///// <param name="dto"></param>
        ///// <returns></returns>
        //[AllowAnonymous]
        //[HttpPost("SendPhoneCode")]
        //[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        //public async Task<IActionResult> SendPhoneCodeAsync(
        //    [FromServices] ISmsService smsService,
        //    [FromServices] IUserService userService,
        //    [FromBody] SendPhoneCodeDTO dto)
        //{
        //    var exists = await userService.CheckUserExistsByPhoneNumberAsync(dto.PhoneNumber);
        //    if (!exists)
        //        return NotFound("该用户没有被注册，请联系管理员！");
        //    var info = await smsService.SendValidationCodeAsync(dto.PhoneNumber);
        //    if (info.StartsWith("F"))
        //    {
        //        return BadRequest(info);
        //    }
        //    return Ok(info);
        //}


        ///// <summary>
        ///// 读取所有角色
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("Roles")]
        //[Authorize(Policy = LocalApi.PolicyName, Roles = "Admin")]
        //[ProducesResponseType(typeof(IEnumerable<RoleDTO>), StatusCodes.Status200OK)]
        //public async Task<IActionResult> GetRoles(
        //        [FromServices] IRoleService roleService
        //    )
        //{
        //    return Ok(await roleService.GetAllAsync());
        //}

        ///// <summary>
        ///// 创建用户
        ///// </summary>
        ///// <param name="createUpdateDTO"></param>
        ///// <param name="userService"></param>
        ///// <returns></returns>
        //[Authorize(Policy = LocalApi.PolicyName, Roles = "Admin")]
        //[HttpPost("Users")]
        //[ProducesResponseType(typeof(IdentityResult), StatusCodes.Status201Created)]
        //public async Task<IActionResult> CreateUserAsync(
        //    [FromBody] UserCreateUpdateDTO createUpdateDTO,
        //    [FromServices] IUserService userService)
        //{
        //    IdentityResult result = await userService.CreateUserAsync(createUpdateDTO);
        //    return Created("", result);
        //}


        ///// <summary>
        ///// 更新用户信息
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="createUpdateDTO"></param>
        ///// <param name="userService"></param>
        ///// <returns></returns>
        //[Authorize(Policy = LocalApi.PolicyName, Roles = "Admin")]
        //[HttpPut("Users/{id}")]
        //[ProducesResponseType(typeof(IdentityResult), StatusCodes.Status200OK)]
        //public async Task<IActionResult> UpdateUserAsync(
        //    [FromRoute] string id,
        //    [FromBody] UserCreateUpdateDTO createUpdateDTO,
        //    [FromServices] IUserService userService)
        //{
        //    IdentityResult result = await userService.UpdateUserAsync(id, createUpdateDTO);
        //    return Ok(result);
        //}

        ///// <summary>
        ///// 读取所有用户
        ///// </summary>
        ///// <param name="userService"></param>
        ///// <returns></returns>
        //[Authorize(Policy = LocalApi.PolicyName, Roles = "Admin")]
        //[HttpGet("Users")]
        //[ProducesResponseType(typeof(List<UserDTO>), StatusCodes.Status200OK)]
        //public async Task<IActionResult> GetUsersAsync([FromServices] IUserService userService)
        //{
        //    List<UserDTO> users = await userService.GetUsersAsync();
        //    return Ok(users);
        //}

        ///// <summary>
        ///// 根据id读取用户
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="userService"></param>
        ///// <returns></returns>
        //[Authorize(Policy = LocalApi.PolicyName, Roles = "Admin")]
        //[HttpGet("Users/{id}")]
        //[ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        //public async Task<IActionResult> GetUserByIdAsync(
        //    [FromRoute] string id,
        //    [FromServices] IUserService userService)
        //{
        //    var user = await userService.GetUserByIdAsync(id);
        //    return Ok(user);
        //}

        ///// <summary>
        ///// 删除用户
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="userService"></param>
        ///// <returns></returns>
        //[Authorize(Policy = LocalApi.PolicyName, Roles = "Admin")]
        //[HttpDelete("Users/{id}")]
        //[ProducesResponseType(typeof(IdentityResult), StatusCodes.Status200OK)]
        //public async Task<IActionResult> DeleteUserAsync(
        //        [FromRoute] string id,
        //        [FromServices] IUserService userService
        //    )
        //{
        //    IdentityResult result = await userService.DeleteUserAsync(id);
        //    return Ok(result);
        //}

        ///// <summary>
        ///// 将用户加入角色
        ///// </summary>
        ///// <param name="userid"></param>
        ///// <param name="roleName"></param>
        ///// <param name="userService"></param>
        ///// <returns></returns>
        //[Authorize(Policy = LocalApi.PolicyName, Roles = "Admin")]
        //[HttpPost("UserToRole/{userId}/{roleName}")]
        //[ProducesResponseType(typeof(IdentityResult), StatusCodes.Status200OK)]
        //public async Task<IActionResult> AddUserToRoleAsync(
        //    [FromRoute] string userid,
        //    [FromRoute] string roleName,
        //    [FromServices] IUserService userService
        //)
        //{
        //    IdentityResult result = await userService.AddUserToRoleAsync(userid, roleName);
        //    return Ok(result);
        //}

        ///// <summary>
        ///// 读取用户的所有角色
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="userService"></param>
        ///// <returns></returns>
        //[Authorize(Policy = LocalApi.PolicyName, Roles = "Admin")]
        //[HttpGet("UserToRole/GetUserRoles")]
        //[ProducesResponseType(typeof(IList<string>), StatusCodes.Status200OK)]
        //public async Task<IActionResult> GetUserRolesAsync(
        //    [FromQuery] string userId,
        //    [FromServices] IUserService userService
        //)
        //{
        //    var roles = await userService.GetUserRolesAsync(userId);
        //    return Ok(roles);
        //}

        ///// <summary>
        ///// 根据角色查询用户
        ///// </summary>
        ///// <param name="roleName">为空则查询所有无角色用户，不为空查询该角色的所有用户</param>
        ///// <param name="userService"></param>
        ///// <returns></returns>
        //[Authorize(Policy = LocalApi.PolicyName, Roles = "Admin")]
        //[HttpGet("UserToRole/GetUsersByRole")]
        //[ProducesResponseType(typeof(IList<UserDTO>), StatusCodes.Status200OK)]
        //public async Task<IActionResult> GetUsersByRoleAsync(
        //           [FromQuery] string roleName,
        //           [FromServices] IUserService userService
        //       )
        //{
        //    var users = await userService.GetUsersByRoleAsync(roleName);
        //    return Ok(users);
        //}



        ///// <summary>
        ///// 删除用户的角色
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="roleName"></param>
        ///// <param name="userService"></param>
        ///// <returns></returns>
        //[Authorize(Policy = LocalApi.PolicyName, Roles = "Admin")]
        //[HttpDelete("UserToRole/{userId}/{roleName}")]
        //[ProducesResponseType(typeof(IdentityResult), StatusCodes.Status200OK)]
        //public async Task<IActionResult> RemoveUserFromRole(
        //    [FromRoute] string userId,
        //    [FromRoute] string roleName,
        //    [FromServices] IUserService userService
        //)
        //{
        //    var result = await userService.RemoveUserFromRole(userId, roleName);
        //    return Ok(result);
        //}


    }
}
