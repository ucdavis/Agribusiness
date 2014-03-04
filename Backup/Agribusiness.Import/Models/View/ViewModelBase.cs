using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agribusiness.Import.Models.View
{
    public class ViewModelBase
    {
        public IList<KeyValuePair<string, string>> Errors { get; set; }
        public bool AlreadyImported { get; set; }
    }
}