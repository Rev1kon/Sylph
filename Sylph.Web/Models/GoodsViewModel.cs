using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sylph.Web.Models
{
    public class GoodsViewModel
    {
        public string SearchQuery { get; set; }
        public List<FilterResult> Result { get; set; }
    }
}
