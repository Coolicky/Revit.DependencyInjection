using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Revit.DependencyInjection.Unity.Base;

namespace Revit.DependencyInjection.Unity.Async
{
    /// <summary>
    /// Provides a way to run asynchronous tasks on Revit's main thread
    /// </summary>
    public class RevitExternalHandler : IRevitEventHandler
    {
        private readonly string _name;
        private readonly ExternalEvent _externalEvent;
        private readonly IDictionary<Task, FuncTask> _queue = new ConcurrentDictionary<Task, FuncTask>();
        private readonly IRevitContext _revitContext;
        private object _contextResult;

        /// <summary>
        /// Constructor
        /// </summary>
        public RevitExternalHandler(RevitAsyncSettings settings, IRevitContext revitContext)
        {
            _revitContext = revitContext;
            _name = settings.Name;
            if (settings.IsJournalable)
            {
                _externalEvent = ExternalEvent.CreateJournalable(this);
            }
            else
            {
                _externalEvent = ExternalEvent.Create(this);
            }
        }

        /// <summary>
        /// Cancels all queue tasks
        /// </summary>
        public void CancelAll()
        {
            _queue.Clear();
        }

        /// <summary>
        /// Gets the number of queue tasks
        /// </summary>
        public int GetQueueCount()
        {
            return _queue.Count();
        }

        /// <summary>
        /// This method is called to handle the external event
        /// </summary>
        public void Execute(UIApplication app)
        {
            if (_queue.Any())
            {
                var actionKey = _queue.First();
                var taskKey = actionKey.Key;

                if (actionKey.Value.DelegateType == DelegateType.Action)
                {
                    RunAction(app, actionKey, taskKey);
                }
                else
                {
                    RunFunc(app, actionKey, taskKey);
                }
            }
        }

        private void RunAction(UIApplication app, KeyValuePair<Task, FuncTask> actionKey, Task taskKey)
        {
            try
            {
                if (!taskKey.IsCanceled)
                {
                    actionKey.Value.Action?.DynamicInvoke(app);
                }
            }
            finally
            {
                _queue.Remove(actionKey.Key);
                actionKey.Key.RunSynchronously();
            }
        }

        private void RunFunc(UIApplication app, KeyValuePair<Task, FuncTask> actionKey, Task taskKey)
        {
            try
            {
                if (!taskKey.IsCanceled)
                {
                    _contextResult = actionKey.Value.Action?.DynamicInvoke(app);
                }
            }
            finally
            {
                _queue.Remove(actionKey.Key);
                actionKey.Key.RunSynchronously();
                _contextResult = null;
            }
        }

        /// <summary>
        /// The event's name
        /// </summary>
        public string GetName()
        {
            return _name;
        }

        /// <summary>
        /// The action will run by Revit as soon as its thread is available
        /// </summary>
        public async Task RunAsync(Action<UIApplication> action, CancellationTokenSource cancelSource = null)
        {
            if (_revitContext.IsInRevitContext())
            {
                var app = _revitContext.GetUIApplication();
                action?.Invoke(app);
                return;
            }

            if (cancelSource == null)
            {
                cancelSource = new CancellationTokenSource();
            }

            var task = new Task(DummyAction, cancelSource.Token);
            await AddTask(action, task, cancelSource);
        }

        private async Task AddTask(Action<UIApplication> action, Task task, CancellationTokenSource tokenSource)
        {
            if (tokenSource.IsCancellationRequested)
            {
                return;
            }

            _queue.Add(task, new FuncTask { Action = action, Cancellation = tokenSource });
            _externalEvent.Raise();
            await task;
        }

        /// <summary>
        /// The action will run by Revit as soon as its thread is available
        /// </summary>
        public async Task<T> RunAsync<T>(Func<UIApplication, T> action, CancellationTokenSource cancelSource = null)
        {
            if (_revitContext.IsInRevitContext())
            {
                var app = _revitContext.GetUIApplication();
                var result = action.Invoke(app);
                return result;
            }

            if (cancelSource == null)
            {
                cancelSource = new CancellationTokenSource();
            }

            var task = new Task<object>(DummyFunc, cancelSource.Token);
            return await AddTask(action, task, cancelSource);
        }

        private async Task<T> AddTask<T>(Func<UIApplication, T> action, Task<object> task, CancellationTokenSource tokenSource)
        {
            if (tokenSource.IsCancellationRequested)
            {
                return default;
            }

            var funcTask = new FuncTask { Action = action, Cancellation = tokenSource, DelegateType = DelegateType.Func };
            _queue.Add(task, funcTask);
            _externalEvent.Raise();

            var asyncResult = await task;
            return (T)asyncResult;
        }

        private void DummyAction()
        {
        }

        private object DummyFunc()
        {
            return _contextResult;
        }
    }
}