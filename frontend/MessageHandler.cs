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
using TGBotNVK.WebApiClient.Dtos.Passenger.ReqDtos;

namespace TGBotNVK;

public static class MessageHandler
{
    private static ApiClient apiClient = new ApiClient(new HttpClient());

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

        await botClient.SendMessage(chat.Id, "Кто вы", replyMarkup: inlineKeyboard);
    }

    public static async Task PrintDriverMenu(ITelegramBotClient botClient, Chat chat, long userId)
    {
        var isDriverInDataBase = Program.IsDriverIdInDataBase(userId);

        var buttons = new List<InlineKeyboardButton[]>(){
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData(
                    isDriverInDataBase ? "Редактировать профиль" : "Создать профиль", 
                    "create driver profile")
            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Сменить профиль", "change profile")
            }
        };
        if (isDriverInDataBase)
        {
            buttons.Add(new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Посмотреть список созданных поездок", "check trips")
                });
            buttons.Add(new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Создать поездку", "create trip")
                });
        }

        var inlineKeyboard = new InlineKeyboardMarkup(buttons);

        await botClient.SendMessage(
            chat.Id, "Выберите действие",
            replyMarkup: inlineKeyboard);
    }

    public static async void PrintPassengerMenu(ITelegramBotClient botClient, Chat chat)
    {
        var dataToPost = new { tgProfileId = chat.Id };
        var getPassenger = await apiClient.GetPassengerProfileAsync(chat.Id, "1.0");
        if (!getPassenger.IsSuccess)
        {
            var createPassenger = await apiClient.CreatePassengerProfileAsync(
                "1.0", new PassengerShortProfileReqDto { TgProfileId = chat.Id });

            if (!createPassenger.IsSuccess)
            {
                await botClient.SendMessage(chat.Id, "Произошла ошибка при создании профиля");
                return;
            }
        }
        
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
                    InlineKeyboardButton.WithCallbackData("Сменить профиль", "change profile")
                }
                /*new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Создание комнаты объединения в таксу", "create_room"),
                },*/
            });
        await botClient.SendMessage(chat.Id, "Чем я могу вам помочь?", replyMarkup: inlineKeyboard);
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
            case "create driver profile":
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
                    await botClient.SendMessage(chat.Id, trips != "" ? trips : "У вас нет созданных поездок");
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
                    await botClient.AnswerCallbackQuery(callbackQuery.Id);
                    await PrintStartMenu(botClient, chat);
                    return;
                }
        }
        return;
    }


}
