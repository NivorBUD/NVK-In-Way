﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TGBotNVK.WebApiClient;
using TGBotNVK.WebApiClient.Dtos.General.ReqDtos;
using TGBotNVK.WebApiClient.Dtos.Passenger.ResDtos;

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
        var response = await apiClient.GetPassengerProfileAsync(chat.Id, "1.0");
        if (response.IsSuccess)
        {
            var data = response.Data;
            var inlineKeyboard = new InlineKeyboardMarkup(
                new List<InlineKeyboardButton[]>()
                {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData($"Назад", "pas_button"),
                    },
                });
            await botClient.SendTextMessageAsync(chat.Id,
                CreatePassengerCard(data, update.CallbackQuery.From.Username), replyMarkup: inlineKeyboard);
        }
        else
        {
            await botClient.SendTextMessageAsync(chat.Id, "Произошла ошибка при получении данных");
        }
    }

    private static string CreatePassengerCard(PassengerShortResDto passengerShortResDto, string account)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"👤 Пользователь: {account}");
        sb.AppendLine($"⭐️ Рейтинг: {(passengerShortResDto.Rating != null ? passengerShortResDto.Rating : 0)}");
        sb.AppendLine($"🚗 Поездок: {passengerShortResDto.TripsCount}");

        return sb.ToString();
    }

    public static async Task RecortToTrip(ITelegramBotClient botClient, Chat chat, Guid tripId)
    {
        var getTrip = await apiClient.GetTripInfoAsync(tripId, "1.0");
        if (!getTrip.IsSuccess)
        {
            await botClient.SendTextMessageAsync(chat.Id, $"Ошибка получения информации о поездке: {getTrip.ErrorText}");
            return;
        }
        var body = new RecordReqDto
        {
            TripId = tripId,
            DriverId = getTrip.Data.DriverId,
            PassengerId = chat.Id
        };
        var recordResponse = await apiClient.RecordToTripAsync("1.0", body);

        if (recordResponse.IsSuccess)
        {
            await botClient.SendTextMessageAsync(chat.Id, "Вы успешно записались на поездку");
        }
        else
        {
            await botClient.SendTextMessageAsync(chat.Id, $"Ошибка записи на поездку: {recordResponse.ErrorText}");
        }
    }
}
