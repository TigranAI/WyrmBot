﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.InlineQueryHandler
{
    [Attributes.Handlers.InlineQueryHandler]
    public abstract class InlineQueryHandler : HandlerModel
    {
        protected readonly InlineQuery InlineQuery;
        protected override string CommandText => "";

        public static readonly ICollection<Type> Commands;
        
        static InlineQueryHandler()
        {
            Commands = new LinkedList<Type>();
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes())
            {
                if (type == typeof(InlineQueryHandler)) continue;
                if (Attribute.IsDefined(type, typeof(Attributes.Handlers.InlineQueryHandler)))
                    Commands.Add(type);
            }
        }

        public static async Task<HandlerModel> Factory(Update update)
        {
            var context = new BotDatabaseContext();
            var user = await context.Users.FindUser(update.InlineQuery!.From);
            if (user.IsBlocked) return new IgnoreHandler(user, context);
            
            user.InitSession();

            foreach (var handlerType in Commands)
            {
                var handler = (HandlerModel?) Activator.CreateInstance(handlerType, user, context, update.InlineQuery);
                if (handler != null && handler.Match()) return handler;
            }
            
            return new IgnoreHandler(user, context);
        }

        protected InlineQueryHandler(User user, BotDatabaseContext context, InlineQuery inlineQuery) : base(user, context)
        {
            InlineQuery = inlineQuery;
        }
    }
}