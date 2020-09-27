using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AwesomeBot.CommandHandlers
{
    public abstract class BaseCommandHandler
    {
        protected TelegramBotClient BotClient { get; set; }
        public BaseCommandHandler(TelegramBotClient botCLient)
        {
            BotClient = botCLient;
        }
        protected abstract bool ValidateMessage(Message message);

        protected abstract Task CustomHandle(Message message);

        public async Task Handle(Message message)
        {
            if (!ValidateMessage(message))
            {
                await BotClient.SendTextMessageAsync(message.Chat.Id, "Invalid message format. Try again please..");
                return;
            }

            await CustomHandle(message);
        }
    }
}
