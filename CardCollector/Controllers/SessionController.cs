﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Resources.Translations;
using CardCollector.Session;

namespace CardCollector.Controllers
{
    public static class SessionController
    {
        private static Dictionary<long, Tuple<long, UserSession>> _openedSessions = new();

        public static UserSession? FindSession(User user)
        {
            if (_openedSessions.ContainsKey(user.Id)) return _openedSessions[user.Id].Item2;
            return null;
        }

        public static void AddSession(User user)
        {
            if (!_openedSessions.ContainsKey(user.Id))
                _openedSessions.Add(user.Id, new Tuple<long, UserSession>(user.Id, user.Session));
        }

        public static async Task CloseSessions()
        {
            using (var context = new BotDatabaseContext())
            {
                foreach (var (userId, session) in _openedSessions.Values)
                {
                    session.EndSession();
                    var user = await context.Users.FindById(userId);
                    await user!.Messages.ClearChat();
                    await user.Messages.SendMessage(Messages.bot_turning_off);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}