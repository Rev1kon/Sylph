using Sylph.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sylph.Web.Services.Interfaces
{
    public interface ISearchService
    {
        public List<string> Browse(string searchedItem);
        public string CutString(string stringToCut);
        public List<Item> Search(string searchedItem);
    }
}
