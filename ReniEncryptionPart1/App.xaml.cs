using System;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using ReniEncryptionPart1.Autofac;
using ViewModels.Autofac;
using ViewModels.ViewModels;

namespace ReniEncryptionPart1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            var builderBase = new ContainerBuilder();

            builderBase.RegisterModule(new ViewModelsModule());
            builderBase.RegisterModule(new ViewModule());

            var containerBase = builderBase.Build();

            void OpenWindow(Window window) => window.ShowDialog();

            var viewBase = containerBase.Resolve<View.MainUserMenu>(
                new NamedParameter("windows", containerBase.Resolve<MainUserMenu_ViewModel>(
                    new NamedParameter("windows", new Action[2]
                    {
                        ()=>OpenWindow(containerBase.Resolve<View.ComparisonBlockAlg>()),
                        ()=>OpenWindow(containerBase.Resolve<View.CryptoTransformData>())
                    }))));
            viewBase.Show();
        }

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Ошибка\n" + e.Exception.StackTrace + " " + "Исключение: "
                            + e.Exception.GetType().ToString() + " " + e.Exception.Message);

            e.Handled = true;
        }
    }
}
