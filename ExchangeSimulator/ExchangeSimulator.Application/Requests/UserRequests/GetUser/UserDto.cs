﻿
namespace ExchangeSimulator.Application.Requests.UserRequests.GetUser;
public class UserDto {
    public string Email { get; set; }
    public string UserName { get; set; }
    public string? ImageUrl { get; set; }
    public int Review { get; set; }

}

