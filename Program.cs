using static Telegram.Bot.TelegramBotClient;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

class Passenger
{
	public int TripCount { get; set; }
	public double Rating { get; set; }
	public string Profile { get; set; }

	public Passenger(int count, double rating, string prof)
	{
		TripCount = count;
		Rating = rating;
		Profile = prof;
	}
}

class TripInfo
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

class Program
{
    //public static Dictionary<int, Passenger> Passengers = new Dictionary<int, Passenger>();
    // Это клиент для работы с Telegram Bot API, который позволяет отправлять сообщения, управлять ботом, подписываться на обновления и многое другое.
    private static ITelegramBotClient _botClient;

    // Это объект с настройками работы бота. Здесь мы будем указывать, какие типы Update мы будем получать, Timeout бота и так далее.
    private static ReceiverOptions _receiverOptions;

    static async Task Main()
    {

        _botClient = new TelegramBotClient("8026874216:AAEOlGPMXT_IjX4GvctMpFm044q8tB2C2fc"); // Присваиваем нашей переменной значение, в параметре передаем Token, полученный от BotFather
        _receiverOptions = new ReceiverOptions // Также присваем значение настройкам бота
        {
            AllowedUpdates = new[] // Тут указываем типы получаемых Update`ов, о них подробнее расказано тут https://core.telegram.org/bots/api#update
            {
                UpdateType.Message, // Сообщения (текст, фото/видео, голосовые/видео сообщения и т.д.)
                UpdateType.CallbackQuery, // Inline кнопки
            },
            // Параметр, отвечающий за обработку сообщений, пришедших за то время, когда ваш бот был оффлайн
            // True - не обрабатывать, False (стоит по умолчанию) - обрабаывать
            //ThrowPendingUpdates = true,
        };

        using var cts = new CancellationTokenSource();

        // UpdateHander - обработчик приходящих Update`ов
        // ErrorHandler - обработчик ошибок, связанных с Bot API
        _botClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token); // Запускаем бота

        var me = await _botClient.GetMeAsync(); // Создаем переменную, в которую помещаем информацию о нашем боте.
        Console.WriteLine($"{me.FirstName} запущен!");

