﻿using System.Threading.Tasks;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.MessageHandler.Admin
{
    public class UploadForSaleSticker : MessageHandler
    {
        protected override string CommandText => "";
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            var stickerId = User.Session.GetModule<AdminModule>().SelectedStickerId;
            var sticker = await Context.Stickers.SingleAsync(item => item.Id == stickerId);
            sticker.ForSaleFileId = Message.Sticker!.FileId;
            sticker.IsForSaleAnimated = Message.Sticker.IsAnimated;
            await User.Messages.EditMessage(Messages.add_watermark_success, Keyboard.BackAndMoreKeyboard);

            await Context.SaveChangesAsync();
            await new RequestBuilder()
                .SetUrl("recache")
                .AddParam("stickerId", sticker.Id)
                .AddParam("type", (int) RecacheType.UploadForSaleSticker)
                .Send();
        }

        public override bool Match()
        {
            if (Message.Type is not MessageType.Sticker) return false;
            if (User.Session.State is not UserState.LoadForSaleSticker) return false;
            if (User.Session.GetModule<AdminModule>().SelectedStickerId == null) return false;
            return true;
        }
    }
}