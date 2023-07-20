﻿using ExchangeSimulator.Domain.Entities;
using ExchangeSimulator.Domain.Enums;
using ExchangeSimulator.Infrastructure.EF.Contexts;

namespace ExchangeSimulator.Api.Tests;

public static partial class DbFiller
{
    public static async Task AddGamesForPagination(this ExchangeSimulatorDbContext dbContext)
    {
        var gamesList = new List<Domain.Entities.Game>();

        for (var i = 0; i < 20; i++)
        {
            gamesList.Add(new Domain.Entities.Game()
            {
                CreatedAt = DateTime.UtcNow.AddDays(i),
                Description = "Description",
                Duration = TimeSpan.FromHours(20),
                Id = Guid.NewGuid(),
                Money = 1000,
                Name = $"Game{i}",
                NumberOfPlayers = 10,
                OwnerId = Guid.Parse(Constants.UserId),
                PasswordHash = "PasswordHash",
                Status = (GameStatus) (i%3)
            });
        }

        await dbContext.Games.AddRangeAsync(gamesList);
        await dbContext.SaveChangesAsync();
    }

    public static async Task AddGamesWithPlayersForPagination(this ExchangeSimulatorDbContext dbContext)
    {
        var gamesList = new List<Domain.Entities.Game>();

        for (var i = 0; i < 20; i++)
        {
            gamesList.Add(new Domain.Entities.Game()
            {
                CreatedAt = DateTime.UtcNow.AddDays(i),
                Description = "Description",
                Duration = TimeSpan.FromHours(20),
                Id = Guid.NewGuid(),
                Money = 1000,
                Name = $"Game{i}",
                NumberOfPlayers = 10,
                OwnerId = Guid.Parse(Constants.UserId),
                PasswordHash = "PasswordHash",
                Status = (GameStatus)(i % 3),
                Players = new()
                {
                    new Player
                    {
                        Id = Guid.NewGuid(),
                        UserId = Guid.Parse(Constants.UserId),
                        Money = 1000,
                        Name = $"Player{i}"
                    }
                }
            });
        }

        await dbContext.Games.AddRangeAsync(gamesList);
        await dbContext.SaveChangesAsync();
    }
}