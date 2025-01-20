using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TGBotNVK;

class Program
{
    private static ITelegramBotClient _botClient;
    private static ReceiverOptions _receiverOptions;

    public static bool isBusy;

    public static Dictionary<long, Driver> DriversDataBase = new();
    private static CancellationTokenSource cts;

    static async Task Main()
    {
        _botClient = new TelegramBotClient("8197866349:AAHs9I0YwiTo5QfBchG0-Q5X2eNMbNA-Q6U");
        //_botClient = new TelegramBotClient("8026874216:AAEOlGPMXT_IjX4GvctMpFm044q8tB2C2fc");
        _receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new[]
            {
                UpdateType.Message,
                UpdateType.CallbackQuery
            },
            DropPendingUpdates = true
        };

        cts = new CancellationTokenSource();

        _botClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token);

        await Task.Delay(-1);
    }

    public static async void StartBotWithAnotherUpdateHandler(
        Func<ITelegramBotClient, Update, CancellationToken, Task> newUpdateHandler)
    {
        cts.Cancel();

        cts = new();
        _botClient.StartReceiving(newUpdateHandler, ErrorHandler, _receiverOptions, cts.Token);
        await Task.Delay(-1);
    }

    public static async void StartWithStandardUpdateHandler()
    {
        cts.Cancel();

        cts = new();

        _botClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token);
        await Task.Delay(-1);
    }

    private static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    {
                        MessageHandler.ProcessMessage(msg: update.Message, botClient);
                        return;
                    }
                case UpdateType.CallbackQuery:
                    {
                        MessageHandler.CheckStartChoice(botClient, update);
                        return;
                    }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
    {
        var ErrorMessage = error switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => error.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }

    public static bool IsDriverIdInDataBase(long id)
    {
        return DriversDataBase.ContainsKey(id);
    }

    public static Driver GetDriverFromDatabse(long id)
    {
        if (IsDriverIdInDataBase(id))
        {
            return DriversDataBase[id];
        }
        else
        {
            var driver = new Driver(id);
            AddDriverToDataBase(driver);
            return driver;
        }
    }

    public static void AddDriverToDataBase(Driver driver)
    {
        DriversDataBase[driver.TGId] = driver;
    }
}
