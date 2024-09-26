using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AGVROSEmulator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleInfoController : ControllerBase
    {
        [HttpPost("SetTagReader")]
        public async Task SetTagReader(uint tagID)
        {
            Utility.AGVEmulator.SetTagReader(tagID);
        }
    }
}
