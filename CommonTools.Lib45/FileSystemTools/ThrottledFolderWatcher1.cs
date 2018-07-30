using System;

namespace CommonTools.Lib45.FileSystemTools
{
    public class ThrottledFolderWatcher1 : FolderChangeWatcher1
    {
        private DateTime _lastRaise;


        public ThrottledFolderWatcher1(uint intervalMS)
        {
            IntervalMS = intervalMS;
        }


        public uint IntervalMS { get; }


        protected override void RaiseFolderChanged(string filePath)
        {
            var now = DateTime.Now;
            if (IsTooSoon(now)) return;

            base.RaiseFolderChanged(filePath);
            _lastRaise = now;
        }


        private bool IsTooSoon(DateTime now)
        {
            var elapsd = (now - _lastRaise).TotalMilliseconds;
            return elapsd < IntervalMS;
        }
    }
}
