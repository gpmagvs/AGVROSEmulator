namespace AGVROSEmulator
{
    public static class Log
    {
        public static void Info(string message)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}] " + message);
        }
    }
}
