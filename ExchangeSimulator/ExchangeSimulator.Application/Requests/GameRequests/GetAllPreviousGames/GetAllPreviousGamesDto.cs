﻿namespace ExchangeSimulator.Application.Requests.GameRequests.GetAllPreviousGames;

public class GetAllPreviousGamesDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public int AvailableSpots { get; set; }
    public string OwnerName { get; set; }
}