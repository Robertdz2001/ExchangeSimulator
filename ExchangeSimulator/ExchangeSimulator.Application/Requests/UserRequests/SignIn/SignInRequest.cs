﻿using MediatR;

namespace ExchangeSimulator.Application.Requests.UserRequests.SignIn;

/// <summary>
/// Request for signing in.
/// </summary>
public class SignInRequest : IRequest<SignInDto>
{
    /// <summary>
    /// Email address.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Password.
    /// </summary>
    public string Password { get; set; }
}