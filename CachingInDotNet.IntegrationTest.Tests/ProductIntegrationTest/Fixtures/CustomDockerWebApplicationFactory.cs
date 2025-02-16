//<copyright file="CustomDockerWebApplicationFactory" Owner=tjtechy> 
//Author: Tajudeen Busari
//Date: 2025-14-01
//</copyright>
using CachingInDotNet.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace CachingInDotNet.IntegrationTest.Tests.ProductIntegrationTest.Fixtures;

/// <summary>
/// Custom Docker Web Application Factory for Integration Test
/// On Initialization, it starts the Postgres and Redis container and creates the database schema
/// After tests, it stops the Postgres and Redis container
/// </summary>
public class CustomDockerWebApplicationFactory: WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer;
    private readonly RedisContainer _redisContainer;

    public CustomDockerWebApplicationFactory()
    {
        _postgreSqlContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .Build();
        
        _redisContainer = new RedisBuilder()
            .WithImage("redis:latest")
            .WithPortBinding(6379) // Expose Redis port
            .Build();
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var connectionString = _postgreSqlContainer.GetConnectionString();
        var redisConnectionString = _redisContainer.GetConnectionString();
        base.ConfigureWebHost(builder);
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
            
            //Replace the Redis connection string with the test container connection string
            services.RemoveAll(typeof(RedisConfiguration));
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                return ConnectionMultiplexer.Connect(redisConnectionString);
            });

        });

    }
    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
        await _redisContainer.StartAsync();

        using (var scope = Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ApplicationDbContext>();
            await db.Database.EnsureCreatedAsync();
            await db.SaveChangesAsync();
        }   
        
    }

    public async Task DisposeAsync()
    {
        await _postgreSqlContainer.StopAsync();
        await _redisContainer.StopAsync();
    }
}