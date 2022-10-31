using Google.Protobuf;
using Grpc.Core;
using GrpcGreeter.Protos;
using System.Linq;

namespace GrpcGreeter.Services
{
    public class Greeter4Service : Greeter4.Greeter4Base
    {
        private readonly ILogger<Greeter4Service> _logger;
        public Greeter4Service(ILogger<Greeter4Service> logger)
        {
            _logger = logger;
        }
        public override async Task DownloadFile(DownloadRequest request, IServerStreamWriter<DownloadResponse> responseStream, ServerCallContext context)
        {
            var filePath = @"C:\Users\kien.nguyen2\Pictures\g.jpg";
            using var fs = File.Open(filePath, System.IO.FileMode.Open);
            int fileChunkSize = 64 * 1024;//64KB

            int bytesRead;
            var buffer = new byte[fileChunkSize];
            while ((bytesRead = await fs.ReadAsync(buffer)) > 0)
            {
                await responseStream.WriteAsync(new DownloadResponse
                {
                    // Here the correct number of bytes must be sent which is starting from
                    // index 0 up to the number of read bytes from the file stream.
                    // If you solely pass 'buffer' here, the same bug would be present.
                    FileChunk = ByteString.CopyFrom(buffer[0..bytesRead]),
                });
            }
        }
        public override async Task<DownloadRequest> UploadFile(IAsyncStreamReader<DownloadResponse> requestStream, ServerCallContext context)
        {
            await foreach (var message in requestStream.ReadAllAsync())
            {
                byte[]? a = requestStream.Current?.FileChunk?.ToArray();
            }
            return new DownloadRequest();
        }
        public override async Task Stremming(IAsyncStreamReader<DownloadResponse> requestStream, IServerStreamWriter<DownloadResponse> responseStream, ServerCallContext context)
        {
            await foreach (var message in requestStream.ReadAllAsync())
            {
                byte[]? a = requestStream.Current?.FileChunk?.ToArray().Append((byte)100).ToArray();
                await responseStream.WriteAsync(new DownloadResponse { FileChunk = ByteString.CopyFrom(a) });
            }
            return;
        }
    }
}
