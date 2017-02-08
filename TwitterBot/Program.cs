using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Models;

namespace TwitterBot
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length != 4)
            {
                Console.WriteLine("Usage: TwitterBot.exe consumerKey consumerSecret accessToken accessTokenSecret");
                return;
            }

            var consumerKey = args[0];
            var consumerSecret = args[1];
            var accessToken = args[2];
            var accessTokenSecret = args[3];

            ITwitterBot bot;
            int maximumNumberOfTweets = 5;

            try
            {
                bot = new TwitterBot(consumerKey, consumerSecret, accessToken, accessTokenSecret);

                while (true)
                {
                    Console.Write("Enter username>");

                    var username = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(username))
                        break;

                    IEnumerable<ITweet> tweets;

                    try
                    {
                        tweets = bot.ReadTweets(username, maximumNumberOfTweets);
                    }

                    catch (ArgumentException e) // can't access tweets due to incorrect username or protection
                    {
                        Console.WriteLine(e.Message);
                        continue;
                    }

                    if (tweets.Count() == 0)
                    {
                        Console.WriteLine("didn't found any tweets from " + username);
                        continue;
                    }

                    var maxCharCounts = FindMostCommonChars(tweets);

                    var messageFormat = maxCharCounts.Count() == 1 ?
                        "@{0}'s most used character is {1}: {2} times in {3} tweets" :
                        "@{0}'s most used characters are {1}: {2} times in {3} tweets";

                    var message = string.Format(messageFormat,
                                                    username,
                                                    maxCharCounts.Count() == 1 ? maxCharCounts.First().Key.ToString() :
                                                        string.Join(" and ", maxCharCounts.Select(x => x.Key.ToString())),
                                                    maxCharCounts.First().Value,
                                                    tweets.Count()
                                                    );

                    Console.WriteLine(message);

                    bot.SendTweet(message);
                }
            }
            catch (ArgumentException e) // Authentication failed
            {
                Console.WriteLine(e.Message);
                return;
            }
        }

        static IEnumerable<KeyValuePair<char, int>> FindMostCommonChars(IEnumerable<ITweet> tweets)
        {
            var plainTweetTexts = from tweet in tweets
                                  select tweet.FullText.Replace(" ", string.Empty);

            var charCounts = plainTweetTexts.CountChars();

            var maxCharCounts = from entry in charCounts
                                where charCounts.Max(x => x.Value) == entry.Value
                                select entry;

            return maxCharCounts;
        }
    }
}
