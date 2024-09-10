using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;

namespace AppTracker
{
    public partial class MainWindow : Window
    {
        private string currentProcess = "None";
        private ObservableCollection<AppUsage> appUsageList = new ObservableCollection<AppUsage>();
        private DispatcherTimer _timer;
        private DateTime _lastSwitchTime;

        public MainWindow()
        {
            InitializeComponent();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();

            AppHistoryListBox.ItemsSource = appUsageList;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateProcess();
            UpdateUsageTimes();
        }

        private void UpdateProcess()
        {
            var process = GetActiveProcess();
            if (process != currentProcess)
            {
                if (!string.IsNullOrEmpty(currentProcess))
                {
                    UpdateAppUsage(currentProcess);
                }

                currentProcess = process;
                if (!string.IsNullOrEmpty(currentProcess))
                {
                    var existingApp = appUsageList.FirstOrDefault(app => app.Name == currentProcess);
                    if (existingApp == null)
                    {
                        var newApp = new AppUsage
                        {
                            Name = currentProcess,
                            Icon = IconHelper.GetIconBitmapSource(GetExecutablePath(currentProcess)),
                            UsageTime = TimeSpan.Zero
                        };
                        appUsageList.Add(newApp);
                    }

                    _lastSwitchTime = DateTime.Now;
                }
            }
        }

        private void UpdateAppUsage(string processName)
        {
            var appUsage = appUsageList.FirstOrDefault(app => app.Name == processName);
            if (appUsage != null)
            {
                var elapsed = DateTime.Now - _lastSwitchTime;
                appUsage.UsageTime += elapsed;
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
