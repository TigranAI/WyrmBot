﻿using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Attributes.Menu;
using CardCollector.Database;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Admin
{
    [MenuPoint]
    public class SelectForSalePack : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.select_for_sale_pack;
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            User.Session.State = UserState.LoadForSaleSticker;
            var packId = int.Parse(CallbackQuery.Data!.Split('=')[1]);
            User.Session.GetModule<AdminModule>().SelectedPackId = packId;
            await User.Messages.EditMessage(User, Messages.choose_sticker, Keyboard.ShowStickers);
        }

        public override bool Match()
        {
            return base.Match() && User.PrivilegeLevel >= PrivilegeLevel.Programmer;
        }

        public SelectForSalePack(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery)
        {
        }
    }
}