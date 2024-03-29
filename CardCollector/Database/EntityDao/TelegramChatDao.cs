﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Database.Entity;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CardCollector.Database.EntityDao
{
    public static class TelegramChatDao
    {
        public static async Task<TelegramChat?> FindById(this DbSet<TelegramChat> table, long id)
        {
            return await table.SingleOrDefaultAsync(item => item.Id == id);
        }

        public static async Task<TelegramChat> FindByChatId(this DbSet<TelegramChat> table, long chatId)
        {
            return await table.SingleOrDefaultAsync(item => item.ChatId == chatId)
                ?? await table.Create(await Bot.Client.GetChatAsync(chatId));
        }

        public static async Task<TelegramChat> FindByChat(this DbSet<TelegramChat> table, Chat chat)
        {
            var result = await table.SingleOrDefaultAsync(item => item.ChatId == chat.Id)
                         ?? await table.Create(chat);
            return result.Update(chat);
        }

        public static async Task<TelegramChat> Create(this DbSet<TelegramChat> table, Chat chat)
        {
            var result = await table.AddAsync(new TelegramChat()
            {
                ChatId = chat.Id,
                ChatType = chat.Type,
                Title = chat.Title
            });
            return result.Entity;
        }

        public static async Task<List<long>> SelectGroupChatIds(this DbSet<TelegramChat> table)
        {
            return await table
                .Where(item => !item.IsBlocked && !item.DistributionsDisabled)
                .Select(item => item.ChatId)
                .ToListAsync();
        }
    }
}