using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace citadel_wpf
{
    public interface IEntity
    {
        string GetName();
        string ToXMLString();
    }
}
