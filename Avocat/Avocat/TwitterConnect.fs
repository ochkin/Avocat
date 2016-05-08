module TwitterConnect


open FSharp.Configuration
type Config = AppSettings<"app.config">

open FSharp.Data.Toolbox.Twitter
let Test () =
    let twitter = Twitter.AuthenticateAppOnly(Config.ConsumerKey, Config.ConsumerSecret)
    for status in twitter.Search.Tweets("fintech", count=10).Statuses do
        printfn "@%s: %s" status.User.ScreenName status.Text



