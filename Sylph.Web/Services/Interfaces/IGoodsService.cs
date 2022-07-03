using Sylph.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sylph.Web.Services.Interfaces
{
    public interface IGoodsService 
    {
        public List<FilterResult> FilterByName(List<Item> items);
    }
}
