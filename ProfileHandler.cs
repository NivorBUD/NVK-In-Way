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
        if (!Program.DriversDataBase.TryGetValue(msg.Chat.Id, out var driver))
        {
            driver = new Driver();
            Program.DriversDataBase[msg.Chat.Id] = driver;
        }

        if (driver.IsProfileComplete)
        {
            await botClient.SendMessage(msg.Chat.Id, "Ваш профиль водителя заполнен");
            Program.isBusy = false;
            return;
        }

        await botClient.SendMessage(
                        msg.Chat.Id,
                        "Введите марку авто");

        Program.StartBotWithAnotherUpdateHandler(CreateDriverProfile);
    }

    public static async void StartChangingDriverProfile(Message msg, ITelegramBotClient botClient)
    {
        if (!Program.DriversDataBase.TryGetValue(msg.Chat.Id, out var driver))
        {
            driver = new Driver();
            Program.DriversDataBase[msg.Chat.Id] = driver;
        }

        await botClient.SendMessage(
                        msg.Chat.Id,
                        "Введите марку авто");

        Program.StartBotWithAnotherUpdateHandler(CreateDriverProfile);
    }

    private static async Task CreateDriverProfile(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var msg = update.Message;
        var chat = msg.Chat;
        var driver = Program.DriversDataBase[msg.From.Id];

        switch (creatingProfileStep)
        {
            case 0:
                {
                    driver.SetAutoName(msg.Text);
                    driver.SetTGId(msg.From.Id);
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
        var driver = Program.DriversDataBase[driverTGId];

        return driver.GetTrips();
    }

    public static async void CreateTrip(Message msg, ITelegramBotClient botClient)
    {
        if (!Program.DriversDataBase.TryGetValue(msg.Chat.Id, out var driver))
        {
            driver = new Driver();
            Program.DriversDataBase[msg.Chat.Id] = driver;
        }

        if (!driver.IsProfileComplete)
        {
            await botClient.SendMessage(msg.Chat.Id, "Ваш профиль водителя не заполнен, заполните его");
            Program.isBusy = false;
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
        var driver = Program.DriversDataBase[msg.From.Id];
        driver.CreatedTrip = driver.CreatedTrip == null ? new Trip(driver) : driver.CreatedTrip;

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
                        "Опишите располложение автомобиля",
                        cancellationToken: cancellationToken);
                    creatingTripStep++;
                    return;
                }
            case 5:
                {
                    driver.CreatedTrip.CarPosition = msg.Text;
                    driver.EndCreatingTrip();
                    Program.isBusy = false;
                    creatingTripStep = 0;
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
}
