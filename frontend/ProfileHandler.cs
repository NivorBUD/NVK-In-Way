using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using static Telegram.Bot.TelegramBotClient;
using Telegram.Bot.Polling;
using System.Threading;

namespace TGBotNVK;
public static class ProfileHandler
{
    private static int creatingProfileStep;
    private static int creatingTripStep;

    public static async void StartCreatingDriverProfile(Message msg, ITelegramBotClient botClient)
    {
        var driver = Program.GetDriverFromDatabse(msg.Chat.Id);

        await botClient.SendMessage(
                        msg.Chat.Id,
                        "Введите марку авто");

        Program.StartBotWithAnotherUpdateHandler(CreateDriverProfile);
    }

    private static async Task CreateDriverProfile(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var msg = update.Message;
        var chat = msg.Chat;
        var driver = Program.GetDriverFromDatabse(msg.Chat.Id);

        switch (creatingProfileStep)
        {
            case 0:
                {
                    driver.SetAutoName(msg.Text);
                    await botClient.SendMessage(
                        chat.Id,
                        "Введите номер авто",
                        cancellationToken: cancellationToken);
                    creatingProfileStep++;
                    return;
                }
            case 1:
                {
                    driver.SetAutoNumber(msg.Text);
                    await botClient.SendMessage(
                        chat.Id,
                        "Введите цвет авто",
                        cancellationToken: cancellationToken);
                    creatingProfileStep++;
                    return;
                }
            case 2:
                {
                    driver.SetAutoColor(msg.Text);
                    Program.isBusy = false;
                    creatingProfileStep = 0;
                    await botClient.SendMessage(
                        chat.Id,
                        driver.ToString(),
                        cancellationToken: cancellationToken);
                    Program.StartWithStandardUpdateHandler();
                    await MessageHandler.PrintDriverMenu(botClient, chat, msg.From.Id);
                    return;
                }
        }
    }

    public static string GetDriverTrips(long driverTGId)
    {
        var driver = Program.GetDriverFromDatabse(driverTGId);

        return driver.GetTrips();
    }

    public static async void CreateTrip(Message msg, ITelegramBotClient botClient)
    {
        var driver = Program.GetDriverFromDatabse(msg.Chat.Id);

        if (!driver.IsProfileComplete)
        {
            await botClient.SendMessage(msg.Chat.Id, "Ваш профиль водителя не заполнен, заполните его");
            Program.isBusy = false;
            await MessageHandler.PrintDriverMenu(botClient, msg.Chat, msg.From.Id);
            return;
        }

        await botClient.SendMessage(
                        msg.Chat.Id,
                        "Введите точку отправления");

        Program.StartBotWithAnotherUpdateHandler(CreatingTrip);
    }

    private static async Task CreatingTrip(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var msg = update.Message;
        var chat = msg.Chat;
        var driver = Program.GetDriverFromDatabse(msg.From.Id);
        driver.CreatedTrip ??= new Trip(driver);

        switch (creatingTripStep)
        {
            case 0:
                {
                    driver.CreatedTrip.From = msg.Text;
                    await botClient.SendMessage(
                        chat.Id,
                        "Введите точку назначения",
                        cancellationToken: cancellationToken);
                    creatingTripStep++;
                    return;
                }
            case 1:
                {
                    driver.CreatedTrip.To = msg.Text;
                    await botClient.SendMessage(
                        chat.Id,
                        "К какой паре вам надо ехать?",
                        cancellationToken: cancellationToken);
                    creatingTripStep++;
                    return;
                }
            case 2:
                {
                    if (!int.TryParse(msg.Text, out var pair))
                    {
                        await botClient.SendMessage(chat.Id, "Введите число");
                        return;
                    }

                    driver.CreatedTrip.ToPair = pair;
                    await botClient.SendMessage(
                        chat.Id,
                        "Сколько свободных мест в автомобиле?",
                        cancellationToken: cancellationToken);
                    creatingTripStep++;
                    return;
                }
            case 3:
                {
                    if (!int.TryParse(msg.Text, out var seats))
                    {
                        await botClient.SendMessage(chat.Id, "Введите число");
                        return;
                    }

                    if (seats <= 0 || seats >= 4)
                    {
                        await botClient.SendMessage(chat.Id, "Некорректное количество свободных мест");
                        return;
                    }

                    driver.CreatedTrip.NumberOfAvailableSeats = seats;
                    await botClient.SendMessage(
                        chat.Id,
                        "Какая стоимость поездки?",
                        cancellationToken: cancellationToken);
                    creatingTripStep++;
                    return;
                }
            case 4:
                {
                    if (!int.TryParse(msg.Text, out var cost))
                    {
                        await botClient.SendMessage(chat.Id, "Введите число");
                        return;
                    }

                    driver.CreatedTrip.Cost = cost;
                    await botClient.SendMessage(
                        chat.Id,
                        "Опишите расположение автомобиля",
                        cancellationToken: cancellationToken);
                    creatingTripStep++;
                    return;
                }
            case 5:
                {
                    driver.CreatedTrip.CarPosition = msg.Text;
                    await botClient.SendMessage(
                        chat.Id,
                        driver.CreatedTrip.ToString(),
                        cancellationToken: cancellationToken);
                    driver.EndCreatingTrip();
                    Program.isBusy = false;
                    creatingTripStep = 0;
                    Program.StartWithStandardUpdateHandler();
                    await MessageHandler.PrintDriverMenu(botClient, chat, msg.From.Id);
                    return;
                }
        }
    }
}
