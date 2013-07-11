using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Signatory.Models
{
    public class RepoViewModel
    {
        public dynamic User { get; set; }
        public dynamic Collaborators { get; set; }
        public dynamic Repository { get; set; }
    }
}