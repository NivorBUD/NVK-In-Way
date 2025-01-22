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

public static class MessageHandler
{
    public static async void ProcessMessage(Message msg, ITelegramBotClient botClient)
    {
        var user = msg.From;
        var chat = msg.Chat;

        switch (msg.Type)
        {
            case MessageType.Text:
                {
                    if (msg.Text == "/start")
                    {
                        await PrintStartMenu(botClient, chat);
                        return;
                    }
                    return;
                }
            default:
                {
                    await botClient.SendMessage(
                        chat.Id,
                        "Используй только текст!");
                    return;
                }
        }
    }

    public static async Task PrintStartMenu(ITelegramBotClient botClient, Chat chat)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(
            new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Водитель", "driver_button"), 
                    InlineKeyboardButton.WithCallbackData("Пассажир", "pas_button"), 
                },
            });
                                
        await botClient.SendTextMessageAsync(chat.Id, "Кто вы", replyMarkup: inlineKeyboard);
    }

    public static async Task PrintDriverMenu(ITelegramBotClient botClient, Chat chat, long userId)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(
            new List<InlineKeyboardButton[]>()
                {
                    new InlineKeyboardButton[]
                    {
                        Program.DriversDataBase.ContainsKey(userId) ?
                            InlineKeyboardButton.WithCallbackData("Редактировать профиль", "change profile") :
                            InlineKeyboardButton.WithCallbackData("Создать профиль", "create profile")
                    },
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Посмотреть список созданных поездок", "check trips")
                    },
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Создать поездку", "create trip")
                    }
                });

        await botClient.SendMessage(
            chat.Id, "Выберите действие",
            replyMarkup: inlineKeyboard);
    }

    public static async void PrintPassengerMenu(ITelegramBotClient botClient, Chat chat)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(
            new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Посмотреть список активных поездок", "view_active"), 
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Посмотреть карточку", "view_info"), 
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Создание комнаты объединения в таксу", "create_room"), 
                },
            });
         await botClient.SendTextMessageAsync(chat.Id, "Чем я могу вам помочь?", replyMarkup: inlineKeyboard);
    }

    public static async void CheckStartChoice(ITelegramBotClient botClient, Update update)
    {
        if (Program.isBusy) return;

        var callbackQuery = update.CallbackQuery;
        var user = callbackQuery.From;
        var chat = callbackQuery.Message.Chat;
        if (callbackQuery.Data.StartsWith("moreInf_"))
        {
            await botClient.AnswerCallbackQuery(callbackQuery.Id);
            var tripId = int.Parse(callbackQuery.Data.Split('_')[1]);
            var trip = ActiveTrip.trips[tripId-1];
            ActiveTrip.ViewTrip(botClient, chat, trip);
            return;
        }
        switch (callbackQuery.Data)
        {
            case "pas_button":
                {
                    await botClient.AnswerCallbackQuery(callbackQuery.Id);
                    PrintPassengerMenu(botClient, chat);
                    return;
                }
            case "driver_button":
                {
                    await botClient.AnswerCallbackQuery(callbackQuery.Id);
                    await PrintDriverMenu(botClient, chat, user.Id);
                    return;
                }
            case "view_info":
                {
                    await botClient.AnswerCallbackQuery(callbackQuery.Id);
                    PassengerInfo.ShowPassangerCard(botClient, chat, update);
                    return;
                }
            case "view_active":
                {
                    await botClient.AnswerCallbackQuery(callbackQuery.Id);
                    ActiveTrip.ViewActiveTrip(botClient, chat);
                    return;
                }
            case "create profile":
                {
                    await botClient.AnswerCallbackQuery(callbackQuery.Id);
                    Program.isBusy = true;
                    ProfileHandler.StartCreatingDriverProfile(callbackQuery.Message, botClient);
                    return;
                }
            case "check trips":
                {
                    await botClient.AnswerCallbackQuery(callbackQuery.Id);
                    var trips = ProfileHandler.GetDriverTrips(user.Id);
                    if (trips != "")
                    {
                        await botClient.SendMessage(chat.Id, trips);
                    }
                    else
                    {
                        await botClient.SendMessage(chat.Id, "У вас нет созданных поездок");
                    }
                    return;
                }
            case "create trip":
                {
                    await botClient.AnswerCallbackQuery(callbackQuery.Id);
                    ProfileHandler.CreateTrip(callbackQuery.Message, botClient);
                    return;
                }
            case "change profile":
                {
                    return;
                }
        }
        return;
    }
}
