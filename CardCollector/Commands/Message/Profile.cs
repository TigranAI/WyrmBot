﻿using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message
{
    /* Команда "Профиль" Отображает профиль пользователя и его баланс */
    public class Profile : MessageCommand
    {
        /* Для данной команды ключевое слово "Профиль" */
        protected override string CommandText => Text.profile;
        protected override bool ClearMenu => true;
        protected override bool AddToStack => true;
        protected override bool ClearStickers => true;

        public override async Task Execute()
        {
            /* Подсчитываем прибыль */
            var income = await User.Cash.CalculateIncome(User.Stickers);
            /* Отправляем сообщение */
            await MessageController.SendMessage(User, 
                /* Имя пользователя */
                $"{User.Username}\n" +
                /* Количество монет */
                $"{Messages.coins}: {User.Cash.Coins}{Text.coin}\n" +
                /* Количество алмазов */
                $"{Messages.gems}: {User.Cash.Gems}{Text.gem}",
                /* Клавиатура профиля */
                Keyboard.GetProfileKeyboard(income, User.PrivilegeLevel));
        }
        
        public Profile() { }
        public Profile(UserEntity user, Update update) : base(user, update) { }
    }
}