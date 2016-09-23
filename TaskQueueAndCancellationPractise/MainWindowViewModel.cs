using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace TaskQueueAndCancellationPractise
{
    public class MainWindowViewModel
    {
        private readonly ObservableCollection<AppTask> _tasksAwaitRunning;
        private readonly ObservableCollection<AppTask> _tasksRunning;
        private readonly ObservableCollection<AppTask> _tasksCompleted;
        private readonly ObservableCollection<AppTask> _tasksCanceled;
        private readonly ObservableCollection<AppTask> _allTasks;
        private Object taskLock = new Object();

        public int CoreNumber { get { return Environment.ProcessorCount; }}

        public MainWindowViewModel()
        {
            _tasksAwaitRunning = new ObservableCollection<AppTask>();
            _tasksRunning = new ObservableCollection<AppTask>();
            _tasksCompleted = new ObservableCollection<AppTask>();
            _tasksCanceled = new ObservableCollection<AppTask>();
            _allTasks = new ObservableCollection<AppTask>();
        }

        public ObservableCollection<AppTask> TasksAwaitRunning { get { return _tasksAwaitRunning; } }
        public ObservableCollection<AppTask> TasksRunning { get { return _tasksRunning; } }
        public ObservableCollection<AppTask> TasksAll { get { return _allTasks; } }
        public ObservableCollection<AppTask> TasksCompleted { get { return _tasksCompleted; } }
        public ObservableCollection<AppTask> TasksCanceled { get { return _tasksCanceled; } }

        public void OnTaskComplete(string id)
        {
            lock (taskLock)
            {
                var task = _tasksRunning.First(x => x.Id == id);

                _tasksRunning.Remove(task);
                _tasksCompleted.Add(task);

                if (_tasksAwaitRunning.Any())
                {
                    var next = _tasksAwaitRunning.OrderBy(x => x.Priority).First();
                    _tasksAwaitRunning.Remove(next);
                    next.Task.Start(TaskScheduler.Default);
                    _tasksRunning.Add(next);
                }
            }
        }

        public void UpdateAwaitTaskPriority(string id)
        {
            lock (taskLock)
            {
                var task = _tasksAwaitRunning.First(x => x.Id == id);

                task.Priority = 0;
                var otherPriority = 1;
                foreach (var otherTask in _tasksAwaitRunning.Where(x => x.Id != id))
                {
                    otherTask.Priority = otherPriority++;
                }
            }
        }

        public void OnTaskCanceled(string id)
        {
            lock (taskLock)
            {
                var task = _tasksRunning.FirstOrDefault(x => x.Id == id);

                if (task != null)
                {
                    _tasksRunning.Remove(task);
                }
                else
                {
                    task = _tasksAwaitRunning.FirstOrDefault(x => x.Id == id);
                    _tasksAwaitRunning.Remove(task);
                }

                _tasksCanceled.Add(task);

                //if (_tasksAwaitRunning.Any())
                //{
                //    var next = _tasksAwaitRunning.First();
                //    _tasksAwaitRunning.Remove(next);
                //    next.Task.Start(TaskScheduler.Default);
                //    _tasksRunning.Add(next);
                //}
            }
        }

        public void QueueNewTask(AppTask appTask)
        {
            lock (taskLock)
            {
                _allTasks.Add(appTask);

                if (_tasksRunning.Count < Environment.ProcessorCount)
                {
                    appTask.Task.Start(TaskScheduler.Default);
                    _tasksRunning.Add(appTask);
                }
                else
                {
                    appTask.Priority = _tasksAwaitRunning.Count;
                    _tasksAwaitRunning.Add(appTask);
                }
            }
        }


    }

    public class AppTask
    {
        public AppTask()
        {
            Priority = 0;
        }

        public Task Task { get; set; }
        public String Id { get; set; }
        public int Delay { get; set; }
        public int Priority { get; set; }

        public override string ToString()
        {
            return Id + "   Delay: " + Delay;
        }
    }

    public static class LongRunningClass
    {
        public static void LongRunningMethod(int delay)
        {
            Thread.Sleep(delay * 1000);
        }
    }
}
