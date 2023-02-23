using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO;

public struct User
{
    public int ID { get; set; }
    public string CustomerName { get; set; }
    public string Password { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerAdress { get; set; }

    public bool IsAdmin { get; set; }
    public override string ToString() => this.ToStringProperty();

}
