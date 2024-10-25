namespace Common.Services.Middlewares
{
    public class GraphQLMiddleware
    {
        //public RequestDelegate _next { get; private set; }
        //public IDocumentExecuter _executor { get; private set; }
        //public GraphQLOptions _options { get; private set; }

        //public GraphQLMiddleware( RequestDelegate next , IDocumentExecuter executor , IOptions<GraphQLOptions> options )
        //{
        //    _next = next;
        //    _executor = executor;
        //    _options = options.Value;
        //}

        //public async Task InvokeAsync( HttpContext httpContext , ISchema schema , IServiceProvider serviceProvider )
        //{
        //    if (httpContext.Request.Path.StartsWithSegments ( _options.EndPoint ) && string.Equals ( httpContext.Request.Method , "POST" , StringComparison.OrdinalIgnoreCase ))
        //    {
        //        var request = await JsonSerializer
        //                                .DeserializeAsync<GraphQLRequest> (
        //                                    httpContext.Request.Body ,
        //                                    new JsonSerializerOptions
        //                                    {
        //                                        PropertyNameCaseInsensitive = true
        //                                    } );

        //        var result = await _executor
        //                        .ExecuteAsync ( doc =>
        //                        {
        //                            doc.Schema = schema;
        //                            doc.Query = request.Query;
        //                            doc.Variables = request.Variables.ToInputs ();
        //                        } ).ConfigureAwait ( false );

        //        httpContext.Response.ContentType = "application/json";
        //        httpContext.Response.StatusCode = 200;

        //        await JsonSerializer.SerializeAsync ( httpContext.Response.Body , result );
        //    }
        //    else
        //    {
        //        await _next ( httpContext );
        //    }
        //    await _next ( httpContext );
        //}
    }
}