        await Task.Delay(-1); // Устанавливаем бесконечную задержку, чтобы наш бот работал постоянно
    }

    private static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        // Обязательно ставим блок try-catch, чтобы наш бот не "падал" в случае каких-либо ошибок
        try
        {
            // Сразу же ставим конструкцию switch, чтобы обрабатывать приходящие Update
            switch (update.Type)
            {
                case UpdateType.Message:
                {
                        // эта переменная будет содержать в себе все связанное с сообщениями
                    var message = update.Message;

                        // From - это от кого пришло сообщение
                    var user = message.From;
                    /*if (!Passengers.Keys.Contains(user.Id))
                    {
                        Passengers[user.Id] = new Passenger (0, 0, user.Username);
                    }*/

                        // Выводим на экран то, что пишут нашему боту, а также небольшую информацию об отправителе
                    Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");

                        // Chat - содержит всю информацию о чате
                    var chat = message.Chat;

                        // Добавляем проверку на тип Message
                    switch (message.Type)
                    {
                         // Тут понятно, текстовый тип
                        case MessageType.Text:
                        {
                            if (message.Text == "/start")
                            {
                                var inlineKeyboard = new InlineKeyboardMarkup(
                                new List<InlineKeyboardButton[]>() // здесь создаем лист (массив), который содрежит в себе массив из класса кнопок
                                {
                                            // Каждый новый массив - это дополнительные строки,
                                            // а каждая дополнительная строка (кнопка) в массиве - это добавление ряда
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Водитель", "driver_button"), 
                                    InlineKeyboardButton.WithCallbackData("Пассажир", "pas_button"), 
                                },
                                });
                                
                                await botClient.SendTextMessageAsync(chat.Id, "Кто вы", replyMarkup: inlineKeyboard); // Все клавиатуры передаются в параметр replyMarkup
                                return;
                            }                                  
                            return;
                        }

                            // Добавил default , чтобы показать вам разницу типов Message
                        default:
                        {
                            await botClient.SendTextMessageAsync(chat.Id, "Используй только текст!");
                            return;
                        }
                    }
                }
                case UpdateType.CallbackQuery:
                {
                    // Переменная, которая будет содержать в себе всю информацию о кнопке, которую нажали
                    var callbackQuery = update.CallbackQuery;              
                    var user = callbackQuery.From;
                    
                    var passenger =  new Passenger(0, 0, user.Username);
                    var active = new Dictionary<int, TripInfo>();
                        active[0] = new TripInfo("NVK - GUK", "1 - 2", 200, 3);
                        active[1] = new TripInfo("GUK - NVK", "1", 300, 1);

                    Console.WriteLine($"{user.FirstName} ({user.Id}) нажал на кнопку: {callbackQuery.Data}");
                    var chat = callbackQuery.Message.Chat; 
                    
                    // Добавляем блок switch для проверки кнопок
                    switch (callbackQuery.Data)
                    {
                        case "pas_button":
                        {
                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
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
                                        InlineKeyboardButton.WithCallbackData("Поиск свободной машины", "find_car"), 
                                    },
                                    new InlineKeyboardButton[]
                                    {
                                        InlineKeyboardButton.WithCallbackData("Создание комнаты объединения в таксу", "create_room"), 
                                    },
                                });
                            await botClient.SendTextMessageAsync(chat.Id, "Чем я могу вам помочь?", replyMarkup: inlineKeyboard);
                            return;
                        }
                        case "view_active":
                        {
                            // В этом типе клавиатуры обязательно нужно использовать следующий метод
                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                            for (int i = 0; i < active.Count; i++)
                            {
                                
                                var inlineKeyboard = new InlineKeyboardMarkup(
                                    new List<InlineKeyboardButton[]>() // здесь создаем лист (массив), который содрежит в себе массив из класса кнопок
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
                            /*var inlineKeyboard = new InlineKeyboardMarkup(
                                new List<InlineKeyboardButton[]>() // здесь создаем лист (массив), который содрежит в себе массив из класса кнопок
                                {
                                    new InlineKeyboardButton[]
                                    {
                                        InlineKeyboardButton.WithCallbackData("Точка старта - точка прибытия"), 
                                    },
                                    new InlineKeyboardButton[]
                                    {
                                        InlineKeyboardButton.WithCallbackData($"Время старта - время прибытия"), 
                                    },
                                    new InlineKeyboardButton[]
                                    {
                                        InlineKeyboardButton.WithCallbackData($"Цена"), 
                                    },
                                    new InlineKeyboardButton[]
                                    {
                                        InlineKeyboardButton.WithCallbackData($"Колличество свободных мест"), 
                                    },

                                });
                            await botClient.SendTextMessageAsync(chat.Id, "Список активных поездок", replyMarkup: inlineKeyboard);*/
                            return;
                        }
                        case "view_info":
                        {             
                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                            var inlineKeyboard = new InlineKeyboardMarkup(
                                new List<InlineKeyboardButton[]>()
                                {
                                    new InlineKeyboardButton[]
                                    {
                                        InlineKeyboardButton.WithCallbackData("Редактировать", "change"), 
                                    },
                                    new InlineKeyboardButton[]
                                    {
                                        InlineKeyboardButton.WithCallbackData($"Рейтинг: {passenger.Rating}", "rating"), 
                                    },
                                    new InlineKeyboardButton[]
                                    {
                                        InlineKeyboardButton.WithCallbackData($"Профиль ТГ: {passenger.Profile}", "profile"), 
                                    },
                                    new InlineKeyboardButton[]
                                    {
                                        InlineKeyboardButton.WithCallbackData($"Число совершенных поездок: {passenger.TripCount}", "trip_count"), 
                                    },
                                    new InlineKeyboardButton[]
                                    {
                                        InlineKeyboardButton.WithCallbackData($"Назад", "pas_button"), 
                                    },
                                });
                            await botClient.SendTextMessageAsync(chat.Id, "Карточка пользователя", replyMarkup: inlineKeyboard);
                            return;
                        }
                        case "change":
                        {         
                            var inlineKeyboard = new InlineKeyboardMarkup(
                                new List<InlineKeyboardButton[]>()
                                {
                                    new InlineKeyboardButton[]
                                    {
                                        InlineKeyboardButton.WithCallbackData("Вернуться в профиль", "view_info"), 
                                    },
                                });
                            await botClient.SendTextMessageAsync(chat.Id, "Введите новый профиль ТГ", replyMarkup: inlineKeyboard);
                            //if (update.Message != null)
                            //passenger.Profile = update.Message.Text;
                            return;
                        }
                    }                    
                    return;
                }
                return;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
    {
        // Тут создадим переменную, в которую поместим код ошибки и её сообщение 
        var ErrorMessage = error switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => error.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
}
