using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSZIP.Web.Site.Models
{
    public class TreeModel
    {
        public Guid Id { get; set; }
        public string DirName { get; set; }
        public List<string> FileNames { get; set; }
        public int Level { get; set; }
        public TreeModel ChildTree { get; set; }
    }
}
