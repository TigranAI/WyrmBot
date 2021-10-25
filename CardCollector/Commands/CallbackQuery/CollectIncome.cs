﻿using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class CollectIncome : CallbackQueryCommand
    {
        protected override string CommandText => Command.collect_income;
        protected override bool ClearMenu => false;
        protected override bool AddToStack => false;

        public override async Task Execute()
        {
            var result = await User.Cash.Payout(User.Stickers);
            await MessageController.AnswerCallbackQuery(User, CallbackQueryId, 
                $"{Messages.you_collected} " +
                $"{result}{Text.coin} " +
                $"\n\n{Messages.your_cash}: " +
                $"{User.Cash.Coins}{Text.coin} " +
                $"{User.Cash.Gems}{Text.gem}", true);
            await MessageController.DeleteMessage(User, Update.CallbackQuery!.Message!.MessageId);
        }

        public CollectIncome() { }
        public CollectIncome(UserEntity user, Update update) : base(user, update) { }
    }
}