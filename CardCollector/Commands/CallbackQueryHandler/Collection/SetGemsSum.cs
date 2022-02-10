﻿using System.Threading.Tasks;
using CardCollector.Commands.PreCheckoutQueryHandler;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Collection
{
    public class SetGemsSum : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.set_gems_sum;
        
        protected override async Task Execute()
        {
            var count = int.Parse(CallbackQuery.Data!.Split('=')[1]);
            var label = string.Format(Text.gems_title, count, count / 100 * 69);
            var description = string.Format(Text.gems_description, count);
            await MessageController.SendInvoice(User, label, description, PreCheckoutQueryCommands.buy_gems_item, 
                new[] {new LabeledPrice(label, count*69)}, Keyboard.BuyGemsKeyboard(count), Currency.RUB);
        }

        public SetGemsSum(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery) { }
    }
}