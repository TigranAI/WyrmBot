using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.Commands;
using CardCollector.Commands.MyChatMember;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Controllers
{
    using static Logs;

    /* Данный класс управляет получением обновлений и отправкой сообщений */
    public static class MessageController
    {
        /* Данный метод принимает обновления с сервера Телеграм, определяет для него обработчик и обрабатывет команду */
        public static async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken ct)
        {
            try
            {
                var executor = update.Type switch
                {
                    // Тип обновления - сообщение
                    UpdateType.Message => await MessageCommand.Factory(update),
                    // Тип обновления - нажатие на инлайн кнопку
                    UpdateType.CallbackQuery => await CallbackQueryCommand.Factory(update),
                    // Тип обновления - блокировка/добавление бота
                    UpdateType.MyChatMember => await MyChatMemberCommand.Factory(update),
                    // Тип обновления - вызов бота через @имя_бота
                    UpdateType.InlineQuery => await InlineQueryCommand.Factory(update),
                    // Тип обновления - выбор результата в инлайн меню
                    UpdateType.ChosenInlineResult => await ChosenInlineResultCommand.Factory(update),
                    // Тип обновления - платеж
                    UpdateType.PreCheckoutQuery => await PreCheckoutQueryCommand.Factory(update),
                    _ => throw new ArgumentOutOfRangeException()
                };
                // Обработать команду
                await executor.PrepareAndExecute();
            }
            catch (Exception e)
            {
                switch (e)
                {
                    // Случай, когда не определена обработка для данного типа обновленияот
                    case ArgumentOutOfRangeException:
                        LogOut(update.Type);
                        break;
                    // Ошибка, полученная со стороны Telegram API
                    case ApiRequestException:
                        LogOutWarning(e.Message);
                        break;
                    // Прочие ошибки
                    default:
                        LogOutError(e);
                        break;
                }
            }
        }

        // Обработка ошибок, полученных от сервера Телеграм
        public static Task HandleErrorAsync(ITelegramBotClient client, Exception e, CancellationToken ct)
        {
            switch (e)
            {
                case ApiRequestException:
                    LogOutWarning(e.Message);
                    break;
                default:
                    LogOutError(e);
                    break;
            }
            return Task.CompletedTask;
        }

        /* Метод для отправки сообщения
         user - пользователь, которому необходимо отправить сообщение
         message - текст сообщения
         keyboard - клавиатура, которую надо добавить к сообщению */
        public static async Task<Message> SendMessage(UserEntity user, string message, IReplyMarkup keyboard = null)
        {
            try
            {
                if (!user.IsBlocked)
                {
                    if (user.Session.Messages.Count > 0 && (keyboard is null || keyboard is InlineKeyboardMarkup))
                        return await EditMessage(user, message, (InlineKeyboardMarkup)keyboard, 
                            user.Session.Messages.Last());
                    var result = await Bot.Client.SendTextMessageAsync(user.ChatId, message, replyMarkup: keyboard,
                        disableNotification: true);
                    user.Session?.Messages.Add(result.MessageId);
                    return result;
                }
            }
            catch (Exception e)
            {
                LogOutWarning("Can't send text message " + e.Message);
            }
            return new Message();
        }
        
        /* Метод для отправки сообщения с разметкой html
         user - пользователь, которому необходимо отправить сообщение
         message - текст сообщения
         keyboard - клавиатура, которую надо добавить к сообщению */
        public static async Task<Message> SendTextWithHtml(UserEntity user, string message, IReplyMarkup keyboard = null)
        {
            try
            {
                if (!user.IsBlocked)
                {
                    if (user.Session.Messages.Count > 0 && (keyboard is null || keyboard is InlineKeyboardMarkup))
                        return await EditMessage(user, message, (InlineKeyboardMarkup)keyboard, 
                            user.Session.Messages.Last(), ParseMode.Html);
                    var result = await Bot.Client.SendTextMessageAsync(user.ChatId, message, ParseMode.Html,
                        replyMarkup: keyboard, disableNotification: true);
                    user.Session?.Messages.Add(result.MessageId);
                    return result;
                }
            }
            catch (Exception e)
            {
                LogOutWarning("Can't send text message with html " + e.Message);
            }
            return new Message();
        }
        
        /* Метод для отправки стикера
         user - пользователь, которому необходимо отправить сообщение
         fileId - id стикера, расположенного на серверах телеграм */
        public static async Task<Message> SendSticker(UserEntity user, string fileId, IReplyMarkup keyboard = null)
        {
            try
            {
                if (!user.IsBlocked)
                {
                    await user.ClearChat();
                    var result = await Bot.Client.SendStickerAsync(user.ChatId, fileId, true, replyMarkup: keyboard);
                    user.Session.StickerMessages.Add(result.MessageId);
                    return result;
                }
            }
            catch (Exception e)
            {
                LogOutWarning("Can't send sticker " + e.Message);
            }
            return new Message();
        }
        
        /* Метод для редактирования сообщения
         user - пользователь, которому необходимо отредактировать сообщение
         messageId - id сообщения
         message - текст сообщения
         keyboard - клавиатура, которую надо добавить к сообщению */
        public static async Task<Message> EditMessage(UserEntity user, string message,
            InlineKeyboardMarkup keyboard = null, int messageId = -1, ParseMode? parseMode = null)
        {
            try
            {
                if (!user.IsBlocked)
                {
                    var msgId = messageId != -1 ? messageId : user.Session.Messages.Last();
                    return await Bot.Client.EditMessageTextAsync(user.ChatId, msgId, message, replyMarkup: keyboard,
                        parseMode: parseMode);
                }
            }
            catch (Exception)
            {
                await user.ClearChat();
                try
                {
                    if (!user.IsBlocked)
                    {
                        var msg = await Bot.Client.SendTextMessageAsync(user.ChatId, message, parseMode, 
                            replyMarkup: keyboard, disableNotification: true);
                        user.Session.Messages.Add(msg.MessageId);
                        return msg;
                    }
                }
                catch (Exception e1)
                {
                    LogOut("Cant edit message: " + e1.Message);
                }
            }
            return new Message();
        }

        /* Метод для редактирования клавиатуры под сообщением
         user - пользователь, которому необходимо отредактировать сообщение
         messageId - Id сообщения
         keyboard - новая клавиатура, которую надо добавить к сообщению */
        public static async Task<Message> EditReplyMarkup(UserEntity user, InlineKeyboardMarkup keyboard, int messageId = -1)
        {
            try
            {
                if (!user.IsBlocked)
                {
                    var msgId = messageId != -1 ? messageId : user.Session.Messages.Last();
                    return await Bot.Client.EditMessageReplyMarkupAsync(user.ChatId, msgId, keyboard);
                }
            }
            catch (Exception e)
            {
                LogOutWarning("Can't edit reply markup " + e.Message);
            }
            return new Message();
        }
        
        public static async Task AnswerCallbackQuery(UserEntity user, string callbackQueryId, string text, bool showAlert = false)
        {
            try
            {
                user.Session.UndoCurrentCommand();
                if (!user.IsBlocked)
                    await Bot.Client.AnswerCallbackQueryAsync(callbackQueryId, text, showAlert);
            }
            catch (Exception e)
            {
                LogOutWarning("Can't answer callbackquery " + e.Message);
            }
        }
        
        /* Метод для удаления сообщения
         user - пользователь, которому необходимо удалить сообщение
         messageId - Id сообщения */
        public static async Task DeleteMessage(UserEntity user, int messageId, bool deleteFromList = true)
        {
            try
            {
                if (deleteFromList) user.Session.Messages.Remove(messageId);
                if (!user.IsBlocked)
                    await Bot.Client.DeleteMessageAsync(user.ChatId, messageId);
            }
            catch (Exception e)
            {
                LogOutWarning("Can't delete message " + e.Message);
            }
        }

        /* Метод для отправки изображения
         user - пользователь, которому необходимо отправить сообщение
         inputOnlineFile - фото, которое необходимо отправить
         message - текст сообщения
         keyboard - клавиатура, которую надо добавить к сообщению */
        public static async Task<Message> SendImage(UserEntity user, string fileId, string message = null, 
            InlineKeyboardMarkup keyboard = null)
        {
            try
            {
                if (!user.IsBlocked)
                    return await Bot.Client.SendPhotoAsync(user.ChatId, fileId, message, replyMarkup: keyboard, disableNotification: true);
            }
            catch (Exception e)
            {
                LogOutWarning("Can't send photo " + e.Message);
            }
            return new Message();
        }

        /* Метод для ответа на запрос @имя_бота
         queryId - Id запроса
         results - массив объектов InlineQueryResult */
        public static async Task AnswerInlineQuery(string queryId, IEnumerable<InlineQueryResult> results, 
            string offset = null)
        {
            await Bot.Client.AnswerInlineQueryAsync(queryId, results, isPersonal: true, nextOffset: offset, 
                cacheTime: Constants.INLINE_RESULTS_CACHE_TIME);
        }

        public static async Task<Message> SendInvoice(UserEntity user, string title, string description, 
            string payload, IEnumerable<LabeledPrice> prices, int maxTip = 0, IEnumerable<int> tips = null,
            InlineKeyboardMarkup keyboard = null, Currency currency = Currency.USD)
        {
            try
            {
                if (!user.IsBlocked)
                {
                    await user.ClearChat();
                    var result = await Bot.Client.SendInvoiceAsync(user.ChatId, title, description, payload,
                        AppSettings.PAYMENT_PROVIDER, currency.ToString(), prices, maxTip, tips,
                        replyMarkup: keyboard, disableNotification: true);
                    user.Session.Messages.Add(result.MessageId);
                    return result;
                }
            }
            catch (Exception e)
            {
                LogOutWarning("Can't send photo " + e.Message);
            }
            return new Message();
        }
    }
}