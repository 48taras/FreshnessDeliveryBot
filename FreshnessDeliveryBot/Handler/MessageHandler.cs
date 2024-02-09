using FreshnessDeliveryBot.DbContexts;
using FreshnessDeliveryBot.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

public class MessageHandler
{
    private readonly ITelegramBotClient botClient;
    private readonly FreshnessBotDbContext dbContext; // Додайте поле для контексту бази даних

    private readonly PersonService personService;
    private readonly ProductService productService;

    public MessageHandler(ITelegramBotClient client, FreshnessBotDbContext dbContext)
    {
        botClient = client;
        this.dbContext = dbContext;
        productService = new ProductService(dbContext);
    }

    public async Task HandleUpdate(Update update)
    {
        var message = update.Message;
        var callbackQuery = update.CallbackQuery;
        if (message != null)
        {
            var chatId = message.Chat.Id;
        }
        else if (callbackQuery != null)
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;
        }


        if (message != null)
        {
            if (message.Text == "/start")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, $"Вітаємо, {message.From.FirstName}! 🌟   \n" +
                "Я FreshnessBOT і готовий допомогти тобі з оформленням замовлення на доставку свіжої води. 🚚💦\n" +
                "Замовляйте воду, дізнавайтеся корисні поради та більше - у головному меню! 📋 ⬇️ "
                , replyMarkup: GetInlineMenu());
            }
            else if (message.Text == "Замовити воду 💧")
            {

            }
            else if (message.Text == "Каталог товарів 📦")
            {
                await ShowProductCatalog(message.Chat.Id);
            }
            //else if (update.CallbackQuery != null && update.CallbackQuery.Data != null && products.Any(p => p.CallBackData.Equals(update.CallbackQuery.Data, StringComparison.OrdinalIgnoreCase)))
            //{
            //    await ShowProductDetails(update.CallbackQuery.Message.Chat.Id, messageText);
            //}
            //else if (int.TryParse(messageText, out int quantity) && quantity > 0)
            //{
            //    if (selectedProduct != null)
            //    {
            //        var productCallBackData = selectedProduct.CallBackData;

            //        if (!string.IsNullOrEmpty(productCallBackData))
            //        {
            //            await HandleQuantityInput(update.CallbackQuery.Message.Chat.Id, productCallBackData, quantity);
            //        }
            //    }
            //    else
            //    {
            //        await botClient.SendTextMessageAsync(message.Chat.Id, "Кошик порожній!");
            //    }

            //}
            //else if (message.Text == "Кошик 🛒")
            //{
            //    await HandleCartCommand(chatId);
            //}
            else if (message.Text == "Контакти 📞")
            {
                var contactDescription = "🎨 Ось наші ключові контакти для вас:\n\n" +
                    "📧 Електронна пошта: freshness.delivery.dp.ua@gmail.com\n\n" +
                    "📱 Телефонуйте за номером:\n\n" +
                    "+380954948125 | Vodafone\n\n" +
                    "+380976300847 | Kyivstar\n\n" +
                    "Що ми можемо зробити для вас сьогодні ? 🌟";

                await botClient.SendTextMessageAsync(message.Chat.Id, contactDescription);
            }
            else if (message.Text == "Наш сайт 🌐")
            {
                string PathPhoto = "https://freshness-delivery.dp.ua/assets/images/bottle.jpg";
                var siteDescription = "🌐 Дізнайтеся більше про нашу доставку води!\n" +
                                      "✅ Високоякісна вода безпосередньо до вашого дому.\n" +
                                      "🚚 Швидка та надійна доставка за доступними цінами.\n" +
                                      "🛒 Обирайте і замовляйте у нашому каталозі прямо зараз!";

                await botClient.SendPhotoAsync(
                                chatId: message.Chat.Id,
                                photo: InputFile.FromUri(PathPhoto),
                                caption: siteDescription,
                                parseMode: ParseMode.Html,
                                replyMarkup: GetInlineButtSite(message.Chat.Id));
            }

        }
        else if (callbackQuery != null)
        {
            if (update.CallbackQuery.Data != null)
            {
                string selectedProduct = callbackQuery.Data;
                await HandleProductSelection(callbackQuery.Message.Chat.Id, selectedProduct);
            }

        }
    }

    public static ReplyKeyboardMarkup GetInlineMenu()
    {
        var replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
        {
                    new[]
                    {
                        new KeyboardButton("Замовити воду 💧"),
                        new KeyboardButton("Каталог товарів 📦"),
                    },
                    new[]
                    {
                        new KeyboardButton("Кошик 🛒"),
                        new KeyboardButton("Контакти 📞"),
                    },
                    new[]
                    {
                        new KeyboardButton("Наш сайт 🌐")
                    }
                })
        {
            ResizeKeyboard = true
        };

        return replyKeyboardMarkup;
    }

    public async Task HandleAfterRegistration(long chatId)
    {
        await botClient.SendTextMessageAsync(chatId, "Реєстрація пройшла успішно! 🎉\nТепер ти можеш дізнатися більше про мої можливості у головному меню! 📋 ⬇️ ", replyMarkup: GetInlineMenu());
    }

    private static InlineKeyboardMarkup GetInlineButtSite(long chatId)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
                new[]
                {
                    InlineKeyboardButton.WithUrl("Перейти","https://freshness-delivery.dp.ua/")
                }
                });
        return inlineKeyboard;
    }

    //private async Task HandleCartCommand(long chatId)
    //{
    //    var cartDescription = "Ваш кошик:\n";

    //    //if (selectedQuantities.Any() && selectedProducts.Any())
    //    //{
    //    //    foreach (var product in selectedProducts)
    //    //    {
    //    //        var quantity = selectedQuantities.ContainsKey(chatId) ? selectedQuantities[chatId] : 1;
    //    //        cartDescription += $"{product.Name} - {quantity} шт. - {product.Price * quantity} грн\n";
    //    //    }

    //    //    var totalAmount = selectedProducts.Sum(product => product.Price * selectedQuantities[chatId]);
    //    //    cartDescription += $"\nЗагальна сума: {totalAmount} грн";

    //    //    await botClient.SendTextMessageAsync(chatId, cartDescription, replyMarkup: GetInlineButtOrber(chatId));
    //    //}
    //    //else
    //    //{
    //    //    await botClient.SendTextMessageAsync(chatId, "Кошик порожній!");
    //    //}
    //}

    private async Task ShowProductCatalog(long chatId)
    {
        var catalogMessage = "Оберіть товар з каталогу:\n";
        var products = await productService.GetAllAsync();// Отримати список товарів з бази даних

        await botClient.SendTextMessageAsync(chatId, catalogMessage, replyMarkup: GetProductCatalogButtons(products));
    }

    private static InlineKeyboardMarkup GetProductCatalogButtons(List<Product> products)
    {
        var catalogButtons = new List<List<InlineKeyboardButton>>();

        foreach (var product in products)
        {
            var button = InlineKeyboardButton.WithCallbackData(product.Name, product.Name);
            catalogButtons.Add(new List<InlineKeyboardButton> { button });
        }

        return new InlineKeyboardMarkup(catalogButtons);
    }

    private async Task HandleProductSelection(long chatId, string selectedProduct)
    {
        // Получите информацию о выбранном товаре из базы данных
        var product = await productService.GetByNameAsync(selectedProduct);

        if (product != null)
        {
            var detailsMessage = $"{product.Name}\n\n{product.Description}\n\nЦіна: {product.Price} грн\n\nВведіть кількість товару (до 10):";

            var quantityButtons = Enumerable.Range(1, 10).Select(q =>
            InlineKeyboardButton.WithCallbackData(q.ToString(), $"{selectedProduct}_{q}")
        ).ToArray();

            var quantityKeyboard = new InlineKeyboardMarkup(quantityButtons);


            await botClient.SendTextMessageAsync(chatId, detailsMessage, replyMarkup: quantityKeyboard);

        }
        else
        {
            await botClient.SendTextMessageAsync(chatId, "Товар не знайдено.");
        }
    }

}

