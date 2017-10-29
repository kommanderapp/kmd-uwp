using kmd.DependecyInjection;
using System;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using kmd.Activation;
using kmd.ViewModels;
using kmd.Views;

namespace kmd
{
    public sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            ServiceLocatorInitializer.Initialize();
            _activationService = new Lazy<ActivationService>(CreateActivationService);
        }

        private readonly Lazy<ActivationService> _activationService;

        private ActivationService ActivationService => _activationService.Value;

        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, typeof(MainViewModel), new ShellPage());
        }

        #region Activation handlers

        /// <summary>
        /// Invoked when the application is activated by some means other than normal launching.
        /// </summary>
        /// <param name="args">Event data for the event.</param>
        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        protected override async void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (!e.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(e);
            }
        }

        #endregion Activation handlers
    }
}
