using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using static Telegram.Bot.TelegramBotClient;
using Telegram.Bot.Polling;

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
                    return;
                }
        }
    }
}


