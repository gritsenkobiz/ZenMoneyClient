using System;

namespace Gritsenko.Universal.Abstract
{
    public class ConnectionStatusChangedEventEventArgs : EventArgs
    {
        public bool IsOnline;
    }
}