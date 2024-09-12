using AppTracker.DataAccess.Repositories;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;

namespace AppTracker
{
    public partial class MainWindow : Window
    {
        private readonly IRepository<AppUsage> _appUsageRepository;
        private string currentProcess = "None";
        private ObservableCollection<AppUsage> appUsageList = new ObservableCollection<AppUsage>();
        private DispatcherTimer _timer;
        private DateTime _lastSwitchTime;

        public MainWindow(IRepository<AppUsage> appUsageRepository)
        {
            InitializeComponent();
            _appUsageRepository = appUsageRepository;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();

            LoadAppUsageList();
        }

        private async void LoadAppUsageList()
        {
            var appUsages = await _appUsageRepository.GetAllAsync();
            AppHistoryListBox.ItemsSource = appUsages;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateProcess();
            UpdateUsageTimes();
        }

        private async void UpdateProcess()
        {
            var process = GetActiveProcess();
            if (process != currentProcess)
            {
                if (!string.IsNullOrEmpty(currentProcess))
                {
                    await UpdateAppUsageAsync(currentProcess);
                }

                currentProcess = process;
                if (!string.IsNullOrEmpty(currentProcess))
                {
                    var existingApp = await _appUsageRepository.FindByIdAsync(currentProcess.GetHashCode());
                    if (existingApp == null)
                    {
                        var newApp = new AppUsage
                        {
                            Name = currentProcess,
                            Icon = IconHelper.GetIconBitmapSource(GetExecutablePath(currentProcess)),
                            UsageTime = TimeSpan.Zero
                        };
                        await _appUsageRepository.PostAsync(newApp);
                    }

                    _lastSwitchTime = DateTime.Now;
                }
            }
        }

        private async Task UpdateAppUsageAsync(string processName)
        {
            var appUsage = await _appUsageRepository.FindByIdAsync(processName.GetHashCode());
            if (appUsage != null)
            {
                var elapsed = DateTime.Now - _lastSwitchTime;
                appUsage.UsageTime += elapsed;
                await _appUsageRepository.UpdateAsync(appUsage);
            }
        }

        private void UpdateUsageTimes()
        {
            if (!string.IsNullOrEmpty(currentProcess))
            {
                var elapsed = DateTime.Now - _lastSwitchTime;
                var appUsage = appUsageList.FirstOrDefault(app => app.Name == currentProcess);
                if (appUsage != null)
                {
                    appUsage.UsageTime += elapsed;
                }

                _lastSwitchTime = DateTime.Now;

                AppHistoryListBox.Items.Refresh();
            }
        }

        private string GetActiveProcess()
        {
            var handle = GetForegroundWindow();
            if (handle == IntPtr.Zero) return "None";

            uint processId;
            GetWindowThreadProcessId(handle, out processId);
            if (processId == 0) return "None";

            try
            {
                var process = Process.GetProcessById((int)processId);
                return process != null ? process.ProcessName : "None";
            }
            catch
            {
                return "None";
            }
        }

        private string GetExecutablePath(string processName)
        {
            var process = Process.GetProcessesByName(processName).FirstOrDefault();
            return process?.MainModule?.FileName;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
    }
}
