﻿using System.Threading.Tasks;
using CardCollector.DataBase;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.MyChatMemberHandler
{
    public class KickFromGroup : MyChatMemberHandler
    {
        protected override async Task Execute()
        {
            var telegramChat = await Context.TelegramChats
                .SingleOrDefaultAsync(item => item.ChatId == ChatMemberUpdated.Chat.Id);
            if (telegramChat == null) return;
            telegramChat.IsBlocked = true;
        }

        public override bool Match()
        {
            if (ChatMemberUpdated.NewChatMember.Status is not
                (ChatMemberStatus.Kicked or ChatMemberStatus.Left)) return false;
            return ChatMemberUpdated.Chat.Type is ChatType.Group or ChatType.Supergroup;
        }

        public KickFromGroup(User user, BotDatabaseContext context, ChatMemberUpdated member) : base(user, context,
            member)
        {
        }
    }
}