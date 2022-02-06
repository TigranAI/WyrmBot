﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;

namespace CardCollector.Session.Modules
{
    public class FiltersModule : Module
    {
        /* Фильтры, примененные пользователем в меню коллекции/магазина/аукциона */
        public readonly Dictionary<string, object> Filters = new()
        {
            {Command.authors_menu, ""},
            {Command.tier, -1},
            {Command.emoji, ""},
            {Command.price_coins_from, 0},
            {Command.price_coins_to, 0},
            {Command.price_gems_from, 0},
            {Command.price_gems_to, 0},
            {Command.sort, SortingTypes.None},
        };

        public string ToString(UserState state)
        {
            var text = $"{Messages.current_filters}:\n" +
                       $"{Messages.author}: {(Filters[Command.authors_menu] is string author and not "" ? author : Messages.all)}\n" +
                       $"{Messages.tier}: {(Filters[Command.tier] is int tier and not -1 ? new string('⭐', tier) : Messages.all)}\n" +
                       $"{Messages.emoji}: {(Filters[Command.emoji] is string emoji and not "" ? emoji : Messages.all)}\n";
            switch (state)
            {
                case UserState.AuctionMenu:
                    text += $"{Messages.price}: 💎 {Filters[Command.price_gems_from]} -" +
                            $" {(Filters[Command.price_gems_to] is int g and not 0 ? g : "∞")}\n";
                    break;
                case UserState.ShopMenu:
                    text += $"{Messages.price}: 💰 {Filters[Command.price_coins_from]} -" +
                            $" {(Filters[Command.price_coins_to] is int c and not 0 ? c : "∞")}\n";
                    break;
            }

            text += $"{Messages.sorting} {Filters[Command.sort]}\n\n{Messages.select_filter}:";
            return text;
        }
        
        public async Task<IEnumerable<Sticker>> ApplyTo(IEnumerable<Sticker> list, bool applyPrice = false)
        {
            /* Фильтруем по автору */
            if (Filters[Command.authors_menu] is string author && author != "")
                list = list.Where(item => item.Author.Contains(author));
            /* Фильтруем по тиру */
            if (Filters[Command.tier] is int tier && tier != -1)
                list = list.Where(item => item.Tier.Equals(tier));
            /* Фильтруем по эмоции */
            if (Filters[Command.emoji] is string emoji && emoji != "")
                list = list.Where(item => item.Emoji.Contains(emoji));
            /* Сортируем список, если тип сортировки установлен */
            if (Filters[Command.sort] is not string sort || sort == SortingTypes.None) return list;
            {
                /* Сортируем по автору */
                if (sort== SortingTypes.ByAuthor)
                    list = list.OrderBy(item => item.Author);
                /* Сортируем по названию */
                if (sort == SortingTypes.ByTitle)
                    list = list.OrderBy(item => item.Title);
                /* Сортируем по увеличению тира */
                if (sort == SortingTypes.ByTierIncrease)
                    list = list.OrderBy(item => item.Tier);
                /* Сортируем по уменьшению тира */
                if (sort == SortingTypes.ByTierDecrease)
                    list = list.OrderByDescending(item => item.Tier);
            }
            return applyPrice 
                ? await ApplyPriceTo(list)
                : list;
        }
        
        public async Task<IEnumerable<Sticker>> ApplyPriceTo(IEnumerable<Sticker> list)
        {
            /* Фильтруем по цене алмазов ОТ */
            /*if (Filters[Command.price_gems_from] is int PGF && PGF != 0)
                list = await list.WhereAsync(item => AuctionDao.HaveAny(item.Id, i => i.Price >= PGF));
            /* Фильтруем по цене адмазов ДО #1#
            if (Filters[Command.price_gems_to] is int PGT && PGT != 0)
                list = await list.WhereAsync(item => AuctionDao.HaveAny(item.Id, i => i.Price <= PGT));*/
            return list;
        }
        
        public IEnumerable<Auction> ApplyPriceTo(IEnumerable<Auction> list)
        {
            /* Фильтруем по цене алмазов ОТ */
            if (Filters[Command.price_gems_from] is int PGF && PGF != 0)
                list = list.Where(item => item.Price >= PGF);
            /* Фильтруем по цене адмазов ДО */
            if (Filters[Command.price_gems_to] is int PGT && PGT != 0)
                list = list.Where(item => item.Price <= PGT);
            return list;
        }
        
        public void Reset()
        {
            Filters[Command.authors_menu] = "";
            Filters[Command.tier] = -1;
            Filters[Command.emoji] = "";
            Filters[Command.price_coins_from] = 0;
            Filters[Command.price_coins_to] = 0;
            Filters[Command.price_gems_from] = 0;
            Filters[Command.price_gems_to] = 0;
            Filters[Command.sort] = SortingTypes.None;
        }
    }
}