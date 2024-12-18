﻿using System;
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
                        await PrintStartMenu(botClient, chat, msg);
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

    public static async Task PrintStartMenu(ITelegramBotClient botClient, Chat chat, Message msg)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(
            new List<InlineKeyboardButton[]>()
                {
                    new InlineKeyboardButton[]
                    {
                        Program.DriversDataBase.ContainsKey(msg.From.Id) ?
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

    public static async void CheckStartChoice(ITelegramBotClient botClient, Update update)
    {
        if (Program.isBusy) return;

        var callbackQuery = update.CallbackQuery;
        var user = callbackQuery.From;
        var chat = callbackQuery.Message.Chat;

        switch (callbackQuery.Data)
        {
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