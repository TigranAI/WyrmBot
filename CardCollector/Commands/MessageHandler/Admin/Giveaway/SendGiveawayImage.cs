﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Commands.CallbackQueryHandler;
using CardCollector.Database;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.MessageHandler.Admin.Giveaway
{
    public class SendGiveawayImage : MessageHandler
    {
        protected override string CommandText => "";

        private static readonly LinkedList<long> Queue = new();

        protected override async Task Execute()
        {
            RemoveFromQueue(User.Id);
            var module = User.Session.GetModule<AdminModule>();
            if (module.SelectedChannelGiveawayId == null) return;
            var giveaway = await Context.ChannelGiveaways.FindById(module.SelectedChannelGiveawayId.Value);
            giveaway!.ImageFileId = Message.Photo?.First().FileId;
            await User.Messages.ClearChat();
            await User.Messages.SendPhoto(giveaway.ImageFileId!, giveaway.GetFormattedMessage(),
                giveaway.GetFormattedKeyboard(CallbackQueryCommands.ignore));
            await User.Messages.SendMessage(Messages.please_confirm_this_giveaway, Keyboard.CreateGiveaway);
        }

        public static async Task Skip(User user, BotDatabaseContext context)
        {
            RemoveFromQueue(user.Id);
            var module = user.Session.GetModule<AdminModule>();
            if (module.SelectedChannelGiveawayId == null) return;
            var giveaway = await context.ChannelGiveaways.FindById(module.SelectedChannelGiveawayId.Value);
            await user.Messages.ClearChat();
            await user.Messages.SendMessage(giveaway!.GetFormattedMessage(), 
                giveaway.GetFormattedKeyboard(CallbackQueryCommands.ignore));
            await user.Messages.SendMessage(Messages.please_confirm_this_giveaway, Keyboard.CreateGiveaway);
        }

        public static void AddToQueue(long userId)
        {
            if (!Queue.Contains(userId)) Queue.AddLast(userId);
        }

        public static void RemoveFromQueue(long userId)
        {
            Queue.Remove(userId);
        }

        public override bool Match()
        {
            return Queue.Contains(User.Id) && Message.Type == MessageType.Photo;
        }
    }
}