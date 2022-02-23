﻿using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Attributes.Menu;
using CardCollector.Commands.CallbackQueryHandler.Others;
using CardCollector.Database;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Collection
{
    [MenuPoint]
    public class CombineMenu : CallbackQueryHandler
    {
        protected override string CommandText => "";
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            var combineModule = User.Session.GetModule<CombineModule>();
            if (combineModule.CombineCount == 0)
                await new Back(User, Context, CallbackQuery).PrepareAndExecute();
            else 
                await User.Messages.EditMessage(User, combineModule.ToString(), Keyboard.GetCombineKeyboard(combineModule));
        }

        public override bool Match() => false;

        public CombineMenu(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery) { }
    }
}