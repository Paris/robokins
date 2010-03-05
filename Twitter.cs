using System;
using System.Diagnostics;
using System.Timers;
using robokins.IRC;
using robokins.Utility;

namespace robokins
{
    partial class Bot
    {
        Timer tweets = null;
        string[] tweetResponses;
        bool tweetNext = false;
        User tweetLast = null;
        string tweetId = null;

        [Conditional("TWEET")]
        void TweetSetup()
        {
            tweetResponses = new[]
            {
                "Great",
                "Haha yeah",
                "lol",
                "You won't believe how often I hear that",
                "I agree",
                "Perhaps you're right",
                "Interesting",
                "I suppose so",
                "If you say so",
                "Really",
                "That struck a chord",
                "You wish",
            };

            tweets = new Timer(60 * 60 * 1000 * 3);
            tweets.Elapsed += new ElapsedEventHandler(delegate(object sender, ElapsedEventArgs e)
            {
                tweetNext = true;
            });
            tweets.Start();
        }

        [Conditional("TWEET")]
        void Tweet(ReceivedMessage message)
        {
            const string prefix = ": ";

            if (!tweetNext ||
                message.Target.Length == 0 || message.Target[0] != '#' ||
                message.Text.Length < Twitter.MaxLength / 5 ||
                message.Text[0] == '\x01' ||
                message.Text.Length + message.User.Nick.Length + prefix.Length > Twitter.MaxLength)
                return;

            tweetNext = false;

            string msg = string.Concat(message.User.Nick, prefix, message.Text);
            string id = Twitter.Update(TwitterUsername, password, msg);

            if (string.IsNullOrEmpty(id))
                return;

            tweetLast = message.User;
            tweetId = id;

            var rand = new Random();
            string resp = string.Concat(tweetResponses[rand.Next(0, tweetResponses.Length - 1)],
                " ", message.User.Nick, ", tweeted that ", TwitterStatusPrefix, id, " - type !tdelete to remove.");

            Message(message.Target, resp);
        }
    }
}
