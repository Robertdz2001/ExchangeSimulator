﻿using ExchangeSimulator.Application.Pagination;
using ExchangeSimulator.Application.Pagination.Enums;
using ExchangeSimulator.Application.Repositories;
using ExchangeSimulator.Application.Services;
using ExchangeSimulator.Domain.Enums;
using MediatR;

namespace ExchangeSimulator.Application.Requests.GameRequests.GetAllCurrentGames;

public class GetAllCurrentGamesRequestHandler : IRequestHandler<GetAllCurrentGamesRequest, PagedResult<GetAllCurrentGamesDto>>
{
    private readonly IGameRepository _gameRepository;
    private readonly IUserContextService _userContext;

    public GetAllCurrentGamesRequestHandler(IGameRepository gameRepository, IUserContextService userContext)
    {
        _gameRepository = gameRepository;
        _userContext = userContext;
    }
    public async Task<PagedResult<GetAllCurrentGamesDto>> Handle(GetAllCurrentGamesRequest request, CancellationToken cancellationToken)
    {

        var userId = _userContext.GetUserId()!.Value;
        var games = await _gameRepository.GetGamesByUserId(userId);

        games = games.Where(x => x.Status != GameStatus.Finished);

        switch (request.SortOption)
        {
            case GameSortOption.Date:
                games = games.OrderByDescending(x => x.CreatedAt);
                break;
            case GameSortOption.Name:
                games = games.OrderBy(x => x.Name);
                break;
            case GameSortOption.Owner:
                games = games.OrderBy(x => x.Owner.Username);
                break;
        }

        if (request.GameName is not null)
        {
            games = games.Where(x => x.Name.Contains(request.GameName, StringComparison.OrdinalIgnoreCase));
        }

        if (request.OwnerName is not null)
        {
            games = games.Where(x => x.Owner.Username.Contains(request.OwnerName, StringComparison.OrdinalIgnoreCase));
        }

        var gameDtos = games.Select(game => new GetAllCurrentGamesDto
        {
            Name = game.Name,
            Description = game.Description,
            CreatedAt = game.CreatedAt,
            AvailableSpots = game.TotalPlayers - game.Players.Count,
            PlayersRatio = 100 * game.Players.Count / game.TotalPlayers,
            TimeRatio = game.StartsAt.HasValue && game.Duration.TotalMinutes > 0
            ? Math.Max(0.00, Math.Min(100.00, 100 * (DateTime.UtcNow - game.StartsAt.Value).TotalMinutes / game.Duration.TotalMinutes)) : 0.00,
            OwnerName = game.Owner.Username,
            Status = game.Status
        });

        var pagedResult = new PagedResult<GetAllCurrentGamesDto>(gameDtos.ToList(), gameDtos.Count(), 6, request.PageNumber);

        return pagedResult;
    }
}