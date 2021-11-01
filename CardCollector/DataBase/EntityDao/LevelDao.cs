﻿#nullable enable
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public class LevelDao
    {
        
        /* Получение объекта по Id */
        public static async Task<Level?> GetLevel(int level)
        {
            var Table = BotDatabase.Instance.Levels;
            return await Table.FirstOrDefaultAsync(item => item.LevelValue == level);
        }

        /* Добавление нового объекта в систему */
        /*private static async Task<Level> AddNew(long userId)
        {
            var level = new Level();
            var result = await Table.AddAsync(level);
            await Instance.SaveChangesAsync();
            return result.Entity;
        }*/
    }
}