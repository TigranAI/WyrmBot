﻿using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Admin
{
    public class SelectForSalePack : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.select_for_sale_pack;
        protected override bool AddToStack => true;
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