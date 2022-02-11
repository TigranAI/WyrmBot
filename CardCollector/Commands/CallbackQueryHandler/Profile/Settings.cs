﻿using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.DataBase;
using CardCollector.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Profile
{
    [MenuPoint]
    public class Settings : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.settings;

        protected override async Task Execute()
        {
            await User.Messages.EditMessage(User, Messages.settings,
                Keyboard.Settings(User.PrivilegeLevel), ParseMode.Html);
        }

        public Settings(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery) { }
    }
}