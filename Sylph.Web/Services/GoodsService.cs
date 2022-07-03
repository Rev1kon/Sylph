using Sylph.Web.Models;
using Sylph.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sylph.Web.Services
{
    public class GoodsService : IGoodsService
    {
        public List<FilterResult> FilterByName(List<Item> items) 
        {
            var filterResult = new List<FilterResult>();
            foreach (var item in items)
            {
                var name = item.Name;
                if (!filterResult.Exists(x => name.Split(' ').All(y => x.FilteringCriteria.Contains(y, StringComparison.OrdinalIgnoreCase)))
                    && !filterResult.Exists(x => x.FilteringCriteria.Split(' ').All(y => name.Contains(y, StringComparison.OrdinalIgnoreCase))))
                {
                    var section = new FilterResult
                    {
                        FilteringCriteria = name,
                        Items = new List<Item>()
                    };
                    section.Items.Add(item);
                    filterResult.Add(section);
                }
                else if(filterResult.Exists(x => name.Split(' ').All(y => x.FilteringCriteria.Contains(y, StringComparison.OrdinalIgnoreCase)))
                    ||filterResult.Exists(x => x.FilteringCriteria.Split(' ').All(y => name.Contains(y, StringComparison.OrdinalIgnoreCase))))
                {
                    filterResult.Find(x => (name.Split(' ').All(y => x.FilteringCriteria.Contains(y, StringComparison.OrdinalIgnoreCase))) 
                    || (x.FilteringCriteria.Split(' ').All(y => name.Contains(y, StringComparison.OrdinalIgnoreCase))))
                        .Items.Add(item);
                }
            }
            return filterResult;
        }
    }
}
