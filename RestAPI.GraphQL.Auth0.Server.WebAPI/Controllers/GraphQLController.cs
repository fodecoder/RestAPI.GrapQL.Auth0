using Common.Services.Requests;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;

namespace RestAPI.GraphQL.Auth0.Server.WebAPI.Controllers
{
    //[ApiController]
    //[Route ( "[controller]" )]
    public sealed class GraphQLController : Controller
    {
        private readonly ISchema _schema;
        private readonly IDocumentExecuter _documentExecuter;

        public GraphQLController( ISchema schema , IDocumentExecuter documentExecuter )
        {
            _schema = schema;
            _documentExecuter = documentExecuter;
        }

        public async Task<IActionResult> Post( [FromBody] GraphQLRequest graphQlRequest )
        {
            ExecutionOptions executionOptions = new ExecutionOptions ()
            {
                Schema = _schema ,
                Query = graphQlRequest.Query ,
                Variables = graphQlRequest.Variables?.ToInputs ()
            };

            ExecutionResult executionResult = await _documentExecuter.ExecuteAsync ( executionOptions );

            if (executionResult.Errors != null)
                return BadRequest ( executionResult );

            return Ok ( executionResult );
        }
    }
}
