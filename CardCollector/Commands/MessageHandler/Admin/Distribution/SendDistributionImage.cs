﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Commands.ChosenInlineResultHandler.Admin;
using CardCollector.Database;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.MessageHandler.Admin.Distribution
{
    public class SendDistributionImage : MessageHandler
    {
        protected override string CommandText => "";
        
        private static readonly LinkedList<long> Queue = new();
        
        protected override async Task Execute()
        {
            RemoveFromQueue(User.Id);
            var module = User.Session.GetModule<AdminModule>();

            var distribution = await Context.ChatDistributions.FindById(module.ChatDistributionId!.Value);
            distribution.ImageFileId = Message.Photo![0].FileId;
            
            await User.Messages.SendMessage(Messages.send_distribution_sticker, 
                Keyboard.StickerSkipKeyboard(typeof(SetDistributionSticker).Name));
            
            User.Session.State = UserState.SendDistributionSticker;
        }

        public static async Task Skip(User user, BotDatabaseContext context)
        {
            RemoveFromQueue(user.Id);
            await user.Messages.SendMessage(Messages.send_distribution_sticker, 
                Keyboard.StickerSkipKeyboard(typeof(SetDistributionSticker).Name));
            
            user.Session.State = UserState.SendDistributionSticker;
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