﻿using System;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Database.Entity;
using CardCollector.Resources;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Profile
{
    public class ShowInvitedFriends : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.show_invited_friends;

        protected override async Task Execute()
        {
            var message = string.Join("\n", User.InviteInfo!.InvitedFriends
                .Select(friend => {
                    var timespan = friend.InviteInfo!.InvitedAt!.Value + Constants.BEGINNERS_TASKS_INTERVAL - DateTime.Now;
                    var progress =
                        (timespan < TimeSpan.Zero ? $"{Messages.time_is_up}" : $"{timespan.Days} {Text.days}") +
                        $" ({friend.InviteInfo.TasksProgress!.Progress} / {BeginnersTasksProgress.TaskCount})";
                    return $"{friend.Username} - {progress}";
                })
            );
            if (message == "") message = Messages.you_havent_invite_friends;
            await User.Messages.EditMessage(message, Keyboard.BackKeyboard);
        }
    }
}