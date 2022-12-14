
namespace BlApi
{
    /// <summary>
    /// A main interface that centers all the interfaces of the logical layer
    /// </summary>
    public interface IBl
    {
        /// <summary>
        /// A property that returns the IProduct entity
        /// </summary>
        public IProduct Product { get; }
        /// <summary>
        /// A property that returns the IOrder entity
        /// </summary>
        public IOrder Order { get; }
        /// <summary>
        /// A property that returns the ICart entity
        /// </summary>
        public ICart cart { get; }
    }
}