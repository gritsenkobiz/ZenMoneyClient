using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gritsenko.Universal.Common
{
    public class DefferedActionManager
    {
        private static readonly Dictionary<string, DefferedAction> _pendingActions = new Dictionary<string, DefferedAction>();

        /**********************************************************************************/

        public static void Run(Action action)
        {
            Run(400, action);
        }

        /**********************************************************************************/

        public static void Run(int delayMilliseconds, Action action, [CallerFilePath] string instanceKey = null)
        {


            DefferedAction inst = null;

            lock (_pendingActions)
            {
                _pendingActions.TryGetValue(instanceKey, out inst);
            }

            if (inst != null)
            {
                inst.Update(delayMilliseconds, action);
            }
            else
            {
                inst = new DefferedAction(delayMilliseconds, instanceKey, action, OnActionComplete);
                lock (_pendingActions)
                {
                    _pendingActions[instanceKey] = inst;
                }
            }

        }

        public static void OnActionComplete(DefferedAction action)
        {
            lock (_pendingActions)
            {
                _pendingActions.Remove(action.Key);
            }
        }

    }

    public class DefferedAction
    {
        public string Key { get; set; }
        private Action _action;
        private readonly Action<DefferedAction> _onActionComplete;

        private CancellationTokenSource _cts;

        public DefferedAction(int delayMilliseconds, string key, Action action, Action<DefferedAction> onActionComplete)
        {
            try
            {
                Key = key;
                _action = action;
                _onActionComplete = onActionComplete;

                _cts = new CancellationTokenSource();
                Run(delayMilliseconds, _cts.Token);

            }
            catch (Exception ex)
            {
                var message = "Exception in (DefferedAction.cs)\\[DefferedAction.DefferedAction]";
                Logger.LogException(ex, message);
                throw new Exception(message, ex);
            }
        }

        /**********************************************************************************/

        public async void Run(int delay, CancellationToken token)
        {
            try
            {
                await Task.Delay(delay);

                if (token.IsCancellationRequested)
                {
                    return;
                }

                if (_action != null)
                {
                    _action();
                }

                if (_onActionComplete != null)
                {
                    _onActionComplete(this);
                }
            }
            catch (TaskCanceledException)
            {
                //ok
            }
            catch (Exception ex)
            {
                var message = "Exception in (DefferedAction.cs)\\[DefferedAction.Run]";
                Logger.LogException(ex, message);
                throw new Exception(message, ex);
            }
        }

        public void Update(int delayMilliseconds, Action action)
        {
            if (_cts != null)
            {
                _cts.Cancel();
            }

            _cts = new CancellationTokenSource();

            _action = action;
            Run(delayMilliseconds, _cts.Token);
        }
    }
}
