﻿using ExchangeSimulator.Domain.Entities;

namespace ExchangeSimulator.Application.Requests.PlayerRequests.GetMyPlayer;

public class GetMyPlayerDto
{
    /// <summary>
    /// 
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Player name (based on UserName)
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Amount of money that player has in game
    /// </summary>
    public decimal TotalBalance { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public decimal LockedBalance { get; set; }

    /// <summary>
    /// Amount of trades that player made
    /// </summary>
    public int Trades { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int BuyTrades { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int SellTrades { get; set; }

    /// <summary>
    /// Total volume
    /// </summary>
    public decimal TurnOver { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int CreatedOrders { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int BuyCreated { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int SellCreated { get; set; }

    /// <summary>
    /// List of players coins
    /// </summary>
    public List<PlayerCoinDto> PlayerCoins { get; set; }

    public class PlayerCoinDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Coin name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Amount of coins
        /// </summary>
        public decimal TotalBalance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal LockedBalance { get; set; }

        /// <summary>
        /// Image of coin
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Turn over of coin
        /// </summary>
        public decimal TurnOver { get; set; }
    }
}