﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;

namespace CardCollector.DataBase.Entity
{
    public partial class UserEntity
    {
        public class UserSession
        {
            private readonly UserEntity user;
            public UserSession(UserEntity user)
            {
                this.user = user;
            }
            /* Текущее состояние пользователя */
            public UserState State = UserState.Default;

            /* Фильтры, примененные пользователем в меню коллекции/магазина/аукциона */
            public readonly Dictionary<string, object> Filters = new()
            {
                {Command.author, ""},
                {Command.tier, -1},
                {Command.emoji, ""},
                {Command.price_coins_from, 0},
                {Command.price_coins_to, 0},
                {Command.price_gems_from, 0},
                {Command.price_gems_to, 0},
                {Command.sort, SortingTypes.None},
            };
            
            /* Сообщения в чате пользователя */
            public readonly List<int> Messages = new();

            /* Прибыль, которую может получить пользователь, подсчитывается во время команды профиля */
            public int IncomeCoins;
            public int IncomeGems;
            private DateTime LastPayout;

            public async Task ClearMessages()
            {
                foreach (var messageId in Messages)
                    await MessageController.DeleteMessage(user, messageId);
                Messages.Clear();
            }

            public async Task CalculateIncome()
            {
                IncomeCoins = 0;
                IncomeGems = 0;
                LastPayout = DateTime.Now;
                foreach (var sticker in user.Stickers.Values)
                {
                    var stickerInfo = await StickerDao.GetStickerByHash(sticker.ShortHash);
                    var payoutInterval = LastPayout - sticker.Payout;
                    var payoutsCount = payoutInterval.Minutes / stickerInfo.IncomeTime;
                    if (payoutsCount < 1) continue;
                    var multiplier = payoutsCount * sticker.Count;
                    IncomeCoins += stickerInfo.IncomeCoins * multiplier;
                    IncomeGems += stickerInfo.IncomeGems * multiplier;
                }
            }

            public async Task PayOut()
            {
                IncomeCoins = 0;
                IncomeGems = 0;
                foreach (var sticker in user.Stickers.Values)
                {
                    var stickerInfo = await StickerDao.GetStickerByHash(sticker.ShortHash);
                    var payoutInterval = LastPayout - sticker.Payout;
                    var payoutsCount = payoutInterval.Minutes / stickerInfo.IncomeTime;
                    if (payoutsCount < 1) continue;
                    var multiplier = payoutsCount * sticker.Count;
                    var prevDate = sticker.Payout;
                    sticker.Payout += new TimeSpan(0, stickerInfo.IncomeTime, 0) * payoutsCount;
                    IncomeCoins += stickerInfo.IncomeCoins * multiplier;
                    IncomeGems += stickerInfo.IncomeGems * multiplier;
                }
                user.Cash.Coins += IncomeCoins;
                user.Cash.Gems += IncomeGems;
            }
        }
    }
}