﻿using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Commands.CallbackQueryHandler.Others;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

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
                await new Back().Init(User, Context, new Update() {CallbackQuery = CallbackQuery}).PrepareAndExecute();
            else 
                await User.Messages.EditMessage(combineModule.ToString(), Keyboard.GetCombineKeyboard(combineModule));
        }

        public override bool Match() => false;
    }
}