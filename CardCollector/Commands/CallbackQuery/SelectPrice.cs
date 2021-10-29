﻿using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class SelectPrice : CallbackQueryCommand
    {
        protected override string CommandText => Command.select_price;

        public override async Task Execute()
        {
            await MessageController.SendMessage(User, Messages.choose_price, 
                User.Session.State == UserState.AuctionMenu ? Keyboard.GemsPriceOptions : Keyboard.CoinsPriceOptions);
        }

        public SelectPrice() { }
        public SelectPrice(UserEntity user, Update update) : base(user, update) { }
    }
}