﻿namespace AGVROSEmulator.IOModule
{
    public class IOMap
    {
        public enum SUBMARINE_IOMAP_OUTPUTS
        {
            EMU_EQ_L_REQ = 0x0000,
            EMU_EQ_U_REQ = 0x0001,
            EMU_EQ_READY = 0x0003,
            EMU_EQ_UP_READY = 0x0004,
            EMU_EQ_LOW_READY = 0x0005,
            EMU_EQ_BUSY = 0x0006,
            Recharge_Circuit = 0x0008,
            Safety_Relays_Reset = 0x0009,
            Horizon_Motor_Stop = 0x000A,
            Horizon_Motor_Free = 0x000B,
            Horizon_Motor_Reset = 0x000C,
            Front_LsrBypass = 0x0010,
            Back_LsrBypass = 0x0011,
            Left_LsrBypass = 0x0012,
            Right_LsrBypass = 0x0013,
            AGV_DiractionLight_Front = 0x0016,
            AGV_DiractionLight_Back = 0x0017,
            AGV_DiractionLight_R = 0x0018,
            AGV_DiractionLight_Y = 0x0019,
            AGV_DiractionLight_G = 0x001A,
            AGV_DiractionLight_B = 0x001B,
            AGV_DiractionLight_Left = 0x001C,
            AGV_DiractionLight_Right = 0x001D,
            AGV_VALID = 0x0020,
            AGV_READY = 0x0023,
            AGV_TR_REQ = 0x0024,
            AGV_BUSY = 0x0025,
            AGV_COMPT = 0x0026,
            TO_EQ_Low = 0x0028,
            TO_EQ_Up = 0x0029,
            CMD_reserve_Up = 0x002A,
            CMD_reserve_Low = 0x002B,
            Front_Protection_Sensor_IN_1 = 0x0030,
            Front_Protection_Sensor_CIN_1 = 0x0031,
            Front_Protection_Sensor_IN_2 = 0x0032,
            Front_Protection_Sensor_CIN_2 = 0x0033,
            Front_Protection_Sensor_IN_3 = 0x0034,
            Front_Protection_Sensor_CIN_3 = 0x0035,
            Front_Protection_Sensor_IN_4 = 0x0036,
            Front_Protection_Sensor_CIN_4 = 0x0037,
            Back_Protection_Sensor_IN_1 = 0x0038,
            Back_Protection_Sensor_CIN_1 = 0x0039,
            Back_Protection_Sensor_IN_2 = 0x003A,
            Back_Protection_Sensor_CIN_2 = 0x003B,
            Back_Protection_Sensor_IN_3 = 0x003C,
            Back_Protection_Sensor_CIN_3 = 0x003D,
            Back_Protection_Sensor_IN_4 = 0x003E,
            Back_Protection_Sensor_CIN_4 = 0x003F
        }
        public enum SUBMARINE_IOMAP_INPUTS
        {
            EMO = 0x0008,
            Bumper_Sensor = 0x000A,
            Panel_Reset_PB = 0x000B,
            Horizon_Motor_Switch = 0x000C,
            Vertical_Motor_Switch = 0x000D,
            LeftProtection_Area_Sensor_3 = 0x000E,
            RightProtection_Area_Sensor_3 = 0x000F,
            Horizon_Motor_Alarm_1 = 0x0014,
            Horizon_Motor_Busy_1 = 0x0015,
            Horizon_Motor_Alarm_2 = 0x0016,
            Horizon_Motor_Busy_2 = 0x0017,
            Vertical_Motor_Alarm = 0x0018,
            Vertical_Motor_Busy = 0x0019,
            EQ_L_REQ = 0x0020,
            EQ_U_REQ = 0x0021,
            EQ_READY = 0x0023,
            EQ_UP_READY = 0x0024,
            EQ_LOW_READY = 0x0025,
            EQ_BUSY = 0x0026,
            EQ_GO = 0x0028,
            Cst_Sensor_1 = 0x002D,
            Cst_Sensor_2 = 0x002E,
            FrontProtection_Obstacle_Sensor = 0x002F,
            FrontProtection_Area_Sensor_1 = 0x0030,
            FrontProtection_Area_Sensor_2 = 0x0031,
            FrontProtection_Area_Sensor_3 = 0x0032,
            FrontProtection_Area_Sensor_4 = 0x0033,
            BackProtection_Area_Sensor_1 = 0x0034,
            BackProtection_Area_Sensor_2 = 0x0035,
            BackProtection_Area_Sensor_3 = 0x0036,
            BackProtection_Area_Sensor_4 = 0x0037,
        }


        public enum DEMO_INSPECTION_AGV_IOMAP_OUTPUTS 
        {
            FrontLsrBypass = 0x0000,
            BackLsrBypass = 0x0001,
            LeftLsrBypass = 0x0002,
            RightLsrBypass = 0x0003,
            SafetyRelaysReset = 0x0004,
            AGVDiractionLightR = 0x0008,
            AGVDiractionLightY = 0x0009,
            AGVDiractionLightG = 0x000A,
            AGVDiractionLightB = 0x000B,
            AGVDiractionLightLeft = 0x000C,
            AGVDiractionLightRight = 0x000D,
            AGVVALID = 0x0010,
            AGVLREQ = 0x0011,
            AGVUREQ = 0x0012,
            AGVREADY = 0x0013,
            AGVCS0 = 0x0014,
            AGVCS1 = 0x0015,
            AGVCheckREQ = 0x0016,
            HorizonMotorStop = 0x001A,
            LeftProtectionSensorIN1 = 0x001C,
            LeftProtectionSensorIN2 = 0x001D,
            LeftProtectionSensorIN3 = 0x001E,
            LeftProtectionSensorIN4 = 0x001F,
            HorizonMotorFree = 0x0020,
            Battery1ElectricityInterrupt = 0x0021,
            Battery2ElectricityInterrupt = 0x0022,
            InfraredDoor1 = 0x0023,
            InfraredPW2 = 0x0024,
            InfraredPW1 = 0x0025,
            InfraredPW0 = 0x0026,
            N2Open = 0x0027,
            FrontProtectionSensorIN1 = 0x0028,
            FrontProtectionSensorCIN1 = 0x0029,
            FrontProtectionSensorIN2 = 0x002A,
            FrontProtectionSensorCIN2 = 0x002B,
            FrontProtectionSensorIN3 = 0x002C,
            FrontProtectionSensorCIN3 = 0x002D,
            FrontProtectionSensorIN4 = 0x002E,
            FrontProtectionSensorCIN4 = 0x002F,
            BackProtectionSensorIN1 = 0x0030,
            BackProtectionSensorCIN1 = 0x0031,
            BackProtectionSensorIN2 = 0x0032,
            BackProtectionSensorCIN2 = 0x0033,
            BackProtectionSensorIN3 = 0x0034,
            BackProtectionSensorCIN3 = 0x0035,
            BackProtectionSensorIN4 = 0x0036,
            BackProtectionSensorCIN4 = 0x0037
        }


        public enum DEMO_INSPECTION_AGV_IOMAP_INPUTS 
        {
            FrontProtectionAreaSensor1 = 0x0000,
            FrontProtectionAreaSensor2 = 0x0001,
            FrontProtectionAreaSensor3 = 0x0002,
            FrontProtectionAreaSensor4 = 0x0003,
            BackProtectionAreaSensor1 = 0x0004,
            BackProtectionAreaSensor2 = 0x0005,
            BackProtectionAreaSensor3 = 0x0006,
            BackProtectionAreaSensor4 = 0x0007,

            Battery2Exist1 = 0x0010,
            Battery2Exist2 = 0x0011,
            N2Sensor = 0x0013,
            PanelResetPB = 0x0015,
            HorizonMotorSwitch = 0x0016,
            Battery1Exist1 = 0x0017,
            Battery1Exist2 = 0x0018,
            EMOButton = 0x0019,
            EMOButton2 = 0x001A,
            SMSError = 0x001B,
            Battery2LockSensor = 0x001C,
            Battery2UnlockSensor = 0x001D,
            Battery1LockSensor = 0x001E,
            Battery1UnlockSensor = 0x001F,
            HorizonMotorAlarm1 = 0x0020,
            HorizonMotorAlarm2 = 0x0021,
            HorizonMotorAlarm3 = 0x0022,
            HorizonMotorAlarm4 = 0x0023,
            FrontRightUltrasoundSensor = 0x0024,
            BackLeftUltrasoundSensor = 0x0025,
            LeftProtectionAreaSensor1 = 0x0028,
            LeftProtectionAreaSensor2 = 0x0029,
            LeftProtectionAreaSensor3 = 0x002A,
            RightProtectionAreaSensor1 = 0x002C,
            RightProtectionAreaSensor2 = 0x002D,
            RightProtectionAreaSensor3 = 0x002E,
            EQGO = 0x0030,
            EQVALID = 0x0031,
            EQTRREQ = 0x0032,
            EQBUSY = 0x0033,
            EQCOMPT = 0x0034,
            EQCheckResult = 0x0035,
            EQCheckReady = 0x0036,
            Battery2Exist3 = 0x0039,
            Battery2Exist4 = 0x003A,
            Battery1Exist3 = 0x003B,
            Battery1Exist4 = 0x003C,
            SmokeSensor1 = 0x003D,
            SaftyPLCOutput = 0x0040,
            BumperSensor = 0x0041
        }

    }
}
