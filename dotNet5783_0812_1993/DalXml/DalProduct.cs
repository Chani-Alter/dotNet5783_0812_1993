using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    /// <summary>
    /// delete a product 
    /// </summary>
    /// <param name="id">the id of the product thet need to be deleted</param>
    /// <exception cref="Exception">if the product didnt exist</exception>
    public void Delete(int id)
    {
        XElement productRoot = XmlTools.LoadListFromXmlElement(entityName);
        XElement product = (from prod in productRoot.Elements()
                            where (int?)prod.Element("ID") == id
                            select prod).FirstOrDefault() ?? throw new DoesNotExistedDalException(id, "product", "product is not exist");
        product.Remove();
        XmlTools.SaveListForXmlElement(productRoot , entityName);   


    }


    /// <summary>
    /// update a product
    /// </summary>
    /// <param name="product">the product new details</param>
    /// <exception cref="Exception">if the product didnt exist</exception>
    public void Update(Product product)
    {
        Delete(product.ID);
        Add(product);   
    }

    #endregion

    #region PRIVATE MEMBER

    /// <summary>
    /// the entity name
    /// </summary>
    const string entityName = @"Product";

    #endregion

}