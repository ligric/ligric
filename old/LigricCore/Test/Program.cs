﻿using BoardRepositories.Abstractions;
using BoardRepositories.BitZlato;
using BoardRepositories.BitZlato.Types;
using Common.Enums;
using Common.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        #region IBitZlatoRequestsService initialization
        private static string apiKey = "{" +
                        "\"kty\":\"EC\"," +
                        "\"alg\":\"ES256\"," +
                        "\"crv\":\"P-256\"," +
                        "\"x\":\"WnVJnRzpTUo0mYEdkiDSuyGqfDZtBVLepkzqHk7O8SE\"," +
                        "\"y\":\"J9P-SkGy4qyyL6f-T9KHtJzwiTASHcxAxmwtWiUVF1Q\"," +
                        "\"d\":\"1xF-MartEnw4cAQB3eJC-Eg5YwThMemMx96DuHhyGFA\"" +
                        "}";

        private static string email = "balalay16@gmail.com";

        private static Dictionary<string, string> filters = new Dictionary<string, string>()
        {
            { "limit", "10" },
            { "currency", "RUB" },
            { "type", "purchase" },
            { "cryptocurrency", "BTC" }
        };
        #endregion

        public async static Task Main(string[] args)
        {
            AbstractBoardRepositoryWithTimer<Ad> bitZlatoRepository = new BitZlatoWithTimerRepository(apiKey, email, TimeSpan.FromSeconds(5), filters, StateEnum.Active);

            bitZlatoRepository.AdsChanged += OnAdsChangedAsync;
            Console.ReadLine();
        }

        private static int oldNumber = -1;
        private async static void OnAdsChangedAsync(object? sender, NotifyEnumerableChangedEventArgs<Ad> e)
        {
            int timeout = 0;
            int еxpectedNumber = e.Number - 1;

            while (oldNumber < еxpectedNumber && timeout < 100)
            {
                timeout++;
                await Task.Delay(1);
            }

            if (oldNumber < еxpectedNumber)
                throw new ArgumentException($"Сообщение после {oldNumber} потерялось.");
            else if (oldNumber > еxpectedNumber)
                throw new ArgumentException($"Сообщение после {oldNumber} уже обработано");
            else
            {
                try
                {
                    await Task.Run(() => OnAdsChanged(e));
                    oldNumber++;
                    if (oldNumber != e.Number)
                        throw new ArgumentException("Чё-то непонятное произошло.");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message);
                }
            }
        }

        private static List<Ad> tempAds = new List<Ad>();
        private static void OnAdsChanged(NotifyEnumerableChangedEventArgs<Ad> e)
        {
            switch (e.Action)
            {
                case NotifyEnumumerableChangedAction.Added:
                    foreach (var item in e.NewValues)
                    {
                        tempAds.Add(item);
                        ShowLineToConsole(item, '+');
                    }
                    break;
                case NotifyEnumumerableChangedAction.Changed:
                    Console.Clear();
                    foreach (var item in e.NewValues)
                    {
                        var change = tempAds.Find(x => x.Id == item.Id);
                        change = item;
                    }

                    foreach (var item in tempAds)
                    {
                        var find = e.NewValues.FirstOrDefault(x => x.Id == item.Id);
                        if (find == null)
                        {
                            ShowLineToConsole(item, ' ');
                        }
                        else
                        {
                            ShowLineToConsole(item, '~');
                        }
                    }
                    break;
                case NotifyEnumumerableChangedAction.Removed:
                    Console.Clear();
                    foreach (var remove in e.OldValues)
                    {
                        tempAds.Remove(remove);
                        ShowLineToConsole(remove, '-');
                    }
                    foreach (var current in tempAds)
                    {
                        ShowLineToConsole(current, ' ');
                    }
                    break;
                case NotifyEnumumerableChangedAction.Reset:
                    Console.Clear();
                    foreach (var item in e.NewValues)
                    {
                        tempAds.Add(item);
                        ShowLineToConsole(item, '+');
                    }
                    break;
                default:
                    break;
            }
        }


        private static void ShowLineToConsole(Ad ad, char symbol)
        {
            Console.Write(symbol + "\tId\t" + ad.Id + "\tType\t" + ad.Type.ToString() + "\tRate\t" + ad.Rate.Value +
                                   "\n\t\tCrypto currency\t" + ad.Rate.RightCurrency.Name + "\tMin\t" + ad.LimitCurrencyRight.From + "\tMax\t" + ad.LimitCurrencyRight.To +
                                   "\n\t\tCurrency\t" + ad.Rate.LeftCurrency.Name + "\tMin\t" + ad.LimitCurrencyLeft.From + "\tMax\t" + ad.LimitCurrencyLeft.To + "\n");
        }
    }
}