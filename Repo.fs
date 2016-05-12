module Repo

//EmbeddableDocumentStore store = new EmbeddableDocumentStore
//{
//	DataDirectory = "Data"
//};
Raven.Database.Server.NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8080);
open Raven.Client.Embedded

let testRavenDb () =
    // TODO: dispose
    let documentStore = new EmbeddableDocumentStore(DataDirectory="..\..\Data", UseEmbeddedHttpServer=true)
    documentStore.Initialize ()

let GetMinId () =
    None

let GetMaxId () =
    None

let Add tweets =
    Array.length tweets