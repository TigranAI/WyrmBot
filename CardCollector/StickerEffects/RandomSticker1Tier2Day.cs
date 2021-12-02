﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;

namespace CardCollector.StickerEffects
{
    public class RandomSticker1Tier3Day
    {
        public static int Interval = 2;

        public static async Task<int> GetStickersCount(Dictionary<string, UserStickerRelation> stickers)
        {
            var today = DateTime.Today.ToString(CultureInfo.CurrentCulture);
            var stickersWithEffect = (await StickerDao.GetListWhere(
                item => item.Effect == (int) Effect.RandomSticker1Tier3Day)).Select(item => item.Md5Hash);
            var userStickers = stickers.Values.Where(item => stickersWithEffect.Contains(item.ShortHash));
            return userStickers.Sum(item =>
            {
                var interval = DateTime.Today - Convert.ToDateTime(item.AdditionalData);
                if (interval.Days < Interval) return 0;
                item.AdditionalData = today;
                return item.Count;
            });
        }

        public static async Task<Dictionary<StickerEntity, int>> GenerateList(int count)
        {
            var stickers = await StickerDao.GetListWhere(item => item.Tier == 1);
            var rnd = new Random();
            var result = new List<StickerEntity>();
            for (var i = 0; i < count; i++)
                result.Add(stickers[rnd.Next(stickers.Count)]);
            return result.GroupBy(item => item).ToDictionary(item => item.Key, item => item.Count());
        }
    }
}