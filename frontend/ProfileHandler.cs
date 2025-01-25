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
using Telegram.Bot.Types.ReplyMarkups;
using TGBotNVK.WebApiClient;
using TGBotNVK.WebApiClient.Dtos.Driver.ReqDtos;
using TGBotNVK.WebApiClient.Dtos.General.ReqDtos;
using System.Linq.Expressions;
using TGBotNVK.WebApiClient.Dtos.CarTrip.ReqDtos;
using TGBotNVK.WebApiClient.Dtos.General.ResDtos;
using TGBotNVK.WebApiClient.Exceptions;

namespace TGBotNVK;
public static class ProfileHandler
{
    private static int creatingProfileStep;
    private static int creatingTripStep;

    public static async void StartCreatingDriverProfile(Message msg, ITelegramBotClient botClient)
    {
        //var driver = Program.GetDriverFromDatabse(msg.Chat.Id);
        //var driver = await apiClient.GetProfileAsync(msg.Chat.Id, "1.0");
        await botClient.SendMessage(msg.Chat.Id, "Введите марку авто");
        Program.StartBotWithAnotherUpdateHandler(CreateDriverProfile);
        /*if (!driver.IsSuccess)
        {
            await botClient.SendMessage(msg.Chat.Id, "Введите марку авто");
            Program.StartBotWithAnotherUpdateHandler(CreateDriverProfile);
        }
        else
        {
            await MessageHandler.PrintDriverMenu(botClient, msg.Chat, msg.From.Id);
        }*/
    }
    
    private static ApiClient apiClient = new ApiClient(new HttpClient());
    private static string name = "";
    private static string number = "";
    private static string color = "";

    public static async Task CreateDriverProfile(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var msg = update.Message;
        var chat = msg.Chat;
        var driver = await apiClient.GetDriverProfileAsync(msg.Chat.Id, "1.0", cancellationToken);

        switch (creatingProfileStep)
        {
            case 0:
                {
                    name = msg.Text;
                    await botClient.SendMessage(
                        chat.Id,
                        "Введите номер авто",
                        cancellationToken: cancellationToken);
                    creatingProfileStep++;
                    return;
                }
            case 1:
                {
                    number = msg.Text;
                    await botClient.SendMessage(
                        chat.Id,
                        "Введите цвет авто",
                        cancellationToken: cancellationToken);
                    creatingProfileStep++;
                    return;
                }
            case 2:
                {
                    color = msg.Text;
                    Program.isBusy = false;
                    creatingProfileStep = 0;
                    
                    if (driver.IsSuccess)
                    {
                        var test = driver.Data.Cars.ToArray()[0];
                        test.AutoNumber = number;
                        test.AutoName = name;
                        test.AutoColor = color;
                        var newCar = new DetailedСarReqDto { Id = test.AutoId, AutoName = name, AutoNumber = number, AutoColor = color };
                        var car = await apiClient.UpdateDriverCarsAsync(chat.Id, "1.0", new List<DetailedСarReqDto> { newCar });
                        //var car = await apiClient.UpdateDriverCarsAsync(chat.Id, "0.1", driver.Data.Cars);
                    }
                    else
                    {
                        var createDriver = await apiClient.CreateProfileAsync(
                        "1.0", new DriverProfileReqDto { Cars = new[] { new CarReqDto { AutoName = name, AutoNumber = number, AutoColor = color } }, TgProfileId = chat.Id });
                    }
                    Program.StartWithStandardUpdateHandler();
                    await MessageHandler.PrintDriverMenu(botClient, chat, msg.From.Id);
                    return;
                }
        }
    }

    private static CreateTripReqDto _tripKostil = null;

