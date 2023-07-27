﻿using ExchangeSimulator.Application.Requests.GameRequests.JoinGame;
using ExchangeSimulator.Domain.Enums;
using ExchangeSimulator.Infrastructure.EF.Contexts;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using ExchangeSimulator.Domain.Entities;
using ExchangeSimulator.Application.Pagination;
using ExchangeSimulator.Application.Requests.GameRequests.GetAllAvailableGames;
using Microsoft.EntityFrameworkCore;

namespace ExchangeSimulator.Api.Tests.Game;

public class JoinGameTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory<Program> _factory;
    private readonly ExchangeSimulatorDbContext _dbContext;

    public JoinGameTests()
    {
        _factory = new TestWebApplicationFactory<Program>();

        _client = _factory.CreateClient();

        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();

        var scope = scopeFactory.CreateScope();

        _dbContext = scope.ServiceProvider.GetService<ExchangeSimulatorDbContext>();
    }

    [Fact]
    public async Task JoinGame_Should_Create_Player_With_Coins_On_Success()
    {
        //given
        await _dbContext.Init();
        await _dbContext.AddUser();
        var gameId = Guid.NewGuid();
        await _dbContext.AddGame(gameId, "Game1", GameStatus.Available);

        var request = new JoinGameRequest()
        {
            GameName = "Game1",
            Password = Constants.Password
        };

        var json = JsonConvert.SerializeObject(request);

        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

        //when
        var response = await _client.PostAsync("api/game/join-game", httpContent);

        //then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var player = await _dbContext.Players.FirstOrDefaultAsync(x => x.GameId == gameId);
        var playerCoins = await _dbContext.PlayerCoins.Where(x => x.PlayerId == player.Id).ToListAsync();

        player.Should().BeEquivalentTo(ReturnExamplePlayer(gameId), opts => opts.Excluding(x => x.Id));
        playerCoins.Should().BeEquivalentTo(ReturnExamplePlayerCoins(player.Id), opts => opts.Excluding(x => x.Id));
    }

    /// <summary>
    /// Game was not found.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task JoinGame_Returns_NotFound_On_Fail()
    {
        //given
        await _dbContext.Init();
        await _dbContext.AddUser();
        var gameId = Guid.NewGuid();
        await _dbContext.AddGame(gameId, "Game1", GameStatus.Available);

        var request = new JoinGameRequest()
        {
            GameName = "Game2",
            Password = Constants.Password
        };

        var json = JsonConvert.SerializeObject(request);

        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

        //when
        var response = await _client.PostAsync("api/game/join-game", httpContent);

        //then
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Game is not available.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task JoinGame_Returns_BadRequest_On_Fail()
    {
        //given
        await _dbContext.Init();
        await _dbContext.AddUser();
        var gameId = Guid.NewGuid();
        await _dbContext.AddGame(gameId, "Game1", GameStatus.Active);

        var request = new JoinGameRequest()
        {
            GameName = "Game1",
            Password = Constants.Password
        };

        var json = JsonConvert.SerializeObject(request);

        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

        //when
        var response = await _client.PostAsync("api/game/join-game", httpContent);

        //then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    public Player ReturnExamplePlayer(Guid gameId)
        => new()
        {
            GameId = gameId,
            Id = Guid.NewGuid(),
            Money = 1000,
            Name = "TestUserName",
            TradesQuantity = 0,
            TurnOver = 0,
            UserId = Guid.Parse(Constants.UserId)
        };

    public List<PlayerCoin> ReturnExamplePlayerCoins(Guid playerId)
        => new()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Coin1",
                Quantity = 10,
                ImageUrl = "http://image1.com",
                PlayerId = playerId
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Coin2",
                Quantity = 20,
                PlayerId = playerId
            }
        };
}