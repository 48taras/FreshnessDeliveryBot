using FreshnessDeliveryBot.DbContexts;
using FreshnessDeliveryBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreshnessDeliveryBot.Handler
{
    public class RegistrationHandler
    {
        private readonly ITelegramBotClient botClient;
        private readonly FreshnessBotDbContext dbContext;

        private readonly PersonService personService;

        private string phoneNum = "";

        public RegistrationHandler(ITelegramBotClient client, FreshnessBotDbContext dbContext)
        {
            botClient = client;
            this.dbContext = dbContext;
            personService = new PersonService(dbContext);
        }

        public async Task HandleRegistration(Update update)
        {
            var message = update.Message;
            var callbackQuery = update.CallbackQuery;
            var chatId = message.Chat.Id;

            string address ;

            if (message != null)
            {
                if (message.Text == "/start")
                {
                    await botClient.SendTextMessageAsync(chatId, $"Вітаю,{message.From.FirstName} 🌟! \n" +
                        $"Схоже ви тут перший раз! 😀 \n" +
                        $"Отже давайте знайомитися! 🤓 \n" +
                        $"Зараз мені треба дізнатися ваш номер телефону для зворотнього зв'язку 📞\n" +
                        $"Введіть, будь ласка, у форматі +380... 🙂");
                }
                else if (message.Text.StartsWith("+380"))
                {
                    string phoneNumberInput = message.Text;

                    if (IsValidPhoneNumber(phoneNumberInput))
                    {
                        phoneNum = phoneNumberInput;
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Тепер введіть повну адресу доставки (місто,вулиця,будинок,квартира) 🏣 \n" +
                            "У форматі 'Адреса: ... ' ");
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Введіть номер телефону у форматі +380... 📱 ! ");
                    }
                }
                else if (message.Text.StartsWith("Адреса:"))
                {
                    string addressInput = message.Text.Replace("Адреса:", "");

                    if (!string.IsNullOrWhiteSpace(addressInput) && IsValidAddress(addressInput))
                    {
                        address = addressInput;

                        if (!string.IsNullOrEmpty(phoneNum) && !string.IsNullOrEmpty(address))
                        {
                            RegisterUser(chatId, message.From.Id, message.From.FirstName, phoneNum, address);

                            var messageHandler = new MessageHandler(botClient, dbContext);
                            await messageHandler.HandleAfterRegistration(chatId);
                        }
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(chatId, "Введіть коректну адресу 🏡 ");
                    }
                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"{message.From.FirstName}, ведені дані не коректні. Будь ласка, використовуйте запропонований формат. 😉");
                }
            }
        }

        private async Task RegisterUser(long chatId, long personId, string name, string phoneNumber, string address)
        {
            Person person = new Person
            {
                CustomerID = personId,
                FirstName = name,
                PhoneNumber = phoneNumber,
                Address = address
            };

            try
            {
                await personService.CreateAsync(person);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await botClient.SendTextMessageAsync(chatId, $"Під час реєстрації сталася помилка...");
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // Паттерн для проверки формата +380...
            string regexPattern = @"^\+380\d{9}$";

            return Regex.IsMatch(phoneNumber, regexPattern);
        }

        private bool IsValidAddress(string address)
        {
            // Проверка, что адрес не пустой и содержит только буквы, цифры и символы пробела
            if (string.IsNullOrWhiteSpace(address) || !address.All(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)))
            {
                return false;
            }

            // Проверка длины адреса
            if (address.Length > 100)
            {
                return false;
            }

            return true;
        }
    }
}
