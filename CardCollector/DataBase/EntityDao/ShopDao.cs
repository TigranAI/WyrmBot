﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class ShopDao
    {
        private static readonly DbSet<ShopEntity> Table = CardCollectorDatabase.Instance.Shop;

        public static async Task<IEnumerable<StickerEntity>> GetAllShopPositions(string filter = "")
        {
            var entityList = Table
                .Where(e => e.Count > 0 || e.IsInfinite)
                .Select(e => e.StickerId)
                .ToHashSet();
            var stickersList = await StickerDao.GetAll(filter);
            return stickersList.Where(e => entityList.Contains(e.Id));
        }

        public static async Task<ShopEntity> GetSticker(string stickerId)
        {
            return await Table.FirstAsync(e => e.StickerId == stickerId);
        }
    }
}