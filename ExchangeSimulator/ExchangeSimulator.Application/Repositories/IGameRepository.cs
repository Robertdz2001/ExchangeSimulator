﻿
using ExchangeSimulator.Domain.Entities;
using ExchangeSimulator.Domain.Enums;

namespace ExchangeSimulator.Application.Repositories;

/// <summary>
/// Interface for game repository
/// </summary>
public interface IGameRepository {
    /// <summary>
    /// Adds game
    /// </summary>
    /// <param name="game"></param>
    /// <returns></returns>
    Task CreateGame(Game game);

    /// <summary>
    /// Gets game by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Game?> GetGameById(Guid id);

    /// <summary>
    /// Gets game by name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<Game?> GetGameByName(string name);

    Task<IEnumerable<Game>> GetAllGamesByStatus(GameStatus status);
}
