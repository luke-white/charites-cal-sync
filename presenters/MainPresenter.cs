namespace itdevgeek_charites.presenters
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;
    using itdevgeek_charites.helper.application;
    using itdevgeek_charites.screens;

    public interface IMainView
    {
        void UpdateStatusText(string updatedText);
        void ShowWindow();
        void EnableActions(bool enable);
        void ShowMessage(string message);
        event EventHandler UpdateRequested;
        event EventHandler SettingsRequested;
        event EventHandler ImportRequested;
        event EventHandler ExitRequested;
    }

    public class MainPresenter
    {
        private readonly IMainView _view;
        private readonly BackgroundWorker _backgroundSynchWorker;
        private static ConfigurationScreen _settingsScreen;
        private static ImportScreen _importScreen;

        public MainPresenter(IMainView view)
        {
            _view = view;
            
            _view.UpdateRequested += OnUpdateRequested;
            _view.SettingsRequested += OnSettingsRequested;
            _view.ImportRequested += OnImportRequested;
            _view.ExitRequested += OnExitRequested;

            _backgroundSynchWorker = new BackgroundWorker();
            _backgroundSynchWorker.DoWork += BgSynchWorker_DoWork;
            _backgroundSynchWorker.RunWorkerCompleted += BgSynchWorker_RunWorkerCompleted;
        }

        private void OnUpdateRequested(object sender, EventArgs e)
        {
            if (Charites.HaveRequiredData())
            {
                _view.EnableActions(false);
                _backgroundSynchWorker.RunWorkerAsync();
            }
            else
            {
                _view.ShowMessage("Please update program settings to enter an Account and select a Calendar to Update.");
            }
        }

        private void OnSettingsRequested(object sender, EventArgs e)
        {
            if (_settingsScreen == null || _settingsScreen.IsDisposed)
            {
                _settingsScreen = new ConfigurationScreen();
            }

            _settingsScreen.TopMost = true;
            _settingsScreen.StartPosition = FormStartPosition.CenterScreen;
            _settingsScreen.Show();
            _settingsScreen.Refresh();
        }

        private void OnImportRequested(object sender, EventArgs e)
        {
            if (_importScreen == null || _importScreen.IsDisposed)
            {
                _importScreen = new ImportScreen();
            }

            _importScreen.TopMost = true;
            _importScreen.StartPosition = FormStartPosition.CenterScreen;
            _importScreen.Show();
            _importScreen.Refresh();
        }

        private void OnExitRequested(object sender, EventArgs e)
        {
            Application.Exit(); // Simplified for presenter logic, assuming calling context handles rest
        }

        private void BgSynchWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Charites.UpdateGoogleCalendarAsync().Wait();
        }

        private void BgSynchWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _view.EnableActions(true);
        }
    }
}
