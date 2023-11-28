using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json; 
using System.Threading.Tasks; 

namespace WebClient
{
    static class Program
    {

        static async Task Main(string[] args)
        {
            var commands = new Dictionary<String, Func<Task>>
                            {
                                { "view", View },
                                { "add", AddRandom }
                            };
            var userCommand = "";
            try
            {
                while (!userCommand.StartsWith("exit"))
                {
                    var splits = userCommand.Split(' ');
                    var command = splits[0];
                    var param = splits.Length > 1 ? splits.Skip(1).ToArray() : new String[0];
                    if (!commands.ContainsKey(command))
                        Console.WriteLine($"Доступны следующие команды {String.Join(", ", commands.Keys)}, exit");
                    else
                        await commands[command]();
                    userCommand = Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Что-то пошло не так, а именно: {ex.Message}");
            }
        }
         

        private static async Task View()
        {
            Console.WriteLine("Введите id клиента:");
            if (!long.TryParse(Console.ReadLine(), out var id))
                Console.WriteLine("Требовалось целое число");
            await View(id);
        }

        private static async Task View(long id)
        {
            using var httpClient = new HttpClient();
            var url = $"https://localhost:5001/customers/{id}";
            var resp = await httpClient.GetAsync(url);
            if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                Console.WriteLine("Данного пользователя не существует");
            else if (!resp?.IsSuccessStatusCode ?? true)
                Console.WriteLine("Что-то пошло не так");
            else
            {
                Console.WriteLine($"Данные пользователя с id = {id}:");
                Console.WriteLine(await resp.Content.ReadAsStringAsync());
            }
        }

        private static async Task AddRandom()
        {
            var rnd = new Random();
            var request = new CustomerCreateRequest($"FirstName {rnd.Next(1, 9999)}", $"LastName {rnd.Next(1, 9999)}");
            var json = JsonSerializer.Serialize(request);
            Console.WriteLine("Создан пользователь:");
            Console.WriteLine(json);
            using var httpClient = new HttpClient();
            var url = "https://localhost:5001/customers/create";
            var content = new StringContent(json);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var resp = await httpClient.PostAsync(url, content);
            if (!resp?.IsSuccessStatusCode ?? true)
                Console.WriteLine("Что-то пошло не так");
            else
            {
                var id = long.Parse(await resp.Content.ReadAsStringAsync());
                await View(id);
            }
        }
    }
}