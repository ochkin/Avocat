module Repo

//EmbeddableDocumentStore store = new EmbeddableDocumentStore
//{
//	DataDirectory = "Data"
//};

// TODO: dispose
let documentStore = new Raven.Client.Embedded.EmbeddableDocumentStore(DataDirectory="Data")

let GetMinId () =
    None

let GetMaxId () =
    None

let Add tweets =
    Array.length tweets