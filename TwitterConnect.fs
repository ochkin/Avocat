module TwitterConnect

type Config = FSharp.Configuration.AppSettings<"app.config">
open FSharp.Data.Toolbox.Twitter
let twitter = Twitter.AuthenticateAppOnly(Config.ConsumerKey, Config.ConsumerSecret)

let Test () =
    let query = "fintech"
    for status in twitter.Search.Tweets(query, count=10, lang="en").Statuses do
        printfn "@%s: %s" status.User.ScreenName status.Text

let load sinceId maxId =
    let query = "fintech"
    let pageSize = 100 // max = 800
    let tweets =
        match sinceId, maxId with
            | None, None -> twitter.Search.Tweets(query, count=pageSize, lang="en")
            | None, Some mId -> twitter.Search.Tweets(query, count=pageSize, lang="en", maxId=mId)
            | Some sId, None -> twitter.Search.Tweets(query, count=pageSize, lang="en", sinceId=sId)
            | Some sId, Some mId-> twitter.Search.Tweets(query, count=pageSize, lang="en", sinceId=sId, maxId=mId)
    tweets.Statuses

type Tweets = FSharp.Data.JsonProvider<Sample="json/search_tweets.json", EmbeddedResource="FSharp.Data.Toolbox.Twitter,search_tweets.json">

