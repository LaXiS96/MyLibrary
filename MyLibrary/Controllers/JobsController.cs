using LaXiS.MyLibrary.Jobs;
using Microsoft.AspNetCore.Mvc;

namespace LaXiS.MyLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly JobManager _jobManager;

        public JobsController(
            JobManager jobManager)
        {
            _jobManager = jobManager;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_jobManager.GetLastJobStatus());
        }

        [HttpPost("Scan")]
        public IActionResult Scan()
        {
            _jobManager.Launch<ScanJob>();

            return Accepted();
        }
    }
}
