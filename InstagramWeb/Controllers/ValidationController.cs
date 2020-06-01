using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InstagramWeb.Controllers
{
    public class ValidationController : BaseController
    {
        // GET: Validation

        public JsonResult IsAvailableEmail(string email, int Id = 0)
        {
            var emailExists = ScheduleContext.Users.Count(x => x.Email == email && Id != x.Id);

            if (emailExists == 0)
                return Json(true, JsonRequestBehavior.AllowGet);
            var suggestedUid = string.Format(CultureInfo.InvariantCulture, "{0} is already registered", email);
            return Json(suggestedUid, JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsAvailableUsername(string username, int Id = 0)
        {
            var emailExists = ScheduleContext.Users.Count(x => x.Username == username && Id != x.Id);

            if (emailExists == 0)
                return Json(true, JsonRequestBehavior.AllowGet);
            var suggestedUid = string.Format(CultureInfo.InvariantCulture, "{0} is already In use", username);
            return Json(suggestedUid, JsonRequestBehavior.AllowGet);
        }

    }

}