using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TGBotNVK.WebApiClient;
using TGBotNVK.WebApiClient.Dtos.CarTrip.ReqDtos;
using TGBotNVK.WebApiClient.Dtos.CarTrip.ResDtos;
using TGBotNVK.WebApiClient.Dtos.General.ReqDtos;
using static System.Net.WebRequestMethods;
using File = System.IO.File;
using System.Drawing;
using System.Xml.Linq;
using TGBotNVK.WebApiClient.Dtos.Driver.ReqDtos;

namespace TGBotNVK;
public class TripInfo
{
    public string PlaceStartEnd;
    public string TimeStartEnd;
    public double Price;
    public int Free;

    public TripInfo(string place, string time, double price, int free)
    {
        PlaceStartEnd = place;
        TimeStartEnd = time;
        Price = price;
        Free = free;
    }
}

public class SearchTrip
{
    private static ApiClient apiClient = new(new HttpClient());

    private static IntervalSearchReqDto _intervalSearch = null;
    private static int creatingSearchsStep;

    public static async void ViewSearchingTrips(ITelegramBotClient botClient, Chat chat)
    {
        Program.StartBotWithAnotherUpdateHandler(IntervalSearch);
        await botClient.SendTextMessageAsync(chat.Id, "Введите место отправления");

    }

    private static async Task IntervalSearch(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        var msg = update.Message;
        var chat = msg.Chat;

        _intervalSearch ??= new IntervalSearchReqDto();

        switch (creatingSearchsStep)
        {
            case 0:
                {
                    _intervalSearch.StartPointAddress = new LocationReqDto()
                    {
                        Coordinate = null,
                        TextDescription = msg.Text
                    };
                    await botClient.SendMessage(
                        chat.Id,
                        "Введите место прибытия",
                        cancellationToken: cancellationToken);
                    creatingSearchsStep++;
                    return;
                }
            case 1:
                {
                    _intervalSearch.EndPointAddress = new LocationReqDto()
                    {
                        Coordinate = null,
                        TextDescription = msg.Text
                    };
                    await botClient.SendMessage(
                        chat.Id,
                        "Введите максимальное время прибытия(если не желаете - N)",
                        cancellationToken: cancellationToken);
                    creatingSearchsStep++;
                    return;
                }
            case 2:
                {
                    if (msg.Text.ToUpper() == "N")
                    {
                        _intervalSearch.MaxEndTime = null;
                    }
                    else {
                        if (!DateTimeOffset.TryParse(msg.Text, out var timeOffset))
                        {
                            await botClient.SendMessage(chat.Id, "Некорректный формат даты, нужен DateTimeOffset");
                            return;
                        }
                        _intervalSearch.MaxEndTime = timeOffset;
                    }

                    await botClient.SendMessage(
                        chat.Id,
                        "Введите минимальное время прибытия(если не желаете - N)",
                        cancellationToken: cancellationToken);
                    creatingSearchsStep++;
                    return;
                }
            case 3:
                {
                    if (msg.Text.ToUpper() == "N")
                    {
                        _intervalSearch.MaxEndTime = null;
                    }
                    else
                    {
                        if (!DateTimeOffset.TryParse(msg.Text, out var timeOffset))
                        {
                            await botClient.SendMessage(chat.Id, "Некорректный формат даты, нужен DateTimeOffset");
                            return;
                        }
                        _intervalSearch.MinDateTime = timeOffset;
                    }

                    Program.isBusy = false;
                    creatingSearchsStep = 0;

                    var searchResponse = await apiClient.SearchTripsAsync(0, 5, "1.0",
                        _intervalSearch, cancellationToken);
                    if (!searchResponse.IsSuccess)
                    {
                        await botClient.SendMessage(chat.Id, $"Произошла ошибка при поиске поездок: {searchResponse.ErrorText}");
                        return;
                    }

                    foreach (var shortActiveTrip in searchResponse.Data)
                    {
                        await WriteActiveTripsResDto(botClient, chat, shortActiveTrip);
                    }
                    Program.StartWithStandardUpdateHandler();
                    return;
                }
        }
    }

    private static async Task WriteActiveTripsResDto(ITelegramBotClient botClient, Chat chat, ShortActiveTripResDto resDto)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(
            new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Поеду", $"takeIt_{resDto.Id}"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Подробнее", $"moreInf_{resDto.Id}"),
                },
            });
        await botClient.SendTextMessageAsync(chat.Id, GetTripShortInfoString(resDto), replyMarkup: inlineKeyboard);
    }

    private static string GetTripShortInfoString(ShortActiveTripResDto resDto)
    {
        var sb = new StringBuilder();
        sb.AppendLine("=== Информация о поездке ===");
        sb.AppendLine();
        sb.AppendLine($"🗺️  Откуда - Куда:    {resDto.StartPoint.TextDescription} - {resDto.EndPoint.TextDescription}");
        sb.AppendLine($"🕒  Время старта:     {resDto.TripStartTime}");
        sb.AppendLine();
        sb.AppendLine("=============================");
        sb.AppendLine();

        return sb.ToString();
    }
}

public class ActiveTrip
{
    private static ApiClient apiClient = new(new HttpClient());
    
    public static List<Trip> trips; 

