﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Database.EntityDao;

namespace CardCollector.Cache.Entity
{
    public class LadderInfo
    {
        public int CurrentPackId = -1;
        public int GamesToday;
        public List<long> UserIds = new();
        public List<long> StickerIds = new();

        public void Add(long userId, int packId, long stickerId)
        {
            if (CurrentPackId != -1 &&
                (CurrentPackId != packId || UserIds.Contains(userId) || StickerIds.Contains(stickerId))) Reset();
            CurrentPackId = packId;
            UserIds.Add(userId);
            StickerIds.Add(stickerId);
        }

        public async Task<bool> TryComplete(int goal)
        {
            if (StickerIds.Count != goal) return false;
            
            GamesToday++;
            using (var context = new BotDatabaseContext())
            {
                foreach (var userId in UserIds)
                {
                    var user = await context.Users.FindById(userId);
                    user!.UserStats.IncreaseLadderGames();
                }

                await context.SaveChangesAsync();
            }

            Reset();
            return true;
        }

        public bool IsLimitReached(int maxGames)
        {
            return GamesToday >= maxGames;
        }

        public void Reset()
        {
            UserIds.Clear();
            StickerIds.Clear();
            CurrentPackId = -1;
        }

        public void ResetRestrictions()
        {
            GamesToday = 0;
        }
    }
}