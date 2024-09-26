using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AGVROSEmulator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatteryStateController : ControllerBase
    {


        [HttpPost("ErrorCode")]
        public async Task<IActionResult> SetErrorCode(byte errorCode)
        {
            Utility.AGVEmulator.moduleInfo.Battery.errorCode = errorCode;
            return Ok();
        }
    }
}
