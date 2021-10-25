﻿using System.Threading.Tasks;
using CardCollector.DataBase.EntityDao;

namespace CardCollector.DailyTasks.CustomTasks
{
    public class SendStickers : DailyTask
    {
        public override int Id => (int)DailyTaskKeys.SendStickersToUsers;
        public override int Goal => 5;
        public override string Title => Titles.send_stickers;
        public override string Description => Descriptions.send_stickers;

        public override async Task<bool> Execute(long userId, object[] args = null)
        {
            var task = await DailyTaskDao.GetTaskInfo(userId, Id);
            if (task.Progress == 0) return false;
            task.Progress--;
            return task.Progress == 0;
        }

        public override async Task GiveReward(long userId, object[] args = null)
        {
            var userPacks = await UserPacksDao.GetOne(userId, 1);
            userPacks.Count++;
        }
    }
}