﻿using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.CallbackQueryHandler.Collection
{
    public class CombineCount : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.count;

        protected override async Task Execute()
        {
            var module = User.Session.GetModule<CombineModule>();
            var userSticker = User.Stickers.FirstOrDefault(item => item.Sticker.Id == module.SelectedStickerId);
            if (CallbackQuery.Data!.Contains(Text.plus))
            {
                if (module.Count < userSticker?.Count) module.Count++;
                else await AnswerCallbackQuery(User, CallbackQuery.Id, Messages.cant_change_count);
            }
            else if (CallbackQuery.Data!.Contains(Text.minus))
            {
                if (module.Count > 1) module.Count--;
                else await AnswerCallbackQuery(User, CallbackQuery.Id, Messages.cant_change_count);
            }

            await EditReplyMarkup(User, CallbackQuery.Message!.MessageId,
                Keyboard.GetCombineStickerKeyboard(module));
        }

        public override bool Match()
        {
            return base.Match() && User.Session.State is UserState.CombineMenu;
        }
    }
}