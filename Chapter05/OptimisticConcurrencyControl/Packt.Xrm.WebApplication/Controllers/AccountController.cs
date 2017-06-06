using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Packt.Xrm.Extensions;
using System.Configuration;

namespace Packt.Xrm.WebApplication.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        // GET: Account
public ActionResult CreateAccount()
{
    var connectionString = ConfigurationManager.AppSettings["D365ConnectionString"];
    var updateActivity = new Dynamics365DataAccessLayer_Reused(connectionString);
    updateActivity.CreateAccount();
    return Content("Success!");
}
    }
}