using CareConnect.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareConnect.WPF.Workers
{
    public class CodesWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        private int ExpiringMinutes { get; }

        public CodesWorker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;

            ExpiringMinutes = 5;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var modelContext = scope.ServiceProvider.GetRequiredService<ModelContext>();

                        var codes = await modelContext.Codes.ToListAsync(stoppingToken);
                        var expiredCodes = codes.Where(c => (DateTime.Now - c.DateCreated).TotalMinutes >= ExpiringMinutes).ToList();

                        if (expiredCodes.Any())
                        {
                            modelContext.Codes.RemoveRange(expiredCodes);
                            await modelContext.SaveChangesAsync(stoppingToken);
                        }
                    }

                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
                catch (OperationCanceledException) { }
            }
        }
    }
}
