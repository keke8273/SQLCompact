using System;
using System.IO;
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

            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            Directory.CreateDirectory(Path.Combine(path, "Upsoft"));
            // Set the data directory to the users %AppData% folder            
            // So the database file will be placed in:  C:\\Users\\<Username>\\AppData\\Roaming\\            
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(path, "Upsoft"));

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
