using Tweetinvi.Models;
using System.Collections.Generic;

namespace TwitterBot
{
    interface ITwitterBot
    {
        void SendTweet(string text);
        IEnumerable<ITweet> ReadTweets(string screenName, int maximumNumberOfTweets);
    }
}
