using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using static Simulator.Simulator;
using System.Windows.Threading;

namespace PL.Manager;

/// <summary>
/// Interaction logic for SimulatorWindow.xaml
/// </summary>
public partial class SimulatorWindow : Window
{
    #region PUBLIC MEMBERS

    /// <summary>
    /// dependency property for the order id text box
    /// </summary>
    public string OrderIDTxt
    {
        get { return (string)GetValue(OrderIDTxtProperty); }
        set { SetValue(OrderIDTxtProperty, value); }
    }

    // Using a DependencyProperty as the backing store for orderIDTxt.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty OrderIDTxtProperty =
        DependencyProperty.Register("OrderIDTxt", typeof(string), typeof(Window), new PropertyMetadata(""));

    /// <summary>
    /// dependency property for statuse text box
    /// </summary>
    public string StatusTxt
    {
        get { return (string)GetValue(StatusTxtProperty); }
        set { SetValue(StatusTxtProperty, value); }
    }

    // Using a DependencyProperty as the backing store for StatusTxt.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty StatusTxtProperty =
        DependencyProperty.Register("StatusTxt", typeof(string), typeof(Window), new PropertyMetadata(""));

    /// <summary>
    /// the simulator window ctor
    /// </summary>
    public SimulatorWindow()
    {
        InitializeComponent();
        Loaded += ToolWindow_Loaded;
        workerStart();

    }

    #endregion

    #region PRIVATE MEMBERS

    /// <summary>
    /// instance of the bl who contains access to all the bl implementation
    /// </summary>
    BlApi.IBl? bl = BlApi.Factory.Get();

    BackgroundWorker? worker;
    Duration duration;
    DoubleAnimation? doubleanimation;
    ProgressBar? ProgressBar;
    int seconds;
    DispatcherTimer? _timer;
    TimeSpan _time;

    /// <summary>
    /// Loads the events of the simulator and start it
    /// </summary>
    void workerStart()
    {
        worker = new BackgroundWorker();
        worker.DoWork += WorkerDoWork!;
        worker.WorkerReportsProgress = true;
        worker.WorkerSupportsCancellation = true;
        worker.ProgressChanged += workerProgressChanged!;
        worker.RunWorkerCompleted += RunWorkerCompleted!;
        worker.RunWorkerAsync();
    }

    /// <summary>
    /// the function for the DoWork event that loads the simulator events and run the simulator
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void WorkerDoWork(object sender, DoWorkEventArgs e)
    {
        propsChanged += progressChanged!;
        Simulator.Simulator.stop += stop!;
        Run();
        while (!worker.CancellationPending)
        {
            worker.ReportProgress(1);
            Thread.Sleep(1000);
        }
    }

    /// <summary>
    /// the clock event ProgressChangeds function
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void workerProgressChanged(object sender, ProgressChangedEventArgs e) => simulatorTxt.Text = DateTime.Now.ToString("h:mm:ss");

    /// <summary>
    /// caled tothe stop function for closing the window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void stopSimulatorBtn_Click(object sender, RoutedEventArgs e) => stop(sender, EventArgs.Empty);

    /// <summary>
    /// the function for the RunWorkerCompleted event of the bakeground Worker
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) => this.Close();

    /// <summary>
    /// stop all the threds, background Worker(the clock), the simolutor and the progress bar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void stop(object sender, EventArgs e)
    {
        if (worker.WorkerSupportsCancellation == true)
            worker.CancelAsync();
        Stop();

        SBar.Items.Remove(ProgressBar);
        propsChanged -= progressChanged!;
        Simulator.Simulator.stop -= stop!;

        if (!CheckAccess())
            Dispatcher.BeginInvoke(stop, sender, e);
        else
        {
            Close();
        }
    }

    /// <summary>
    /// the function for the propsChanged event of the simulator /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void progressChanged(object sender, EventArgs e)
    {
        OrderEventArgs orderEventArgs = (OrderEventArgs)e;
        seconds = orderEventArgs.seconds / 1000;

        if (!CheckAccess())
        {
            Dispatcher.BeginInvoke(progressChanged, sender, e);
        }
        else
        {
            if (seconds == 0)
            {
                timer(0);
                SBar.Items.Remove(ProgressBar);
                OrderIDTxt = "אין הזמנות לטיפול";
            }
            else
            {
                timer(seconds - 1);
                ProgressBarStart(seconds);
                OrderIDTxt = orderEventArgs.order.ID.ToString();
                StatusTxt = orderEventArgs.order.Status.ToString()!;
            }
        }
    }

    void timer(int sec)
    {

        _time = TimeSpan.FromSeconds(sec);

        _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
        {
            tbTime.Text = string.Format("{0:D2}", _time.Seconds);
            if (_time == TimeSpan.Zero) _timer.Stop();
            _time = _time.Add(TimeSpan.FromSeconds(-1));
        }, Application.Current.Dispatcher);

        _timer.Start();
    }
    void ProgressBarStart(int s)
    {
        if (ProgressBar != null) SBar.Items.Remove(ProgressBar);
        ProgressBar = new ProgressBar();
        ProgressBar.IsIndeterminate = false;
        ProgressBar.Orientation = Orientation.Horizontal;
        ProgressBar.Width = 500;
        ProgressBar.Height = 30;
        duration = new Duration(TimeSpan.FromSeconds(s * 2));
        doubleanimation = new DoubleAnimation(200.0, duration);
        ProgressBar.BeginAnimation(ProgressBar.ValueProperty, doubleanimation);

        SBar.Items.Add(ProgressBar);
    }

    #region the closing button
    private const int GWL_STYLE = -16;
    private const int WS_SYSMENU = 0x80000;

    [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    void ToolWindow_Loaded(object sender, RoutedEventArgs e)
    {
        var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
        SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
    }
    #endregion

    #endregion
}
