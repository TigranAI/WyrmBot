﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Session;
using CardCollector.Session.Modules;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Resources
{
    /* В данном классе содержатся все клавиатуры, используемые в проекте */
    public static class Keyboard
    {

        /* Клавиатура, отображаемая с первым сообщением пользователя */
        public static readonly ReplyKeyboardMarkup Menu = new(new[]
        {
            new KeyboardButton[] {Text.profile, Text.collection},
            new KeyboardButton[] {Text.shop, Text.auction},
        }) {ResizeKeyboard = true};

        public static readonly InlineKeyboardMarkup PackMenu = new(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.open_random, $"{Command.open_pack}=1")},
            new[] {InlineKeyboardButton.WithCallbackData(Text.open_author, $"{Command.author_menu}=1")},
            new[] {InlineKeyboardButton.WithCallbackData(Text.open_specific, $"{Command.open_specific}=1")},
            new[] {InlineKeyboardButton.WithCallbackData(Text.cancel, Command.cancel)},
        });

        public static InlineKeyboardMarkup BackToFilters(string stickerTitle)
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[] {InlineKeyboardButton.WithSwitchInlineQuery(Text.send_sticker, stickerTitle)},
                new[] {InlineKeyboardButton.WithCallbackData(Text.back, $"{Command.back}={Command.clear_chat}")}
            });
        }

        public static InlineKeyboardMarkup GetSortingMenu(UserState state)
        {
            var keyboard = new List<InlineKeyboardButton[]>
            {
                new[] {InlineKeyboardButton.WithCallbackData(Text.author, $"{Command.author}=1")},
                new[] {InlineKeyboardButton.WithCallbackData(Text.tier, Command.tier)},
                new[] {InlineKeyboardButton.WithCallbackData(Text.emoji, Command.emoji)}
            };
            if (state != UserState.CollectionMenu) keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(Text.price, Command.price)});
            keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(Text.sort, Command.sort)});
            keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(Text.cancel, Command.cancel)});
            keyboard.Add(new[] {InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(Text.show_stickers)});
            return new InlineKeyboardMarkup(keyboard);
        }
        /* Клавиатура меню сортировки */
        public static readonly InlineKeyboardMarkup SortOptions = new(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.no, $"{Command.set}={Command.sort}={SortingTypes.None}")},
            new[] {InlineKeyboardButton.WithCallbackData(SortingTypes.ByTierIncrease, $"{Command.set}={Command.sort}={SortingTypes.ByTierIncrease}")},
            new[] {InlineKeyboardButton.WithCallbackData(SortingTypes.ByTierDecrease, $"{Command.set}={Command.sort}={SortingTypes.ByTierDecrease}")},
            new[] {InlineKeyboardButton.WithCallbackData(SortingTypes.ByAuthor, $"{Command.set}={Command.sort}={SortingTypes.ByAuthor}")},
            new[] {InlineKeyboardButton.WithCallbackData(SortingTypes.ByTitle, $"{Command.set}={Command.sort}={SortingTypes.ByTitle}")},
            new[] {InlineKeyboardButton.WithCallbackData(Text.cancel, Command.back)},
        });

        /* Клавиатура меню выбора тира */
        public static readonly InlineKeyboardMarkup TierOptions = new (new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.all, $"{Command.set}={Command.tier}=-1")},
            new[] {InlineKeyboardButton.WithCallbackData("1", $"{Command.set}={Command.tier}=1")},
            new[] {InlineKeyboardButton.WithCallbackData("2", $"{Command.set}={Command.tier}=2")},
            new[] {InlineKeyboardButton.WithCallbackData("3", $"{Command.set}={Command.tier}=3")},
            new[] {InlineKeyboardButton.WithCallbackData("4", $"{Command.set}={Command.tier}=4")},
            new[] {InlineKeyboardButton.WithCallbackData(Text.cancel, Command.back)},
        });

        /* Клавиатура меню ввода эмоджи */
        public static readonly InlineKeyboardMarkup EmojiOptions = new (new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.all, $"{Command.set}={Command.emoji}=")},
            new[] {InlineKeyboardButton.WithCallbackData(Text.cancel, Command.back)},
        });

        /* Клавиатура с одной кнопкой отмены */
        public static readonly InlineKeyboardMarkup CancelKeyboard = new (new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.cancel, Command.cancel)},
        });

        /* Клавиатура с отменой и выставлением */
        public static readonly InlineKeyboardMarkup AuctionPutCancelKeyboard = new (new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.sell_on_auction, Command.confirm_selling)},
            new[] {InlineKeyboardButton.WithCallbackData(Text.cancel, Command.cancel)},
        });

        /* Клавиатура меню выбора цен */
        public static readonly InlineKeyboardMarkup CoinsPriceOptions = new (new[]
        {
            new[] {
                InlineKeyboardButton.WithCallbackData($"💰 {Text.from} 0", $"{Command.set}={Command.price_coins_from}=0"),
                InlineKeyboardButton.WithCallbackData($"💰 {Text.to} 100", $"{Command.set}={Command.price_coins_to}=100"),
            },
            new[] {
                InlineKeyboardButton.WithCallbackData($"💰 {Text.from} 100", $"{Command.set}={Command.price_coins_from}=100"),
                InlineKeyboardButton.WithCallbackData($"💰 {Text.to} 500", $"{Command.set}={Command.price_coins_to}=500"),
            },
            new[] {
                InlineKeyboardButton.WithCallbackData($"💰 {Text.from} 500", $"{Command.set}={Command.price_coins_from}=500"),
                InlineKeyboardButton.WithCallbackData($"💰 {Text.to} 1000", $"{Command.set}={Command.price_coins_to}=1000"),
            },
            new[] {
                InlineKeyboardButton.WithCallbackData($"💰 {Text.from} 1000", $"{Command.set}={Command.price_coins_from}=1000"),
                InlineKeyboardButton.WithCallbackData($"💰 {Text.to} ∞", $"{Command.set}={Command.price_coins_to}=0"),
            },
            new[] {InlineKeyboardButton.WithCallbackData(Text.cancel, Command.back)},
        });

        /* Клавиатура меню выбора цен */
        public static readonly InlineKeyboardMarkup GemsPriceOptions = new (new[]
        {
            new[] {
                InlineKeyboardButton.WithCallbackData($"💎 {Text.from} 0", $"{Command.set}={Command.price_gems_from}=0"),
                InlineKeyboardButton.WithCallbackData($"💎 {Text.to} 10", $"{Command.set}={Command.price_gems_to}=10"),
            },
            new[] {
                InlineKeyboardButton.WithCallbackData($"💎 {Text.from} 10", $"{Command.set}={Command.price_gems_from}=10"),
                InlineKeyboardButton.WithCallbackData($"💎 {Text.to} 50", $"{Command.set}={Command.price_gems_to}=50"),
            },
            new[] {
                InlineKeyboardButton.WithCallbackData($"💎 {Text.from} 50", $"{Command.set}={Command.price_gems_from}=50"),
                InlineKeyboardButton.WithCallbackData($"💎 {Text.to} 100", $"{Command.set}={Command.price_gems_to}=100"),
            },
            new[] {
                InlineKeyboardButton.WithCallbackData($"💎 {Text.from} 100", $"{Command.set}={Command.price_gems_from}=100"),
                InlineKeyboardButton.WithCallbackData($"💎 {Text.to} ∞", $"{Command.set}={Command.price_gems_to}=0"),
            },
            new[] {InlineKeyboardButton.WithCallbackData(Text.cancel, Command.back)},
        });

        /* Возвращает клавиатуру со списоком авторов */
        public static InlineKeyboardMarkup GetAuthorsKeyboard(List<string> list, int page = 1)
        {
            /* Список авторов, отображаемый на текущей странице */
            var sublist = list.GetRange((page - 1) * 10,
                list.Count >= page * 10 ? 10 : list.Count % 10);
            /* Список кнопок на клавиатуре */
            var keyboardList = new List<InlineKeyboardButton[]>
            {
                new[]
                {
                    /* Добавляем в список кнопку "Все" */
                    InlineKeyboardButton.WithCallbackData(Text.all, 
                        $"{Command.set}={Command.author}=")
                }
            };
            /* Копируем список */
            var copyList = sublist.ToList();
            while (copyList.Count > 0)
            {
                /* Берем первый элемент и запихиваем его в строку */
                var author = copyList[0];
                copyList.RemoveAt(0);
                var keyRow = new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(author,
                            $"{Command.set}={Command.author}={author}")
                };
                /* Если есть еще элементы, то добавляем в строку вторую кнопку */
                if (copyList.Count > 0)
                {
                    author = copyList[0];
                    copyList.RemoveAt(0);
                    keyRow.Add(InlineKeyboardButton.WithCallbackData(author,
                            $"{Command.set}={Command.author}={author}"));
                }
                /* Добавляем строку кнопок в клавиатуру */
                keyboardList.Add(keyRow.ToArray());
            }

            /* Если всего авторов больше 10, то добавляем стрелочки */
            if (list.Count > 10)
                keyboardList.Add(
                    sublist.Count switch
                    {
                        <10 => new[]
                        {
                            InlineKeyboardButton.WithCallbackData(Text.previous, $"{Command.author}={page - 1}")
                        },
                        >=10 when page == 1 => new[]
                        {
                            InlineKeyboardButton.WithCallbackData(Text.next, $"{Command.author}={page + 1}")
                        },
                        _ => new[]
                        {
                            InlineKeyboardButton.WithCallbackData(Text.previous, $"{Command.author}={page - 1}"),
                            InlineKeyboardButton.WithCallbackData(Text.next, $"{Command.author}={page + 1}")
                        }
                    }
                );
            /* Добавляем кнопку отмены */
            keyboardList.Add(new[] {
                InlineKeyboardButton.WithCallbackData(Text.cancel, Command.back)
            });
            /* Вовзращаем клавиатуру */
            return new InlineKeyboardMarkup(keyboardList);
        }

        /* Возвращает клавиатуру со списоком авторов */
        public static InlineKeyboardMarkup GetAuthorsKeyboard(List<PackEntity> infoList, int page)
        {
            /* Список кнопок на клавиатуре */
            var keyboardList = new List<InlineKeyboardButton[]>();
            /* Копируем список */
            foreach (var (item, i) in infoList.WithIndex())
            {
                if (i % 2 == 0) keyboardList.Add(new [] {
                    InlineKeyboardButton.WithCallbackData(item.Author, $"{Command.open_pack}={item.Id}")
                });
                else keyboardList[keyboardList.Count - 1] = new [] {
                    keyboardList[keyboardList.Count - 1][0],
                    InlineKeyboardButton.WithCallbackData(item.Author, $"{Command.open_pack}={item.Id}")
                };
            }
            keyboardList.Add(new[] {
                InlineKeyboardButton.WithCallbackData(Text.previous, $"{Command.author_menu}={page - 1}"),
                InlineKeyboardButton.WithCallbackData(Text.next, $"{Command.author_menu}={page + 1}")
            });
            keyboardList.Add(new[] {
                InlineKeyboardButton.WithCallbackData(Text.cancel, Command.cancel)
            });
            /* Вовзращаем клавиатуру */
            return new InlineKeyboardMarkup(keyboardList);
        }

        /* Возвращает клавиатуру со списоком авторов */
        public static async Task<InlineKeyboardMarkup> GetAuthorsKeyboard(List<SpecificPacksEntity> infoList, int page)
        {
            /* Список кнопок на клавиатуре */
            var keyboardList = new List<InlineKeyboardButton[]>();
            /* Копируем список */
            foreach (var (item, i) in infoList.WithIndex())
            {
                var author = await PacksDao.GetById(item.PackId);
                if (i % 2 == 0) keyboardList.Add(new [] {
                    InlineKeyboardButton.WithCallbackData($"{author.Author} {item.Count}", $"{Command.open_pack}={item.PackId}")
                });
                else keyboardList[keyboardList.Count - 1] = new [] {
                    keyboardList[keyboardList.Count - 1][0],
                    InlineKeyboardButton.WithCallbackData($"{author.Author} {item.Count}", $"{Command.open_pack}={item.PackId}")
                };
            }
            keyboardList.Add(new[] {
                InlineKeyboardButton.WithCallbackData(Text.previous, $"{Command.open_specific}={page - 1}"),
                InlineKeyboardButton.WithCallbackData(Text.next, $"{Command.open_specific}={page + 1}")
            });
            keyboardList.Add(new[] {
                InlineKeyboardButton.WithCallbackData(Text.cancel, Command.cancel)
            });
            /* Вовзращаем клавиатуру */
            return new InlineKeyboardMarkup(keyboardList);
        }

        public static InlineKeyboardMarkup GetCollectionStickerKeyboard(CollectionModule module)
        {
            var sticker = module.SelectedSticker;
            var count = module.Count;
            var keyboard = new List<InlineKeyboardButton[]>
            {
                new[] {InlineKeyboardButton.WithSwitchInlineQuery(Text.send_sticker, sticker.Title)},
                new[] {InlineKeyboardButton.WithCallbackData($"{Text.sell_on_auction} ({count})", Command.sell_on_auction)},
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Text.minus, $"{Command.count}={Text.minus}"),
                    InlineKeyboardButton.WithCallbackData(Text.plus, $"{Command.count}={Text.plus}"),
                }
            };
            if (sticker.Tier != 4) keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData($"{Text.combine} ({count})", Command.combine)});
            keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(Text.back, $"{Command.back}={Command.clear_chat}")});
            return new InlineKeyboardMarkup(keyboard);
        }

        public static InlineKeyboardMarkup GetAuctionStickerKeyboard()
        {
            return new InlineKeyboardMarkup(new[] {
                new[] {InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(Text.show_traders)},
                new[] {InlineKeyboardButton.WithCallbackData(Text.back, $"{Command.back}={Command.clear_chat}")},
            });
        }

        public static InlineKeyboardMarkup GetAuctionProductKeyboard(AuctionModule module, double discount)
        {
            var price = (int)(module.Price * module.Count * discount);
            return new InlineKeyboardMarkup(new[] {
                new[] {InlineKeyboardButton.WithCallbackData($"{Text.buy} ({module.Count}) {price}{Text.gem}", Command.confirm_buying)},
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Text.minus, $"{Command.count}={Text.minus}"),
                    InlineKeyboardButton.WithCallbackData(Text.plus, $"{Command.count}={Text.plus}"),
                },
                new[] {InlineKeyboardButton.WithCallbackData(Text.back, $"{Command.back}={Command.clear_chat}")},
            });
        }

        public static InlineKeyboardMarkup GetStickerKeyboard(StickerEntity stickerInfo)
        {
            return new InlineKeyboardMarkup(new[] {
                new[] {InlineKeyboardButton.WithSwitchInlineQuery(Text.send_sticker, stickerInfo.Title)},
                new[] {InlineKeyboardButton.WithCallbackData(Text.back, Command.clear_chat)},
            });
        }

        public static InlineKeyboardMarkup GetConfirmationKeyboard(string command)
        {
            return new InlineKeyboardMarkup(new[] {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Text.no, Command.back),
                    InlineKeyboardButton.WithCallbackData(Text.yes, command)
                }
            });
        }

        public static InlineKeyboardMarkup GetStickerKeyboard(UserSession session, double discount = 1.0)
        {
            return session.State switch
            {
                UserState.ProductMenu => GetAuctionProductKeyboard(session.GetModule<AuctionModule>(), discount),
                UserState.AuctionMenu => GetAuctionStickerKeyboard(),
                UserState.CombineMenu => GetCombineStickerKeyboard(session.GetModule<CombineModule>()),
                UserState.CollectionMenu => GetCollectionStickerKeyboard(session.GetModule<CollectionModule>()),
                _ => GetStickerKeyboard(session.GetModule<DefaultModule>().SelectedSticker)
            };
        }

        public static InlineKeyboardMarkup GetCombineStickerKeyboard(CombineModule module)
        {
            return new InlineKeyboardMarkup(new[] {
                new[] {InlineKeyboardButton.WithCallbackData($"{Text.add} ({module.Count})", Command.combine)},
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Text.minus, $"{Command.count}={Text.minus}"),
                    InlineKeyboardButton.WithCallbackData(Text.plus, $"{Command.count}={Text.plus}"),
                },
                new[] {InlineKeyboardButton.WithCallbackData(Text.cancel, Command.back_to_combine)},
                new[] {InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(Text.select_another)},
            });
        }

        public static InlineKeyboardMarkup GetCombineKeyboard(CombineModule module)
        {
            var keyboard = new List<InlineKeyboardButton[]>();
            foreach (var (sticker, _) in module.CombineList)
            {
                keyboard.Add(new []{InlineKeyboardButton.WithCallbackData($"{Text.delete} {Text.sticker} {keyboard.Count + 1}",
                    $"{Command.delete_combine}={sticker.Md5Hash}")});
            }
            keyboard.Add(new []{InlineKeyboardButton.WithCallbackData(Text.cancel, Command.back)});
            if (module.GetCombineCount() == Constants.COMBINE_COUNT)
                keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(
                    $"{Text.combine} {module.CalculateCombinePrice()}{Text.coin}",
                    Command.combine_stickers)});
            else keyboard.Add(new[] {InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(Text.add_sticker)});
            return new InlineKeyboardMarkup(keyboard);
        }

        /* Клавиатура, отображаемая вместе с сообщением профиля */
        public static InlineKeyboardMarkup GetProfileKeyboard(int income, int privilegeLevel)
        {
            var keyboard = new List<InlineKeyboardButton[]> {
                new[] {InlineKeyboardButton.WithCallbackData($"{Text.collect} {income}{Text.coin}", Command.collect_income)},
                new[] {InlineKeyboardButton.WithCallbackData(Text.daily_tasks, Command.daily_tasks)},
                new[] {InlineKeyboardButton.WithCallbackData(Text.my_packs, Command.my_packs)},
            };
            if (privilegeLevel > 2) keyboard.Add(
                new[] {InlineKeyboardButton.WithCallbackData(Text.control_panel, Command.control_panel)});
            return new InlineKeyboardMarkup(keyboard);
        }

        public static InlineKeyboardMarkup ShopKeyboard(IEnumerable<ShopEntity> specialOffers)
        {
            var keyboard = new List<InlineKeyboardButton[]>();
            foreach (var offer in specialOffers)
                keyboard.Add(new []{InlineKeyboardButton.WithCallbackData(offer.Title,
                        $"{Command.select_offer}={offer.Id}")
                });
            keyboard.Add(new []{InlineKeyboardButton.WithCallbackData(Text.buy_pack, Command.buy_pack)});
            keyboard.Add(new []{InlineKeyboardButton.WithCallbackData(Text.buy_gems, Command.buy_gems)});
            keyboard.Add(new []{InlineKeyboardButton.WithCallbackData(Text.cancel, Command.cancel)});
            return new InlineKeyboardMarkup(keyboard);
        }

        public static InlineKeyboardMarkup ShopPacksKeyboard = new (new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.buy_random, $"{Command.select_offer}=1")},
            new[] {InlineKeyboardButton.WithCallbackData(Text.buy_author, $"{Command.select_offer}=2")},
            new[] {InlineKeyboardButton.WithCallbackData(Text.info, Command.pack_info)},
            new[] {InlineKeyboardButton.WithCallbackData(Text.cancel, Command.cancel)},
        });

        public static InlineKeyboardMarkup OfferKeyboard(ShopEntity offerInfo)
        {
            var keyboard = new List<InlineKeyboardButton[]>();
            if (offerInfo.PriceCoins >= 0)
                keyboard.Add(new [] {InlineKeyboardButton.WithCallbackData(
                    $"{offerInfo.ResultPriceCoins}{Text.coin}", Command.buy_by_coins)
                });
            if (offerInfo.PriceGems >= 0)
                if (keyboard.Count > 0) keyboard[0] = new [] {keyboard[0][0], InlineKeyboardButton.WithCallbackData(
                    $"{offerInfo.ResultPriceGems}{Text.gem}", Command.buy_by_gems)
                };
                else keyboard.Add(new [] {InlineKeyboardButton.WithCallbackData(
                    $"{offerInfo.ResultPriceGems}{Text.gem}", Command.buy_by_gems)
                });
            keyboard.Add(new []{InlineKeyboardButton.WithCallbackData(Text.cancel, Command.cancel)});
            return new InlineKeyboardMarkup(keyboard);
        }
    }
}