using System;
using System.Windows;
using TaskQueueAndCancellationPractise.DB;

namespace TaskQueueAndCancellationPractise
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var context = new ExamManagementDbContext())
            {
                context.Database.Initialize(true);

                context.ClassExamed.Add(new ClassExamedEntity(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()));
                context.SaveChanges();
            }

            this.MainWindow = new MainWindow(){DataContext = new MainWindowViewModel()};
            this.MainWindow.Show();
        }
    }
}
