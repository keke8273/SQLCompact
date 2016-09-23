using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace TaskQueueAndCancellationPractise
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Random _delayRandomizer;
        private readonly CancellationTokenSource _cancellationSource;

        public MainWindow()
        {
            InitializeComponent();
            _delayRandomizer = new Random(DateTime.Now.Millisecond);
            _cancellationSource = new CancellationTokenSource();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as MainWindowViewModel;
            var id = "Button1Task";

            QueueButtonTask(vm, id);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as MainWindowViewModel;
            var id = "Button2Task";

            QueueButtonTask(vm, id);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as MainWindowViewModel;
            var id = "Button3Task";

            QueueButtonTask(vm, id);

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as MainWindowViewModel;
            var id = "Button4Task";

            QueueButtonTask(vm, id);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as MainWindowViewModel;
            var id = "Button5Task";

            QueueButtonTask(vm, id);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as MainWindowViewModel;
            var id = "Button6Task";

            QueueButtonTask(vm, id);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as MainWindowViewModel;
            var id = "Button7Task";

            QueueButtonTask(vm, id);
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as MainWindowViewModel;
            var id = "Button8Task";

            QueueButtonTask(vm, id);
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            _cancellationSource.Cancel();
        }

        private void QueueButtonTask(MainWindowViewModel vm, string id)
        {
            if (vm.TasksRunning.Any(x => x.Id == id))
                return;

            //put the tasks that is click on the height priority
            if (vm.TasksAwaitRunning.Any(x => x.Id == id))
            {
                vm.UpdateAwaitTaskPriority(id);
                return;
            }

            var delay = _delayRandomizer.Next(5, 15);
            var appTask = new AppTask
            {
                Id = id,
                Delay = delay,
                Task = new Task(() => LongRunningClass.LongRunningMethod(delay), _cancellationSource.Token, TaskCreationOptions.None)
            };

            appTask.Task.ContinueWith(t => vm.OnTaskComplete(id), 
                CancellationToken.None,
                TaskContinuationOptions.OnlyOnRanToCompletion,
                TaskScheduler.FromCurrentSynchronizationContext());

            appTask.Task.ContinueWith(t => vm.OnTaskCanceled(id),
                CancellationToken.None,
                TaskContinuationOptions.OnlyOnCanceled, 
                TaskScheduler.FromCurrentSynchronizationContext());

            vm.QueueNewTask(appTask);
        }
    }
}
