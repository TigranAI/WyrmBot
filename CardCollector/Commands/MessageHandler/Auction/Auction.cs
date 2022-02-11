﻿using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.MessageHandler.Auction
{
    [Attributes.Menu.MenuPoint]
    public class Auction : MessageHandler
    {
        protected override string CommandText => MessageCommands.auction;
        protected override bool ClearMenu => true;
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            User.Session.State = UserState.AuctionMenu;
            var text = User.Session.GetModule<FiltersModule>().ToString(User.Session.State);
            await User.Messages.EditMessage(User, text, Keyboard.GetSortingMenu(User.Session.State));
        }

        public Auction(User user, BotDatabaseContext context, Message message) : base(user, context, message)
        {
        }
    }
}