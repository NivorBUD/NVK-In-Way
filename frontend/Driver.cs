using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using TGBotNVK.WebApiClient;
using TGBotNVK.WebApiClient.Dtos.CarTrip.ReqDtos;
using TGBotNVK.WebApiClient.Dtos.General.ResDtos;
using System.Security.Principal;

namespace TGBotNVK;

public class Driver
{
    //private Car car;
    public Car car { get; private set; }
    private int rating;
    private int allTripsCount;

    public List<Trip> trips = new List<Trip>();
    public Trip CreatedTrip;
    public long TGId { get; private set; }

    public bool IsProfileComplete => car.Name != "" && car.Number != "" && car.Color != "" && TGId != null;

    public Driver(long id) 
    {
        car = new();
        TGId = id;
    }

    public void SetAutoName(string name)
    {
        car.Name = name;
    }

    public void SetAutoNumber(string number)
    {
        car.Number = number;
    }

    public void SetAutoColor(string color)
    {
        car.Color = color;
    }

    public void SetTGId(long id)
    {
        TGId = id;
    }

    public override string ToString()
    {
        return $"{car}, tg id: {TGId}";
    }

    public string GetAutoInfo()
    {
        return car.ToString();
    }

    public void EndCreatingTrip()
    {
        trips.Add(CreatedTrip);
        CreatedTrip = new(this);
    }

    public string GetTrips()
    {
        var result = new StringBuilder();

        foreach (var trip in trips)
        {
            result.Append(trip.ToString());
        }

        return result.ToString();
    }
}

public static class DriverCardInfo
{
    private static ApiClient apiClient = new ApiClient(new HttpClient());

    public static async void ShowDriverCard(ITelegramBotClient botClient, Chat chat, Update update)
    {
        var response = await apiClient.GetDriverProfileAsync(chat.Id, "1.0");
        if (response.IsSuccess)
        {
            var data = response.Data;
            var account = update.CallbackQuery.From.Username;
            var inlineKeyboard = new InlineKeyboardMarkup(
                new List<InlineKeyboardButton[]>()
                {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData($"Назад", "dri_button"),
                    },
                });
            await botClient.SendTextMessageAsync(chat.Id, CreateDriverCard(data, account), replyMarkup: inlineKeyboard);
        }
        else
        {
            await botClient.SendTextMessageAsync(chat.Id, $"Произошла ошибка при получении данных: {response.ErrorText}");
        }
    }

    private static string CreateDriverCard(DriverProfileResDto profileResDto, string account)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"👤 Пользователь: {account}");
        sb.AppendLine($"⭐️ Рейтинг: {(profileResDto.Rating != null ? profileResDto.Rating : 0)}");
        sb.AppendLine($"🚗 Поездок: {profileResDto.AllTripsCount}");
        sb.AppendLine("");
        sb.AppendLine("Ваши машины:");

        InsertCarsInfoString(profileResDto.Cars, sb);

        return sb.ToString();
    }

    private static void InsertCarsInfoString(ICollection<CarResDto> carsResDtos, StringBuilder insertSb)
    {
        if (carsResDtos.Count > 0)
        {
            foreach (var carResDto in carsResDtos)
            {
                insertSb.AppendLine(new string('-', 40));
                insertSb.AppendLine($"🚘 Название:  {carResDto.AutoName}");
                insertSb.AppendLine($"🔢 Номер:       {carResDto.AutoNumber}");
                insertSb.AppendLine($"🎨 Цвет:           {carResDto.AutoColor}");
                insertSb.AppendLine(new string('-', 40));
            }
        }
        else
        {
            insertSb.AppendLine("🚫 У вас нет машин");
        }
    }
}
