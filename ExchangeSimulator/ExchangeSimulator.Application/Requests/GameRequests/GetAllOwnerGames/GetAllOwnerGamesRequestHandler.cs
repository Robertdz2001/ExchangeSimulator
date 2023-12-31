﻿using ExchangeSimulator.Application.Pagination.Enums;
using ExchangeSimulator.Application.Pagination;
using ExchangeSimulator.Application.Repositories;
using ExchangeSimulator.Application.Services;
using MediatR;

namespace ExchangeSimulator.Application.Requests.GameRequests.GetAllOwnerGames;

public class GetAllOwnerGamesRequestHandler : IRequestHandler<GetAllOwnerGamesRequest, PagedResult<GetAllOwnerGamesDto>>
{
    private readonly IGameRepository _gameRepository;
    private readonly IUserContextService _userContext;

    public GetAllOwnerGamesRequestHandler(IGameRepository gameRepository, IUserContextService userContext)
    {
        _gameRepository = gameRepository;
        _userContext = userContext;
    }
    public async Task<PagedResult<GetAllOwnerGamesDto>> Handle(GetAllOwnerGamesRequest request, CancellationToken cancellationToken)
    {

        var userId = _userContext.GetUserId()!.Value;
        var games = await _gameRepository.GetOwnedGamesByUserId(userId);

        switch (request.SortOption)
        {
            case GameSortOption.Date:
                games = games.OrderByDescending(x => x.CreatedAt);
                break;
            case GameSortOption.Name:
                games = games.OrderBy(x => x.Name);
                break;
        }

        if (request.GameName is not null)
        {
            games = games.Where(x => x.Name.Contains(request.GameName, StringComparison.OrdinalIgnoreCase));
        }


        var gameDtos = games.Select(game => new GetAllOwnerGamesDto {
            Name = game.Name,
            CreatedAt = game.CreatedAt,
            PlayersRatio = 100 * game.Players.Count / game.TotalPlayers,
            TimeRatio = game.StartsAt.HasValue && game.Duration.TotalMinutes > 0 
            ? Math.Max(0.00, Math.Min(100.00, 100 * (DateTime.UtcNow - game.StartsAt.Value).TotalMinutes / game.Duration.TotalMinutes)) : 0.00,
            Status = game.Status,
        });

        var pagedResult = new PagedResult<GetAllOwnerGamesDto>(gameDtos.ToList(), gameDtos.Count(), 18, request.PageNumber);

        return pagedResult;
    }
}