using AGVROSEmulator.IOModule;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AGVROSEmulator.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DemoInspectionAGVIOController : ControllerBase
    {

        private DemoRoomInspectionAGVIOModule IOModule => (DemoRoomInspectionAGVIOModule)Utility.IOModule;

        [HttpPost("SetBat1LockSensorState")]
        public async Task SetBat1LockSensorState(bool state)
        {
            IOModule.SetBat1LockState(state);
        }

        [HttpPost("SetBat2LockSensorState")]
        public async Task SetBat2LockSensorState(bool state)
        {
            IOModule.SetBat2LockSensorState(state);
        }

        [HttpPost("SetBat1UnlockSensorState")]
        public async Task SetBat1UnlockSensorState(bool state)
        {
            IOModule.SetBat1UnlockState(state);
        }

        [HttpPost("SetBat2UnlockSensorState")]
        public async Task SetBat2UnlockSensorState(bool state)
        {
            IOModule.SetBat2UnlockSensorState(state);
        }
    }
}
