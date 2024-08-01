using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Behaviour
{
    public class LoggingBehaviour<TRequest, TResponse>(ILogger<LoggingBehaviour<TRequest, TResponse>> logger) 
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull,IRequest<TResponse>
        where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("[Start] Handle Request = {Request}-- Response ={response} -- RequestData={RequestData}",typeof(TRequest).Name,typeof(TResponse).Name,request);

            var timer = new Stopwatch();
            timer.Start();
            var response = await next();
            timer.Stop();
            if (timer.Elapsed.Seconds > 3) {
                logger.LogWarning("[Performance] - The Request ={Request} took {timeTaken}", typeof(TRequest).Name, timer.Elapsed.Seconds);
            
            }

            logger.LogInformation("[END]  Handled ={Request} With  Response ={response} ", typeof(TRequest).Name, typeof(TResponse).Name);
            return response;
        }
    }
}
