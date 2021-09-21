﻿using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message.TextMessage
{
    /* Обработка команды "/start" */
    public class StartMessage : Message
    {
        /* */
        protected override string CommandText => Text.start;
        
        public override async Task Execute()
        {
            /* Отправляем пользователю сообщение со стандартной клавиатурой */
            await MessageController.SendMessage(User, Messages.start_message, Keyboard.Menu);
        }
        
        public StartMessage(UserEntity user, Update update) : base(user, update) { }
        public StartMessage() { }
    }
}