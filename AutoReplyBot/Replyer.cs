using CoreTweet;
using CoreTweet.Streaming;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Text.RegularExpressions;

namespace AutoReplyBot
{
    public class Replyer
    {
        public Tokens Tokens { get; private set; }
        public Replyer(Tokens tokens)
        {
            Tokens = tokens;
        }

        public void Observe()
        {
            var observable = Tokens.Streaming.UserAsObservable();

            observable.Catch(
                    observable.DelaySubscription(
                        TimeSpan.FromSeconds(10)
                        ).Retry()
                )
                .Repeat()
                .Where((StreamingMessage m) => m.Type == MessageType.Create)
                .Cast<StatusMessage>()
                .Select((StatusMessage m) => m.Status)
                .Subscribe(
                    Next,
                    (Exception ex) => Console.WriteLine(ex),
                    () => Console.WriteLine("終点")
                );
        }
        public void Next(Status status)
        {
            var createdAt = status.CreatedAt.LocalDateTime;
            var screenName = status.User.ScreenName;
            var text = status.Text;
            var id = status.Id;

            Console.WriteLine($"[{createdAt}] {screenName}: {text}");

            var user = "USER_NAME";
            var pattern = $@"@{user}\s.*Gigawatts.*";

            if (Regex.IsMatch(text, pattern))
            {
                var reply = $@"@{screenName} Jigowatts!";

                Console.WriteLine($"{reply}");
                Tokens.Statuses.Update(
                    new
                    {
                        status = reply,
                        in_reply_to_status_id = id
                    });

            }
        }

    }
}
