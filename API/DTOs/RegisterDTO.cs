using System;

namespace API.DTOs;

public class RegisterDTO
{
    public required string UserName { get; set; }
    public required string Password { get; set; }

}
