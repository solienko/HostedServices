using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiAndHostedServices.Services;

namespace WebApiAndHostedServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueController : ControllerBase
    {
        private readonly ILogger _logger;

        public QueueController(IBackgroundTaskQueue queue, ILogger<QueueController> logger)
        {
            Queue = queue;
            _logger = logger;
        }

        public IBackgroundTaskQueue Queue { get; }

        [HttpPost("add")]
        public IActionResult AddWorkItem()
        {
            Queue.QueueBackgroundWorkItem(async token =>
            {
                var guid = Guid.NewGuid().ToString();

                for (int delayLoop = 0; delayLoop < 3; delayLoop++)
                {
                    _logger.LogInformation($"Queued Background Task {guid} is running. {delayLoop}/3");
                    await Task.Delay(TimeSpan.FromSeconds(5), token);
                }

                _logger.LogInformation($"Queued Background Task {guid} is complete. 3/3");
            });

            return Ok();
        }
    }
}