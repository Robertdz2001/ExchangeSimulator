﻿using ExchangeSimulator.Infrastructure.EF.Contexts;
using ExchangeSimulator.Infrastructure.EF.Options;
using ExchangeSimulator.Shared.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeSimulator.Infrastructure.EF;

public static class Extensions
{
    public static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
    {

        var options = configuration.GetOptions<PostgresOptions>("Postgres");

        services.AddDbContext<ExchangeSimulatorDbContext>(ctx
            => ctx.UseNpgsql(options.ConnectionString));

        return services;
    }
}