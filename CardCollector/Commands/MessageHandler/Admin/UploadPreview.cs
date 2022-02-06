﻿using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.MessageHandler.Admin
{
    public class UploadPreview : MessageHandler
    {
        protected override string CommandText => "";
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            var packId = User.Session.GetModule<AdminModule>().SelectedPackId;
            var pack = await Context.Packs.FindPack(packId);
            pack.PreviewFileId = Message.Sticker!.FileId;
            pack.IsPreviewAnimated = Message.Sticker.IsAnimated;
            await MessageController.EditMessage(User, Messages.update_preview_success, Keyboard.BackAndMoreKeyboard);
        }

        public override bool Match()
        {
            if (User.PrivilegeLevel < PrivilegeLevel.Programmer) return false;
            if (User.Session.State != UserState.UploadPackPreview) return false;
            if (Message.Type is not MessageType.Sticker) return false;
            if (User.Session.GetModule<AdminModule>().SelectedPackId == null) return false;
            return true;
        }

        public UploadPreview(User user, BotDatabaseContext context, Message message) : base(user, context, message)
        {
        }
    }
}