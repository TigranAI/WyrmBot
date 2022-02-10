﻿using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.ChosenInlineResultHandler.Collection
{
    public class SelectCombineSticker : ChosenInlineResultHandler
    {
        protected override string CommandText => ChosenInlineResultCommands.select_sticker;
        protected override async Task Execute()
        {
            var stickerId = long.Parse(ChosenInlineResult.ResultId.Split('=')[1]);
            var module = User.Session.GetModule<CombineModule>();
            module.SelectedStickerId = stickerId;
            var sticker = await Context.Stickers.FindById(stickerId);
            var userSticker = User.Stickers.Single(item => item.Sticker.Id == stickerId);
            await User.Messages.SendSticker(User, sticker.FileId);
            await User.Messages.EditMessage(User, sticker.ToString(userSticker.Count), 
                Keyboard.GetCollectionStickerKeyboard(sticker, module.Count));
        }

        public override bool Match()
        {
            return base.Match() && User.Session.State == UserState.CombineMenu;
        }

        public SelectCombineSticker(User user, BotDatabaseContext context, ChosenInlineResult chosenInlineResult) : base(user, context, chosenInlineResult)
        {
        }
    }
}