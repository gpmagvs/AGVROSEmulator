using AGVROSEmulator.IOModule;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AGVROSEmulator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IOController : ControllerBase
    {
        private WagoIOModule IOModule => Utility.IOModule;
        public IOController()
        {
        }


        [HttpPost("EMOButtonPush")]
        public async Task<IActionResult> EMOButtonPush()
        {
            IOModule.EmoButtonPush();
            return Ok();
        }



        [HttpPost("EMOButtonRelease")]
        public async Task<IActionResult> EMOButtonRelease()
        {
            IOModule.EMOButtonRelease();
            return Ok();
        }



        [HttpPost("Bumper")]
        public async Task<IActionResult> Bumper(bool pressed)
        {
            IOModule.Bumper(pressed);
            return Ok();
        }

        [HttpPost("MotorSwitch")]
        public async Task<IActionResult> MotorSwitch(bool opened)
        {
            IOModule.MotorSwitch(opened);
            return Ok();
        }


        [HttpPost("ResetButtonPush")]
        public async Task<IActionResult> ResetButtonPush()
        {
            IOModule.ResetButtonPush();
            return Ok();
        }

        [HttpPost("MotorAlarm")]
        public async Task<IActionResult> MotorAlarm()
        {
            IOModule.MotorsAlarm();
            return Ok();
        }

        [HttpPost("FrontLaserArea3Trigger")]
        public async Task<IActionResult> FrontLaserArea3Trigger()
        {
            IOModule.FrontLaserArea3Trigger();
            return Ok();
        }

        [HttpPost("AllLaserAreaNoTrigger")]
        public async Task<IActionResult> AllLaserAreaNoTrigger()
        {
            IOModule.AllLaserAreaNoTrigger();
            return Ok();
        }



        /// <summary>
        /// 模擬EQ GO訊號中斷
        /// </summary>
        /// <returns></returns>
        [HttpPost("EQ_GO_Flicker")]
        public async Task EQ_GO_Flicker(int flick_time_ms = 100)
        {
            IOModule.SetState(IOMap.SUBMARINE_IOMAP_INPUTS.EQ_GO, false);
            await Task.Delay(flick_time_ms);
            IOModule.SetState(IOMap.SUBMARINE_IOMAP_INPUTS.EQ_GO, true);
        }



        [HttpPost("E84_EQ_L_REQ")]
        public async Task E84_EQ_L_REQ(bool state)
        {
            IOModule.SetState(IOMap.SUBMARINE_IOMAP_INPUTS.EQ_L_REQ, state);
        }

        [HttpPost("E84_EQ_READY")]
        public async Task E84_EQ_READY(bool state)
        {
            IOModule.SetState(IOMap.SUBMARINE_IOMAP_INPUTS.EQ_READY, state);
        }

        [HttpPost("E84_EQ_L_REQ_Flick")]
        public async Task E84_EQ_L_REQ_Flick()
        {
            IOModule.SetState(IOMap.SUBMARINE_IOMAP_INPUTS.EQ_L_REQ, false);
            await Task.Delay(500);

            IOModule.SetState(IOMap.SUBMARINE_IOMAP_INPUTS.EQ_L_REQ, true);
            await Task.Delay(50);
            IOModule.SetState(IOMap.SUBMARINE_IOMAP_INPUTS.EQ_L_REQ, false);
            await Task.Delay(50);
            IOModule.SetState(IOMap.SUBMARINE_IOMAP_INPUTS.EQ_L_REQ, true);
        }



        [HttpPost("EQ_Donw_Simualtion")]
        public async Task EQ_Donw_Simualtion()
        {
            IOModule.EQDownSimulation();
        }


        [HttpPost("Handshake_Signals_Reset")]
        public async Task Handshake_Signals_Reset()
        {
            IOModule.Handshake_Signals_Reset();
        }
    }
}
