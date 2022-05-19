﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types.InlineQueryResults;

namespace CardCollector.Commands.InlineQueryHandler.Admin
{
    public class ShowTest : InlineQueryHandler
    {
        protected override string CommandText => "#test";

        protected override async Task Execute()
        {
            var answer = new List<InlineQueryResultCachedSticker>()
            {
                new ("1", "CAACAgIAAxkBAAJRwWJhc1w-OmssUWdw3HZ94YmZrV9mAAJxFAACx1bpSxoOmlJY9fivJAQ")
            };
            await AnswerInlineQuery(User, InlineQuery.Id, answer, null);
        }

        public override bool Match()
        {
            return InlineQuery.Query.StartsWith(CommandText);
        }
    }
}