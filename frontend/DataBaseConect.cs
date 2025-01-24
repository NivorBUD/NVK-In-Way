using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Telegram.Bot.Args;

namespace TGBotNVK;

public class DataBaseConect
{
    private static readonly HttpClient client = new HttpClient();
    private static readonly string API_BASE_URL = "http://138.124.20.138:5878";

    public static async Task<string> GetDataFromApi(string endpoint)
    {
        HttpResponseMessage response = client.GetAsync(API_BASE_URL + endpoint).Result;

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Данные успешно получены из базы данных:");
            return jsonResponse;
        }
        else
        {
            Console.WriteLine(response.StatusCode.ToString());
            return "Ошибка получения данных: " + response.StatusCode;
        }
    }

    public static Dictionary<string, string> GetResultDictionary(string response)
    {
        var result = new Dictionary<string, string>();
        var res = response.Substring(2, response.Length - 3).Replace(":", string.Empty).Replace(",", string.Empty);
        var array = res.Split("\"");
        for (int i = 0; i < array.Length; i += 2)
        {
            result[array[i]] = array[i + 1];
        }
        return result;
    }

    public static async Task<string> PostDataToApi(string endpoint, object message)
    {
        var dataToPost = new { tgProfileId = message }; // Пример содержания, измените под ваши нужды
        var json = JsonConvert.SerializeObject(dataToPost);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync(API_BASE_URL + endpoint, content);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Данные успешно отправлены!");
            return "Данные успешно отправлены!";
        }
        else
        {
            return "Ошибка отправки данных: " + response.StatusCode;
        }
    }
}
