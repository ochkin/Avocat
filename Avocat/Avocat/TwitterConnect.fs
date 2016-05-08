module TwitterConnect

let ConsumerKey = "038R2o2jOR36S3A5s3gQogets"
let ConsumerSecret = "JBy1YXVNGwVutDXkR8OV1gXWSpCFldqNMcPjh4l2bu8A81uT0D"
//let Owner = "Sergey_Eva"
//let OwnerId = "726988546104422401"
//
//let Host = "api.twitter.com"
//let tokenUri = "/oauth2/token"

open FSharp.Data.Toolbox.Twitter
let Test () =
    let twitter = Twitter.AuthenticateAppOnly(ConsumerKey, ConsumerSecret)
    for status in twitter.Search.Tweets("fintech", count=10).Statuses do
        printfn "@%s: %s" status.User.ScreenName status.Text



