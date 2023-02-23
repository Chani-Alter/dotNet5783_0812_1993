using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    /// <summary>
    /// delete a order item
    /// </summary>
    /// <param name="id">the order item id</param>
    /// <exception cref="Exception">if the order item didnt exist</exception>
    public void Delete(int id)
    {
        List<OrderItem?> orderItemList = XmlTools.LoadListFromXmlSerializer<OrderItem>(entityName);

        var result = orderItemList.FirstOrDefault(ord => ord?.ID == id);

        if (result == null)
            throw new DoesNotExistedDalException(id, "order-item", "order item is not exist");

        orderItemList.Remove(result);
        XmlTools.SaveListForXmlSerializer(orderItemList, entityName);

    }


    /// <summary>
    /// update an order item
    /// </summary>
    /// <param name="orderItem">the new details of the order item</param>
    /// <exception cref="Exception">if the order cdoesnt exist</exception>
    public void Update(OrderItem orderItem)
    {
        List<OrderItem?> orderItemList = XmlTools.LoadListFromXmlSerializer<OrderItem>(entityName);

        int index = orderItemList.FindIndex(item => item?.ID == orderItem.ID);

        if (index == -1)
            throw new DoesNotExistedDalException(orderItem.ID, "order item", "Order Item is not exist");

        orderItemList[index] = orderItem;
        XmlTools.SaveListForXmlSerializer(orderItemList, entityName);

    }

    #endregion

    #region PRIVATE MEMBER

    /// <summary>
    /// the entity name
    /// </summary>
    const string entityName = @"OrderItem";

    #endregion

}