﻿using ExchangeSimulator.Application.Repositories;
using ExchangeSimulator.Domain.Entities;
using ExchangeSimulator.Domain.Enums;
using ExchangeSimulator.Infrastructure.EF.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace ExchangeSimulator.Infrastructure.EF.Repositories;

/// <summary>
/// implementation of game repository
/// </summary>
public class GameRepository : IGameRepository 
{
    private readonly ExchangeSimulatorDbContext _dbContext;

    public GameRepository(ExchangeSimulatorDbContext dbContext) {
        _dbContext = dbContext;
    }

    ///<inheritdoc/>
    public async Task CreateGame(Game game) 
    {
        await _dbContext.Games.AddAsync(game);
        await _dbContext.SaveChangesAsync();
    }


    public async Task<Game?> GetGameById(Guid id) 
        => await _dbContext.Games
            .Include(x => x.StartingCoins)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<Game?> GetGameByName(string name) 
        => await _dbContext.Games
            .Include(x => x.StartingCoins)
            .Include(x => x.Players)
            .FirstOrDefaultAsync(x => x.Name == name);

    public async Task<IEnumerable<Game>> GetAllGamesByStatus(GameStatus status) 
        => await _dbContext.Games
            .Include(x => x.Owner)
            .Include(x => x.Players)
            .Where(x => x.Status == status)
            .ToListAsync();
}