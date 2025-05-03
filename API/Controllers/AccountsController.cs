using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountsController(AppDbContext _context,ITokenService tokenService) : BaseAPIController
{

    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> Register(RegisterDTO register){

        if(await UserExists(register.UserName)) return BadRequest("Ussername is taken");

        using var hmac=new HMACSHA512();
        var user=new AppUser{
            UserName=register.UserName,
            PassHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(register.Password)),
            PassSalt=hmac.Key
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new UserDTO{
            Username=user.UserName,
            Token=tokenService.CreateToken(user)
        };

    }
    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login(LoginDTO login){
        var user=await _context.Users.FirstOrDefaultAsync(u=>u.UserName.ToLower()==login.UserName.ToLower());

        if(user==null) return Unauthorized("Invalid Username");

        using var hmac=new HMACSHA512(user.PassSalt);
        var computedHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));
        for (int i = 0; i < computedHash.Length; i++)
        {
            if(computedHash[i] != user.PassHash[i]) return Unauthorized("Invalid Password");
        }
        return new UserDTO{
            Username=user.UserName,
            Token=tokenService.CreateToken(user)
        };
    }

    private async Task<bool> UserExists(string userName){
        return await _context.Users.AnyAsync(u=>u.UserName.ToLower()==userName.ToLower());
    }

}
