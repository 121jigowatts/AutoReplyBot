using CoreTweet;
using System;

namespace AutoReplyBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var APIKey = args[0];
            var APISecret = args[1];
            var AccessToken = args[2];
            var AccessTokenSecret = args[3];
            try
            {
                var t = Tokens.Create(APIKey
                    , APISecret
                    , AccessToken
                    , AccessTokenSecret);
                var replyer = new Replyer(t);
                replyer.Observe();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");
                throw;
            }

            Console.WriteLine($"[{DateTime.Now.ToString()}] 処理開始");
            Console.ReadKey(true);
        }
    }
}