    public static async void CreateTrip(Message msg, ITelegramBotClient botClient)
    {
        var getDriver = await apiClient.GetDriverProfileAsync(msg.Chat.Id, "1.0");

        if (!getDriver.IsSuccess)
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

    private static async Task CreatingTrip(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        var msg = update.Message;
        var chat = msg.Chat;
        var getDriver = await apiClient.GetDriverProfileAsync(chat.Id, "1.0");

        if (!getDriver.IsSuccess)
        {
            await botClient.SendMessage(update.CallbackQuery.Id, "Ваш профиль водителя не заполнен, заполните его");
            Program.isBusy = false;
            await MessageHandler.PrintDriverMenu(botClient, msg.Chat, msg.From.Id);
            return;
        }
        var driverProfile = getDriver.Data;

        _tripKostil ??= new CreateTripReqDto
        {
            TripCar = new OnlyCarIdsReqDto()
            {
                Id = driverProfile.Cars.First().AutoId,
                DriverId = driverProfile.TgProfileId
            }
        };

        switch (creatingTripStep)
        {
            case 0:
                {
                    _tripKostil.StartPoint = new LocationReqDto()
                    {
                        TextDescription = msg.Text,
                        Coordinate = null
                    };
                    await botClient.SendMessage(
                        chat.Id,
                        "Введите точку прибытия",
                        cancellationToken: cancellationToken);
                    creatingTripStep++;
                    return;
                }
            case 1:
                {
                    _tripKostil.EndPoint = new LocationReqDto()
                    {
                        TextDescription = msg.Text,
                        Coordinate = null
                    };
                    await botClient.SendMessage(
                        chat.Id,
                        "Время начала поездки?",
                        cancellationToken: cancellationToken);
                    creatingTripStep++;
                    return;
                }
            case 2:
                {
                    if (!DateTimeOffset.TryParse(msg.Text, out var timeOffset))
                    {
                        await botClient.SendMessage(chat.Id, "Некорректный формат даты, нужен DateTimeOffset");
                        return;
                    }
                    _tripKostil.DriveStartTime = timeOffset;
                    await botClient.SendMessage(
                        chat.Id,
                        "Время конца поездки?",
                        cancellationToken: cancellationToken);
                    creatingTripStep++;
                    return;
                }
            case 3:
            {
                if (!DateTimeOffset.TryParse(msg.Text, out var timeOffset))
                {
                    await botClient.SendMessage(chat.Id, "Некорректный формат даты, нужен DateTimeOffset");
                    return;
                }
                _tripKostil.DriveEndTime = timeOffset;
                    await botClient.SendMessage(
                    chat.Id,
                    "Сколько свободных мест в автомобиле?",
                    cancellationToken: cancellationToken);
                creatingTripStep++;
                return;
            }
            case 4:
                {
                    if (!int.TryParse(msg.Text, out var seats))
                    {
                        await botClient.SendMessage(chat.Id, "Введите число");
                        return;
                    }

                    if (seats <= 0 || seats >= 10)
                    {
                        await botClient.SendMessage(chat.Id, "Некорректное количество свободных мест");
                        return;
                    }

                    _tripKostil.TotalPlaces = seats;
                    await botClient.SendMessage(
                        chat.Id,
                        "Какая стоимость поездки?",
                        cancellationToken: cancellationToken);
                    creatingTripStep++;
                    return;
                }
            case 5:
                {
                    if (!double.TryParse(msg.Text, out var cost))
                    {
                        await botClient.SendMessage(chat.Id, "Введите число");
                        return;
                    }

                    _tripKostil.TripCost = cost;
                    await botClient.SendMessage(
                        chat.Id,
                        "Опишите расположение автомобиля",
                        cancellationToken: cancellationToken);
                    creatingTripStep++;
                    return;
                }
            case 6:
                {
                    _tripKostil.CarLocation = msg.Text;
                    Program.isBusy = false;
                    creatingTripStep = 0;
                    var createResponse = await apiClient.CreateTripAsync("1.0", _tripKostil);
                    if (!createResponse.IsSuccess)
                    {
                        await botClient.SendMessage(chat.Id, $"Произошла ошибка при отправке поездки: {createResponse.ErrorText}");
                        return;
                    }
                    await botClient.SendMessage(chat.Id, GetCreateTripReqDtoString(_tripKostil));
                    await MessageHandler.PrintDriverMenu(botClient, chat, msg.From.Id);
                    return;
                }
        }
    }

    private static string GetCreateTripReqDtoString(CreateTripReqDto createDto)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Поездка успешно создана");
        sb.AppendLine(new string('-', 50));
        sb.AppendLine($"Откуда: {createDto.StartPoint.TextDescription}");
        sb.AppendLine($"Куда: {createDto.EndPoint.TextDescription}");
        sb.AppendLine($"Время начала: {createDto.DriveStartTime}");
        sb.AppendLine($"Время конца: {createDto.DriveEndTime}");
        sb.AppendLine($"Свободных мест: {createDto.TotalPlaces}");
        sb.AppendLine($"Стоимость поездки: {createDto.TripCost}");
        sb.AppendLine($"Место посадки: {createDto.CarLocation}");
        sb.AppendLine(new string('-', 50));

        return sb.ToString();
    }
}
