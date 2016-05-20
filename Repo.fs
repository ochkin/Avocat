module Repo


open Database
let DefaultIfEmpty (d:'t) (l:'t seq) = 
    seq{
        use en = l.GetEnumerator()
        if en.MoveNext() then 
            yield en.Current
            while en.MoveNext() do
                yield en.Current 
        else
            yield d }
let mutable minId:int64 option =
    use connection = Database.Store.OpenSession()
    let qry = connection.Query<Database.MyTweet>()
    qry |> Seq.map (fun tweet -> Some tweet.Id) |> DefaultIfEmpty None |> Seq.min
let mutable maxId:int64 option =
    use connection = Database.Store.OpenSession()
    let qry = connection.Query<Database.MyTweet>()
    qry |> Seq.map (fun tweet -> Some tweet.Id) |> DefaultIfEmpty None |> Seq.max


let GetMinId () =
    minId

let GetMaxId () =
    maxId

let Add tweets =
    if Array.isEmpty tweets then
        false
    else
        let newMinId = Seq.min <| Seq.map (fun (t:Database.MyTweet) -> t.Id) tweets
        minId <- match minId with
                    | Some id -> Some <| min id newMinId
                    | None -> Some newMinId
        let newMaxId = Seq.max <| Seq.map (fun (t:Database.MyTweet) -> t.Id) tweets
        maxId <- match maxId with
                    | Some id -> Some <| max id newMaxId
                    | None -> Some newMaxId
        use connection = Database.Store.OpenSession ()
        for tweet in tweets do
            connection.Store tweet
        connection.SaveChanges ()
        true