namespace DefaultNamespace
{
    public static class DemoModeChecker
    {
        public static bool DemoMode = false;

        private static long maxDemoTimerSeconds = 20;
        
        private static long demoTimer;

        public static bool CheckDemoTimer()
        {
            var now = System.DateTime.Now.Ticks;
            return DemoMode && (now - demoTimer) > maxDemoTimerSeconds * 10000000;
        }

        public static void ResetDemoTimer()
        {
            demoTimer = System.DateTime.Now.Ticks;
        }

    }
}