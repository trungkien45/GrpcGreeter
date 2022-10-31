// See https://aka.ms/new-console-template for more information
using System.Threading;
using System.Threading.Tasks;
using ConsoleApp1;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;

Console.WriteLine("Hello, World!");
// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("http://localhost:5168");
var client = new Greeter.GreeterClient(channel);
var reply = await client.SayHelloAsync(
                  new HelloRequest { Name = "GreeterClient" });
Console.WriteLine("Greeting: " + reply.Message);
var rep = await client.ABCAsync(new abc { A = 4 });
Console.WriteLine("Greeting: " + rep.Cc);

var client3 = new Greeter3.Greeter3Client(channel);
var reply3 = await client3.SayHelloAsync(
                  new HelloRequest3 { Name = "GreeterClient" });
Console.WriteLine("Greeting: " + reply3.Message);
var a = new abc3() { A = { 1, 2, 3 } };

var rep3
    = await client3.ABCAsync(a);
Console.WriteLine("Greeting: " + rep3.Cc);

var client4 = new Greeter4.Greeter4Client(channel);
var cts = new CancellationToken();
DownloadRequest downloadRequest = new DownloadRequest();
downloadRequest.FileId = "a";
var response = client4.DownloadFile(downloadRequest, cancellationToken: cts);
Stream fs = File.OpenWrite(@"abc.jpg");
await foreach (var chunkMsg in response.ResponseStream.ReadAllAsync().ConfigureAwait(false))
{
    fs.Write(chunkMsg.FileChunk.ToByteArray());
}

byte[] abc = new byte[3] { 1, 2, 3 };
var call  = client4.UploadFile();
await call.RequestStream.WriteAsync(new DownloadResponse { FileChunk = ByteString.CopyFrom(abc) });
await call.RequestStream.CompleteAsync();

var callS = client4.Stremming();
byte[] abc4 = new byte[3] { 1, 2, 4 };

await callS.RequestStream.WriteAsync(new DownloadResponse { FileChunk = ByteString.CopyFrom(abc4) });
await callS.RequestStream.CompleteAsync();

await foreach (var chunkMsg in callS.ResponseStream.ReadAllAsync().ConfigureAwait(false))
{
    var arr = chunkMsg.FileChunk.ToByteArray();
}


//var p= response.FileChunk.Memory.ToArray();
Console.WriteLine("Press any key to exit...");
Console.ReadKey();

