using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sylph.Web.Models
{
    public class FilterResult
    {
        public string FilteringCriteria { get; set; }
        public List<Item> Items { get; set; }
    }
}
