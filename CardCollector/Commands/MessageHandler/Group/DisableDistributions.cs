﻿using System.Threading.Tasks;
using CardCollector.Database.EntityDao;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.MessageHandler.Group
{
    public class DisableDistributions : MessageHandler
    {
        protected override string CommandText => MessageCommands.disable_distributions;
        protected override async Task Execute()
        {
            var telegramChat = await Context.TelegramChats.FindByChat(Message.Chat);
            var chatMember = await Bot.Client.GetChatMemberAsync(Message.Chat.Id, User.ChatId);
            
            if (chatMember.Status is not (ChatMemberStatus.Administrator or ChatMemberStatus.Creator))
                await telegramChat.SendMessage(Messages.this_command_can_execute_only_administrator);
            else
            {
                telegramChat.DistributionsDisabled = true;
                await telegramChat.SendMessage(Messages.distributions_successfully_disabled);
            }
        }
        
        public override bool Match()
        {
            if (Message.Type != MessageType.Text) return false;
            var data = Message.Text!.Split("@");
            if (data.Length < 2) return false;
            if (data[0] != CommandText || data[1] != AppSettings.NAME) return false;
            return Message.Chat.Type is ChatType.Group or ChatType.Supergroup;
        }
    }
}