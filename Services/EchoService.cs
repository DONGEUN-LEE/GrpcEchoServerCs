using Grpc.Core;

namespace GrpcEchoServer.Services;

public class EchoService : Echo.EchoBase
{
    private readonly ILogger<EchoService> _logger;
    public EchoService(ILogger<EchoService> logger)
    {
        _logger = logger;
    }

    public override Task<EchoResponse> UnaryEcho(EchoRequest request, ServerCallContext context)
    {
        Console.WriteLine($"unary echoing message {request.Message}");
        return Task.FromResult(new EchoResponse
        {
            Message = request.Message
        });
    }

    public override async Task BidirectionalStreamingEcho(IAsyncStreamReader<EchoRequest> requestStream,
        IServerStreamWriter<EchoResponse> responseStream, ServerCallContext context)
    {
        var contextCancellationToken = context.CancellationToken;

        try
        {
            await foreach (var request in requestStream.ReadAllAsync())
            {
                if (contextCancellationToken.IsCancellationRequested) return;

                Console.WriteLine($"bidi echoing message {request.Message}");
                await responseStream.WriteAsync(new EchoResponse
                {
                    Message = request.Message
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"server: error receiving from stream: {ex.Message}");
        }
    }
}
