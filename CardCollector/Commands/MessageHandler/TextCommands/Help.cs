﻿using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.MessageHandler.TextCommands
{
    public class Help : MessageHandler
    {
        protected override string CommandText => MessageCommands.help;
        protected override async Task Execute()
        {
            await User.Messages.EditMessage(User, Messages.help);
        }

        public Help(User user, BotDatabaseContext context, Message message) : base(user, context, message)
        {
        }
    }
}