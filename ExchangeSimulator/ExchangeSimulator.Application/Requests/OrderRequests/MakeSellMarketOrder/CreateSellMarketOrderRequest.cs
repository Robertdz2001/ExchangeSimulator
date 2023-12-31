﻿
using MediatR;

namespace ExchangeSimulator.Application.Requests.OrderRequests.CreateSellMarketOrder;

public class CreateSellMarketOrderRequest : IRequest<Guid> {
    public string GameName { get; set; }
    public Guid PlayerCoinId { get; set; }
    public decimal Quantity { get; set; }
}

