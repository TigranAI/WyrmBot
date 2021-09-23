﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    /* Класс, предоставляющий доступ к объектам таблицы Stickers*/
    public static class StickerDao
    {
        private static readonly CardCollectorDatabase Instance = CardCollectorDatabase.GetSpecificInstance(typeof(StickerDao));
        /* Таблица stickers в представлении Entity Framework */
        private static readonly DbSet<StickerEntity> Table = Instance.Stickers;
        
        /* Получение информации о стикере по его хешу, возвращает Null, если стикера не существует */
        public static async Task<StickerEntity> GetStickerByHash(string hash)
        {
            return await Table.FirstOrDefaultAsync(item => item.Md5Hash == hash);
        }

         /* Добавление новго стикера в систему
         fileId - id стикера на сервере telegram
         title - название стикера
         author - автор
         incomeCoins - прибыль стикера в монетах / минуту
         incomeGems - прибыль стикера в алзмазах / минуту
         tier - количество звезд стикера
         emoji - эмоции, связанные со стикером
         description - описание стикера */
        private static async Task<StickerEntity> AddNew(string fileId, string title, string author,
            int incomeCoins = 0, int incomeGems = 0, int tier = 1, string emoji = "", string description = "")

        {
            var cash = new StickerEntity
            {
                Id = fileId, Title = title, Author = author,
                IncomeCoins = incomeCoins, IncomeGems = incomeGems,
                Tier = tier, Emoji = emoji, Description = description,
                Md5Hash = Utilities.CreateMd5(fileId)
            };
            var result = await Table.AddAsync(cash);
            await Instance.SaveChangesAsync();
            return result.Entity;
        }

        public static async Task<List<string>> GetAuthorsList()
        {
            return await Table
                .Select(item => item.Author)
                .Distinct()
                .ToListAsync();
        }

        public static async Task<List<StickerEntity>> GetAll(string filter = "")
        {
            var list = await Table.ToListAsync();
            return filter == "" ? list : list.Where
                (item => item.Title.Contains(filter, StringComparison.CurrentCultureIgnoreCase)).ToList();
        }

        public static async Task AddNew(StickerEntity sticker)
        {
            await Table.AddAsync(sticker);
        }
    }
}