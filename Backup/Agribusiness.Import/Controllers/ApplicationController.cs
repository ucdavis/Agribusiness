using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Agribusiness.Import.Controllers
{
    public class ApplicationController : Controller
    {
        protected static readonly AgribusinessContext Db = new AgribusinessContext();
    }
}
