using System;
using System.Collections.Generic;
using Tweetinvi;
using Tweetinvi.Models;

namespace TwitterBot
{
    class TwitterBot : ITwitterBot
    {
        /// <summary>
        /// Current user used by the bot
        /// </summary>
        IAuthenticatedUser authenticatedUser;

        public TwitterBot(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
        {
            Authenticate(consumerKey, consumerSecret, accessToken, accessTokenSecret);

            authenticatedUser = User.GetAuthenticatedUser();
            if (authenticatedUser == null)
                throw new ArgumentException("Incorrect credentials. Authentication failed.");

        }

        private void Authenticate(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
        {
            var credentials = Auth.CreateCredentials(consumerKey, consumerSecret, accessToken, accessTokenSecret);
            Auth.SetCredentials(credentials);
        }

        /// <summary>
        /// Read latest tweets of given user
        /// </summary>
        /// <param name="screenName">user's screen name with or without "@" mark</param>
        /// <param name="maximumNumberOfTweets">maximum number of tweets to be returned</param>
        /// <returns>collection of latest tweets from user</returns>
        public IEnumerable<ITweet> ReadTweets(string screenName, int maximumNumberOfTweets = 5)
        {
            var user = User.GetUserFromScreenName(screenName);

            if (user == null)
                throw new ArgumentException("screenName " + screenName + " is not found in twitter.");

            if (maximumNumberOfTweets < 1)
                throw new ArgumentOutOfRangeException("Maximum number of tweets must be greater or equal to 1");

            if (user.Protected)
                throw new ArgumentException("Can't access protected tweets of user.");

            var tweets = user.GetUserTimeline(maximumNumberOfTweets);
            return tweets;
        }

        /// <summary>
        /// Sends tweet with given text
        /// </summary>
        /// <param name="text">body of tweet</param>
        public void SendTweet(string text)
        {
            try
            {
                authenticatedUser.PublishTweet(text);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Can't send empty tweet.");
            }
        }
    }
}
