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
        //var isDriverInDataBase = Program.IsDriverIdInDataBase(userId);
        var buttons = new List<InlineKeyboardButton[]>();
        var driver = await apiClient.GetDriverProfileAsync(chat.Id, "1.0");
        if (!driver.IsSuccess)
        {
            buttons = new List<InlineKeyboardButton[]>(){
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Создать профиль", "create driver profile")
            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Сменить профиль", "change profile")
            }
            };
        }     
        else
        {
            buttons = new List<InlineKeyboardButton[]>(){
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Редактировать профиль", "recreate driver profile")
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Посмотреть карточку", "driver_view_info"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Посмотреть список созданных поездок", "check trips")
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Создать поездку", "create_trip")
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Сменить профиль", "change profile")
                } };
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
                    InlineKeyboardButton.WithCallbackData("Посмотреть список активных поездок", "view_active_pass"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Посмотреть карточку", "passenger_view_info"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Сменить профиль", "change profile")
                }
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
            await MoreInfoQuery(botClient, callbackQuery, chat);
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
            case "dri_button":
            {
                await botClient.AnswerCallbackQuery(callbackQuery.Id);
                PrintDriverMenu(botClient, chat, chat.Id);
                return;
            }
            case "driver_button":
                {
                    await botClient.AnswerCallbackQuery(callbackQuery.Id);
                    await PrintDriverMenu(botClient, chat, user.Id);
                    return;
                }
            case "passenger_view_info":
                {
                    await botClient.AnswerCallbackQuery(callbackQuery.Id);
                    PassengerInfo.ShowPassangerCard(botClient, chat, update);
                    return;
                }
            case "driver_view_info":
            {
                await botClient.AnswerCallbackQuery(callbackQuery.Id);
                DriverCardInfo.ShowDriverCard(botClient, chat, update);
                return;
            }
            case "view_active_pass":
                {
                    await botClient.AnswerCallbackQuery(callbackQuery.Id);
                    ActiveTrip.ViewPassengerActiveTrip(botClient, chat);
                    return;
                }
            case "view_active_driver":
                {
                    await botClient.AnswerCallbackQuery(callbackQuery.Id);
                    ActiveTrip.ViewDriverActiveTrip(botClient, chat);
                    return;
                }
            case "create driver profile":
                {
                    await botClient.AnswerCallbackQuery(callbackQuery.Id);
                    Program.isBusy = true;
                    ProfileHandler.StartCreatingDriverProfile(callbackQuery.Message, botClient);
                    return;
                }
            case "recreate driver profile":
                {
                    await botClient.AnswerCallbackQuery(callbackQuery.Id);
                    Program.isBusy = true;
                    ProfileHandler.StartCreatingDriverProfile(callbackQuery.Message, botClient);
                    return;
                }
            case "check trips":
                {
                    await botClient.AnswerCallbackQuery(callbackQuery.Id);
                    //var trips = ProfileHandler.GetDriverTrips(user.Id);
                    await botClient.SendMessage(chat.Id, /*trips != "" ? trips :*/ "У вас нет созданных поездок");
                    return;
                }
            case "create_trip":
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

    private static async Task MoreInfoQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery, Chat chat)
    {
        await botClient.AnswerCallbackQuery(callbackQuery.Id);
        if (!Guid.TryParse(callbackQuery.Data.Split('_')[1], out var tripId))
        {
            await botClient.SendMessage(chat.Id, "Произошла ошибка при получении идентификатора поездки");
            return;
        }

        ActiveTrip.ViewTrip(botClient, chat, tripId);
    }
}
