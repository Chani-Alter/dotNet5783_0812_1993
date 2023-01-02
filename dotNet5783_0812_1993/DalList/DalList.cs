﻿using DalApi;
using Dal;
namespace DalList;

/// <summary>
/// a class how containes all the dal classes
/// </summary>
sealed public class DalList : IDal
{
    public IOrder Order { get; } = new DalOrder();
    public IProduct Product { get; } = new DalProduct();
    public IOrderItem OrderItem { get; } = new DalOrderItem();
}
