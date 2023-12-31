﻿using ExchangeSimulator.Application.Repositories;
using ExchangeSimulator.Application.Services;
using ExchangeSimulator.Shared.Exceptions;
using MediatR;

namespace ExchangeSimulator.Application.Requests.PlayerRequests.GetMyPlayer;

public class GetMyPlayerRequestHandler : IRequestHandler<GetMyPlayerRequest,GetMyPlayerDto>
{
    private readonly IUserContextService _userContextService;
    private readonly IPlayerRepository _playerRepository;

    public GetMyPlayerRequestHandler(IUserContextService userContextService, IPlayerRepository playerRepository)
    {
        _userContextService = userContextService;
        _playerRepository = playerRepository;
    }

    public async Task<GetMyPlayerDto> Handle(GetMyPlayerRequest request, CancellationToken cancellationToken)
    {
        var userId = _userContextService.GetUserId()!.Value;
        var player = await _playerRepository.GetPlayerByUserIdAndGameName(request.GameName, userId)
                     ?? throw new NotFoundException("Player was not found");

        return new GetMyPlayerDto
        {
            Id = player.Id,
            Name = player.Name,
            TotalBalance = player.TotalBalance,
            LockedBalance = player.LockedBalance,
            Trades = player.Trades,
            BuyTrades = player.BuyTrades,
            SellTrades = player.SellTrades,
            TurnOver = player.TurnOver,
            CreatedOrders = player.CreatedOrders,
            BuyCreated = player.BuyCreated,
            SellCreated = player.SellCreated,
            PlayerCoins = player.PlayerCoins.Select(coin => new GetMyPlayerDto.PlayerCoinDto
            {
                Id = coin.Id,
                ImageUrl = coin.ImageUrl,
                Name = coin.Name,
                TotalBalance = coin.TotalBalance,
                LockedBalance = coin.LockedBalance,
                TurnOver = coin.TurnOver,
            }).ToList()
        };
    }
}