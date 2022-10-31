using Grpc.Core;
using GrpcGreeter.Protos;
using System.Linq;
namespace GrpcGreeter.Services
{
    public class Greeter3Service : Greeter3.Greeter3Base
    {
        private readonly ILogger<Greeter3Service> _logger;
        public Greeter3Service(ILogger<Greeter3Service> logger)
        {
            _logger = logger;
        }

        public override Task<Hello3Reply> SayHello(Hello3Request request, ServerCallContext context)
        {
            return Task.FromResult(new Hello3Reply
            {
                Message = "xxx " + request.Name
            });
        }
        public override Task<xyz3> ABC(abc3 request, ServerCallContext context)
        {
            return Task.FromResult(new xyz3 { Cc = request.A.Sum() + 9 });
        }
    }
}