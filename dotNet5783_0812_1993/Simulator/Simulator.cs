using BlApi;

namespace Simulator;
public static class Simulator
{
    private static IBl bl = Factory.Get();

    private static BO.Order order = new();
    private static Thread? myThread { get; set; }

    private static bool doWork = true;

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

    public static event EventHandler? stop;

    public static event EventHandler<OrderEventArgs>? propsChanged;

    public static void Run()
    {
        doWork = true;
        myThread = new Thread(new ThreadStart(Simulation));
        myThread.Start();
    }


    private static void Simulation()
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
                if (order.Status == BO.OrderStatus.ConfirmedOrder)
                    bl.Order.UpdateSendOrderByManager(order.ID);
                else
                    bl.Order.UpdateSupplyOrderByManager(order.ID);
                propsChanged("", new OrderEventArgs(seconds, order));
                Thread.Sleep(seconds);
            }
        }
    }

    public static void Stop()
    {
        doWork = false;
    }
}
