using System;

namespace Gritsenko.Universal.Abstract
{
    public interface IConnectionStatusChecker
    {
        event EventHandler<ConnectionStatusChangedEventEventArgs> StatusChanged;
    }
}