    public static async void ViewPassengerActiveTrip(ITelegramBotClient botClient, Chat chat)
    {
        var getActiveTrips = await apiClient
            .ActivePassengerTripsAsync(chat.Id, 0, 5, "1.0");

        if (!getActiveTrips.IsSuccess)
        {
            await botClient.SendTextMessageAsync(chat.Id, $"Ошибка получения активных поездок: {getActiveTrips.ErrorText}");
            return;
        }
        if(getActiveTrips.Data.Count == 0)
        {
            await botClient.SendTextMessageAsync(chat.Id, "На данный момент нет активных поездок");
            return;
        }

        foreach (var activeTrip in getActiveTrips.Data)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(
            new List<InlineKeyboardButton[]>()
            {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Подробнее", $"moreInf_{activeTrip.Id}"),
                    },
            });
            await botClient.SendTextMessageAsync(chat.Id, GetTripShortInfoString(activeTrip), replyMarkup: inlineKeyboard);
        }
    }

    public static async void ViewDriverActiveTrip(ITelegramBotClient botClient, Chat chat)
    {
        var getActiveTrips = await apiClient
            .ActiveDriverTripsAsync(chat.Id, 0, 5, "1.0");

        if (!getActiveTrips.IsSuccess)
        {
            await botClient.SendTextMessageAsync(chat.Id, $"Ошибка получения активных поездок: {getActiveTrips.ErrorText}");
            return;
        }
        if (getActiveTrips.Data.Count == 0)
        {
            await botClient.SendTextMessageAsync(chat.Id, "На данный момент нет активных поездок");
            return;
        }

        foreach (var activeTrip in getActiveTrips.Data)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(
                new List<InlineKeyboardButton[]>()
                {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Подробнее", $"moreInf_{activeTrip.Id}"),
                    },
                });
            await botClient.SendTextMessageAsync(chat.Id, GetTripShortInfoString(activeTrip), replyMarkup: inlineKeyboard);
        }
    }

    public static async Task ViewTrip(ITelegramBotClient botClient, Chat chat, Guid tripId)
    {
        var getTrip = await apiClient.GetTripInfoAsync(tripId, "1.0");
        if (!getTrip.IsSuccess)
        {
            await botClient.SendTextMessageAsync(chat.Id, $"Ошибка получения информации о поездке: {getTrip.ErrorText}");
            return;
        }

        await botClient.SendTextMessageAsync(chat.Id, GetTripDetailedInfoString(getTrip.Data));

        var image = InputFile.FromUri(new Uri($"http://138.124.20.138:5878/driver_cars/{getTrip.Data.TripCar.AutoId}"));
        
        var message = await botClient.SendPhoto(chat.Id, image);
    }

    private static string GetTripShortInfoString(GetActiveTripsResDto resDto)
    {
        var sb = new StringBuilder();
        sb.AppendLine("=== Информация о поездке ===");
        sb.AppendLine();
        sb.AppendLine($"🗺️  Откуда - Куда:    {resDto.StartPoint.TextDescription} - {resDto.EndPoint.TextDescription}");
        sb.AppendLine($"🕒  Время старта:     {resDto.TripStartTime}");
        sb.AppendLine($"💰  Цена:             {resDto.DriveCost} руб.");
        sb.AppendLine($"🚍  Свободные места:  {resDto.TotalPlaces - resDto.BookedPlaces}");
        sb.AppendLine($"📍   Место посадки:     {resDto.CarLocation}");
        sb.AppendLine($"🚗  Марка авто:       {resDto.TripCar.AutoName}");
        sb.AppendLine($"🎨  Цвет авто:        {resDto.TripCar.AutoColor}");
        sb.AppendLine($"🔢  Номер авто:       {resDto.TripCar.AutoNumber}");
        sb.AppendLine();
        sb.AppendLine("=============================");
        sb.AppendLine();

        return sb.ToString();
    }

    private static string GetTripDetailedInfoString(GetActiveTripsResDto resDto)
    {
        var sb = new StringBuilder();
        sb.AppendLine("=== Информация о поездке ===");
        sb.AppendLine();
        sb.AppendLine($"🗺️  Откуда - Куда:    {resDto.StartPoint.TextDescription} - {resDto.EndPoint.TextDescription}");
        sb.AppendLine($"🕒  Время старта:     {resDto.TripStartTime.DateTime}");
        sb.AppendLine($"🕒  Время конца:      {(resDto.TripEndTime == null ? "не указано" : resDto.TripEndTime.DateTime)}");
        sb.AppendLine($"💰  Цена:             {resDto.DriveCost} руб.");
        sb.AppendLine($"🚍  Всего мест:      {resDto.TotalPlaces}");
        sb.AppendLine($"🚍  Свободных места:  {resDto.TotalPlaces - resDto.BookedPlaces}");
        sb.AppendLine($"📍   Место посадки:     {resDto.CarLocation}");
        sb.AppendLine($"🚗  Марка авто:       {resDto.TripCar.AutoName}");
        sb.AppendLine($"🎨  Цвет авто:        {resDto.TripCar.AutoColor}");
        sb.AppendLine($"🔢  Номер авто:       {resDto.TripCar.AutoNumber}");
        sb.AppendLine();
        sb.AppendLine("=============================");
        sb.AppendLine();

        return sb.ToString();
    }
}