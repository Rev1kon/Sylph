using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sylph.Data.Models
{
    public class Site : IModel
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public int Option { get; set; }
        public string ContentPathOption { get; set; }
        public string NamePathOption { get; set; }
        public string ImagePathOption { get; set; }
        public string PricePathOption { get; set; }
        public string NotAvailableFilter { get; set; }
    }
}
