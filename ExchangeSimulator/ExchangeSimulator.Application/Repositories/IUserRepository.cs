﻿using ExchangeSimulator.Domain.Entities;

namespace ExchangeSimulator.Application.Repositories;

/// <summary>
/// Interface for user repository.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Gets user by email name.
    /// </summary>
    /// <param name="email">Email name.</param>
    /// <returns>User.</returns>
    Task<User?> GetUserByEmail(string email);

    /// <summary>
    /// Adds user.
    /// </summary>
    /// <param name="user">User.</param>
    /// <returns></returns>
    Task AddUser(User user);

    /// <summary>
    /// Get user by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<User?> GetUserById(Guid id);

    /// <summary>
    /// Update user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task Update(User user);

}