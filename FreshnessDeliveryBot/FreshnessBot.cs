using FreshnessDeliveryBot.DbContexts;
using FreshnessDeliveryBot.Handler;
using Telegram.Bot;
using Telegram.Bot.Types;

public class FreshnessBot
{
    private readonly TelegramBotClient botClient;
    private readonly PersonService personService ;
    private readonly Dictionary<long, RegistrationHandler> registrationHandlers = new Dictionary<long, RegistrationHandler>();
    private readonly FreshnessBotDbContext freshnessBotDbContext;

    public FreshnessBot(string token)
    {
        botClient = new TelegramBotClient(token);
        freshnessBotDbContext = new FreshnessBotDbContext();
        personService = new PersonService(freshnessBotDbContext);
        botClient.StartReceiving(Update, Error);

        Console.ReadLine();
    }

    private async Task Update(ITelegramBotClient client, Update update, CancellationToken token)
    {
        try
        {
            long userId = update.Message?.From?.Id ?? update.CallbackQuery?.From?.Id ?? 0;

            // Проверка наличия пользователя в базе данных
            if (!await personService.UserExistsAsync(userId))
            {
                if (!registrationHandlers.TryGetValue(userId, out var registrationHandler))
                {
                    registrationHandler = new RegistrationHandler(client, freshnessBotDbContext);
                    registrationHandlers[userId] = registrationHandler;
                }

                await registrationHandler.HandleRegistration(update);
            }
            else
            { 
                var messageHandler = new MessageHandler(botClient, freshnessBotDbContext); 
                await messageHandler.HandleUpdate(update);

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
    {
        Console.WriteLine(exception.Message);
        return Task.CompletedTask;
    }
}
