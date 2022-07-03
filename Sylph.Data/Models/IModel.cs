using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sylph.Data.Models
{
    public interface IModel
    {
        public Guid Id { get; set; }
    }
}
