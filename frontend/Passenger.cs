using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TGBotNVK.WebApiClient;

namespace TGBotNVK;
public class Passenger
{
    public int TripCount { get; set; }
    public double Rating { get; set; }
    public string Profile { get; set; }

    public Passenger () { }
    public Passenger(int count, double rating, string prof)
    {
        TripCount = count;
        Rating = rating;
        Profile = prof;
    }
}

public static class PassengerInfo
{
    private static ApiClient apiClient = new ApiClient(new HttpClient());
    public static async void ShowPassangerCard(ITelegramBotClient botClient, Chat chat, Update update)
    {
        var data = await apiClient.GetPassengerProfileAsync(chat.Id, "1.0");
        
        var passenger = new Passenger(0, 0, update.CallbackQuery.From.Username);
        var inlineKeyboard = new InlineKeyboardMarkup(
            new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData($"Профиль ТГ: {passenger.Profile}", "profile"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData($"Рейтинг: {(data.Rating != null ? data.Rating : 0)}"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData($"Число совершенных поездок: {data.TripsCount}"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData($"Назад", "pas_button"),
                },
            });
        await botClient.SendTextMessageAsync(chat.Id, "Карточка пользователя", replyMarkup: inlineKeyboard);
    }
}
