using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public class Factory
{
    static public IBl Get() {
        return new BlImplementation.Bl();
    }
}
