using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using Telegram.Bot.Types;

namespace CardCollector.Commands
{
    public class CommandNotFound : UpdateModel
    {
        protected override string Command => "";
        private readonly string _command;

        public override async Task<Telegram.Bot.Types.Message> Execute()
        {
            return await MessageController.SendMessage(User, "Команда не найдена " + _command);
        }

        public CommandNotFound(UserEntity user, Update update, string command) : base(user, update)
        {
            _command = command;
        }
    }
}