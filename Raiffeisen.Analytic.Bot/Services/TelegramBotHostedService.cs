using Microsoft.Extensions.Hosting;

namespace Raiffeisen.Analytic.Bot.Services;

public class TelegramBotHostedService: IHostedService
{
    private readonly BotProcessor _bot;

    public TelegramBotHostedService(string token)
    {
        _bot = new BotProcessor(token);
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _bot.StartBot(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}