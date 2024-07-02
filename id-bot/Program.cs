using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GetGroupIdBot
{
    internal class Program
    {
        private static readonly string Token = "7140014967:AAGBKF2wUPuU2fUZ9b4kZFf7ifAHyNz0g58";
        private static TelegramBotClient botClient;

        static async Task Main(string[] args)
        {
            botClient = new TelegramBotClient(Token);

            var cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token);

            var me = await botClient.GetMeAsync();
            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            cts.Cancel();
            cts.Dispose();
        }

        private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update.Message.Text != null)
            {
                var message = update.Message;

                if (message.Text.ToLower() == "/getgroupid")
                {
                    await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: $"Group ID: {message.Chat.Id}",
                        cancellationToken: cancellationToken);
                }
            }
        }

        private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(exception.ToString());
            return Task.CompletedTask;
        }
    }
}
