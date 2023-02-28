using BlApi;

namespace Simulator;
public static class Simulator
{
    #region PUBLIC MEMBERS

    /// <summary>
    /// the stop simulator event
    /// </summary>
    public static event EventHandler? stop;

    /// <summary>
    /// the props change event
    /// </summary>
    public static event EventHandler<OrderEventArgs>? propsChanged;

    /// <summary>
    /// the event args for the propsChanged
    /// </summary>
    public class OrderEventArgs : EventArgs
    {
        public BO.Order order;
        public int seconds;
        public OrderEventArgs(int newSecond, BO.Order newOrder)
        {
            order = newOrder;
            seconds = newSecond;
        }
    }

    /// <summary>
    ///run the simulator
    /// </summary>
    public static void Run()
    {
        doWork = true;
        myThread = new Thread(new ThreadStart(Simulation));
        myThread.Start();
    }

    /// <summary>
    /// the stop function
    /// </summary>
    public static void Stop()
    {
        doWork = false;
    }

    #endregion

    #region PRIVATE MEMBERS

    /// <summary>
    /// instance of the bl who contains access to all the bl implementation
    /// </summary>
    static IBl? bl = Factory.Get();

    /// <summary>
    /// the order for care of
    /// </summary>
    static BO.Order order = new();

    /// <summary>
    /// the simulator thred
    /// </summary>
    static Thread? myThread { get; set; }

    /// <summary>
    /// a flag to  know if the simulator need to run
    /// </summary>
    static bool doWork = true;

    /// <summary>
    /// the simulator function
    /// </summary>
    static void Simulation()
    {
        while (doWork)
        {
            int? orderID = bl.Order.SelectOrder();
            if (orderID == null)
            {
                propsChanged("", new OrderEventArgs(0, new BO.Order()));
                Thread.Sleep(1000);
            }
            else
            {
                Random rnd = new Random();
                int seconds = rnd.Next(8000, 15000);
                order = bl.Order.GetOrderById((int)orderID);
                propsChanged("", new OrderEventArgs(seconds, order));
                while (doWork && seconds > 0)
                {
                    Thread.Sleep(1000);
                    seconds -= 1000;
                }
                if (doWork)
                {
                    if (order.Status == BO.OrderStatus.ConfirmedOrder)
                        bl.Order.UpdateSendOrderByManager(order.ID);
                    else
                        bl.Order.UpdateSupplyOrderByManager(order.ID);
                }
           }
        }
    }

    #endregion

}
