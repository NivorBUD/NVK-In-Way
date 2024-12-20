using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

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

public static class ActiveTrip
{
    public static async void ViewActiveTrip(ITelegramBotClient botClient, Chat chat)
    {
        // Тестовый словарь, нужно поменять на все доступные поездки из БД
        var active = new Dictionary<int, TripInfo>();
        active[0] = new TripInfo("NVK - GUK", "1 - 2", 200, 3);
        active[1] = new TripInfo("GUK - NVK", "1", 300, 1);

        for (int i = 0; i < active.Count; i++)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(
                new List<InlineKeyboardButton[]>()
                {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Подробнее", "moreInf"), 
                    },
                });
            await botClient.SendTextMessageAsync(chat.Id, 
                $"Активные поездки: {i+1}\nОткуда - Куда: {active[i].PlaceStartEnd}\nВремя: {active[i].TimeStartEnd}\nЦена: {active[i].Price}\nКолличество свободных мест: {active[i].Free}", 
                 replyMarkup: inlineKeyboard);
        }
    }
}