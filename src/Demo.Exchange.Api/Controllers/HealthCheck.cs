namespace Demo.Exchange.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheck : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetHealth()
            => await Task.Run(() => Ok($"Jaeger Open Tracing ddrsdiego API Example => {DateTime.UtcNow:o}"));
    }
}