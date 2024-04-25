using AGVROSEmulator.GPMRosMessageNet.Services;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.Actionlib;
using RosSharp.RosBridgeClient.MessageTypes.Actionlib;
using RosSharp.RosBridgeClient.MessageTypes.Geometry;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AGVROSEmulator.GPMRosMessageNet.Actions
{
    public class TaskCommandActionServer : ActionServer<TaskCommandAction, TaskCommandActionGoal, TaskCommandActionResult, TaskCommandActionFeedback, TaskCommandGoal, TaskCommandResult, TaskCommandFeedback>
    {
        public enum GUID_MODE
        {
            SLAM = 0, TAP = 1
        }
        public int lastVisitedTag
        {
            get => _lastVisitedTag;
            set
            {
                if (_lastVisitedTag != value)
                {
                    _lastVisitedTag = value;
                    IsAtChargeStation = ChargeStationTags.Contains(value);
                }
            }
        }
        private int _lastVisitedTag = 36;
        public int barcodeReadTag = 36;
        private int destineTag = 36;
        private int lastPathEndTag = 36;
        public double currentAngle = 90;
        public bool isRunning = false;
        public List<int> ChargeStationTags { get; private set; } = new List<int>();

        private bool _IsAtChargeStation = false;
        public bool IsAtChargeStation
        {
            get => _IsAtChargeStation;
            set
            {
                if (_IsAtChargeStation != value)
                {
                    _IsAtChargeStation = value;
                    if (_IsAtChargeStation)
                        OnAGVParkAtChargeStation?.Invoke(this, null);
                    else
                        OnAGVLeaveChargeStation?.Invoke(this, null);
                }
            }
        }

        public event EventHandler OnAGVParkAtChargeStation;
        public event EventHandler OnAGVLeaveChargeStation;
        private ManualResetEvent taskWait = new ManualResetEvent(true);
        private GUID_MODE guildTypeUsing = GUID_MODE.SLAM;
        private bool CycleStopFlag = false;
        private bool EmergecyStopFlag = false;
        public RosSharp.RosBridgeClient.MessageTypes.Geometry.Point currentCoordination = new RosSharp.RosBridgeClient.MessageTypes.Geometry.Point(13.088, -2.408, 0);
        public Quaternion orientation => ToQuaternion(currentAngle);
        private bool _replan_flag = false;
        /// <summary>
        /// 將角度值轉換為 Quaternion(四位元)
        /// </summary>
        /// <param name="Theta"></param>
        /// <returns></returns>
        private Quaternion ToQuaternion(double Theta)
        {
            double yaw_radians = (float)Theta * Math.PI / 180.0;
            double cos_yaw = Math.Cos(yaw_radians / 2.0);
            double sin_yaw = Math.Sin(yaw_radians / 2.0);
            return new Quaternion(0.0f, 0.0f, (float)sin_yaw, (float)cos_yaw);
        }
        private double ToTheta(Quaternion orientation)
        {
            double yaw;
            double x = orientation.x;
            double y = orientation.y;
            double z = orientation.z;
            double w = orientation.w;
            // 計算角度
            double siny_cosp = 2.0 * (w * z + x * y);
            double cosy_cosp = 1.0 - 2.0 * (y * y + z * z);
            yaw = Math.Atan2(siny_cosp, cosy_cosp);
            return yaw * 180.0 / Math.PI;
        }
        public TaskCommandActionServer(string actionName, RosSocket rosSocket)
        {
            this.actionName = actionName;
            this.rosSocket = rosSocket;
            action = new TaskCommandAction();
            rosSocket.AdvertiseService<ComplexRobotControlCmdRequest, ComplexRobotControlCmdResponse>("/complex_robot_control_cmd", ComplexRobotControlCallBack);
            rosSocket.Subscribe<Twist>("/cmd_vel", CmdVelCallBack);
        }

        private void CmdVelCallBack(Twist t)
        {
        }

        private bool ComplexRobotControlCallBack(ComplexRobotControlCmdRequest tin, out ComplexRobotControlCmdResponse tout)
        {
            var cmd = tin.reqsrv;
            if (cmd == 100)
            {
                CycleStopFlag = true;
            }
            tout = new ComplexRobotControlCmdResponse()
            {
                confirm = true,
            };
            return true;
        }
        protected override void OnGoalActive()
        {
        }

        protected override void OnGoalPreempting()
        {
        }

        protected override void OnGoalRecalling(GoalID goalID)
        {
        }
        protected override void OnGoalSucceeded()
        {
            base.OnGoalSucceeded();
        }
        private List<Point> PointsToAppend = new List<Point>();
        private bool IsReachGoal => destineTag == (int)lastVisitedTag;
        private Task moveTaskExecution;
        protected override async void OnGoalReceived()
        {
            TaskCommandGoal? goal = this.action.action_goal.goal;

            if (!IsReachGoal && goal.finalGoalID != destineTag)
            {
                //控車生成派車路徑錯誤
            }

            var points = goal.planPath.poses.Select(pose => new Point((int)pose.header.seq, pose.pose.position.x, pose.pose.position.y, ToTheta(pose.pose.orientation))).ToList();

            if (points.Count > 0)
            {
                guildTypeUsing = goal.mobilityModes == 0 ? GUID_MODE.SLAM : GUID_MODE.TAP;
                destineTag = goal.finalGoalID;

                if (moveTaskExecution != null && !moveTaskExecution.IsCompleted)
                {
                    _replan_flag = true;
                    await moveTaskExecution;
                }

                _replan_flag = false;

                var index = points.FindIndex(pt => pt.Tag == lastVisitedTag);

                PointsToAppend = points.Skip(index).ToList();
                lastPathEndTag = points.Count == 0 ? lastPathEndTag : points.Last().Tag;

                UpdateAndPublishStatus(ActionStatus.PENDING);
                UpdateAndPublishStatus(ActionStatus.ACTIVE);
                isRunning = true;
                moveTaskExecution = Movement(PointsToAppend);
            }
            else
            {
                EmergecyStopFlag = isRunning;
                isRunning = _replan_flag = false;
                barcodeReadTag = destineTag = lastPathEndTag = lastVisitedTag;
                taskWait.Set();
                try
                {
                    UpdateAndPublishStatus(ActionStatus.SUCCEEDED);
                }
                catch (Exception ex)
                {
                }
            }
        }


        private Task Movement(List<Point> pointsToAppend)
        {
            return Task.Run(async () =>
            {

                Console.WriteLine($"last visited tag ={lastVisitedTag},path=" + string.Join("->", pointsToAppend.Select(pt => pt.Tag)));

                //0-9 //0 =>  10- =10
                for (int i = 0; i < pointsToAppend.Count; i++)
                {
                    await Task.Delay(1);
                    var next_point = pointsToAppend[i];
                    int remainPointsNum = pointsToAppend.Count - i - 1;
                    bool isLastPoint = remainPointsNum == 0;
                    bool isNextPointEqualCurrentPoint = next_point.Tag == lastVisitedTag;
                    var currentPoint = new Point(currentCoordination.x, currentCoordination.y);
                    if (isNextPointEqualCurrentPoint)
                        continue;

                    var targetAngle = CalculateTargetAngle(currentPoint, next_point);

                    if (guildTypeUsing == GUID_MODE.SLAM)
                        await SimulateRotation(targetAngle);

                    // 模拟直线移动到下一个点
                    await SimulateStraightMove(currentPoint, next_point);

                    if (EmergecyStopFlag)
                    {
                        EmergecyStopFlag = false;
                        Console.WriteLine("Emergency Stop !  Stop Track.");
                        return;
                    }

                    await Task.Delay(30);
                    if (isLastPoint && guildTypeUsing == GUID_MODE.SLAM)
                    {
                        await SimulateRotation(next_point.Theta);
                    }

                    if (CycleStopFlag)
                    {
                        CycleStopFlag = false;
                        Console.WriteLine("Cycle Stop, Stop Track.");
                        break;
                    }

                    if (_replan_flag)
                    {
                        Console.WriteLine("Stop Track because replan.");
                        return;
                    }
                }
                try
                {
                    UpdateAndPublishStatus(ActionStatus.SUCCEEDED);
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    isRunning = false;
                    destineTag = lastPathEndTag = lastVisitedTag;
                }
            });
        }

        public new void UpdateAndPublishStatus(ActionStatus actionStatus, string text = "")
        {
            base.UpdateAndPublishStatus(actionStatus, text);
            Log.Info($"Publish Action Status:{actionStatus}_{text}");
        }

        private double CalculateTargetAngle(Point current, Point next)
        {
            var deltaY = next.Y - current.Y;
            var deltaX = next.X - current.X;
            var angleInRadians = Math.Atan2(deltaY, deltaX);
            return angleInRadians * (180 / Math.PI); // 转换为度
        }
        private async Task SimulateRotation(double targetAngle)
        {

            double delta = targetAngle - currentAngle;
            // 调整角度差为最小路径
            if (delta > 180)
            {
                delta -= 360;
            }
            else if (delta < -180)
            {
                delta += 360;
            }
            double rotated = 0;
            if (Math.Abs(delta) > 10)
                while (rotated <= Math.Abs(delta))
                {
                    currentAngle += 2 * Math.Sign(delta);
                    rotated += 2;
                    if (_replan_flag || EmergecyStopFlag)
                        return;
                    await Task.Delay(10);
                }
            currentAngle = targetAngle; // 确保精确设置目标角度
        }

        private async Task SimulateStraightMove(Point start, Point end)
        {
            double distance = Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2));
            double moveTime = distance / 2; // 1m/s
            double moveStartTime = GetCurrentTimeInSeconds();


            while (GetCurrentTimeInSeconds() - moveStartTime < moveTime)
            {
                // 假设可以实时更新车辆位置
                double timeElapsed = GetCurrentTimeInSeconds() - moveStartTime;
                double fractionOfJourney = timeElapsed / moveTime;
                Point currentPosition = new Point()
                {
                    X = start.X + fractionOfJourney * (end.X - start.X),
                    Y = start.Y + fractionOfJourney * (end.Y - start.Y),
                    Tag = lastVisitedTag
                };
                // 更新车辆位置
                UpdateVehiclePosition(currentPosition);

                if (_replan_flag || EmergecyStopFlag)
                    return;
                await Task.Delay(10);
            }

            UpdateVehiclePosition(end);
        }

        // 假设的辅助方法
        private double GetCurrentTimeInSeconds()
        {
            return (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        private void UpdateVehicleAngle(double angle)
        {
            // 更新车辆的当前角度
            currentAngle = angle;
        }

        private void UpdateVehiclePosition(Point point)
        {
            // 更新车辆的当前位置
            currentCoordination.x = point.X;
            currentCoordination.y = point.Y;
            barcodeReadTag = lastVisitedTag = point.Tag;
        }

        internal void SetChargeStationTags(List<int> chargeStationTags)
        {
            ChargeStationTags = chargeStationTags;
        }

        public class Point
        {
            public Point()
            {

            }
            public Point(double x, double y)
            {
                X = x;
                Y = y;
            }
            public Point(int tag, double x, double y, double theta)
            {
                X = x;
                Y = y;
                Tag = tag;
                Theta = theta;
            }

            public double X { get; set; }
            public double Y { get; set; }
            public int Tag { get; set; }
            public double Theta { get; internal set; }
        }
    }
}
