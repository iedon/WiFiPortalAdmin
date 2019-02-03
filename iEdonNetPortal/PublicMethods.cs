using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iEdonNetPortal
{
    public class PublicMethods
    {
        public static int IsValidSession(HttpContext ctx, string ViewDatasMessage = "")
        {
            if (ctx.Session.Get("username") == null || Int64.Parse(ctx.Session.GetString("expires")) <= DateTime.Now.ToBinary())
            {
                ctx.Session.Clear();
                ViewDatasMessage = "会话超时，请重新登录。";
                return 0;
            }
            return 1;
        }

        public static void SetSession(HttpContext ctx, string username) {
            ctx.Session.SetString("username", username);
            ctx.Session.SetString("expires", DateTime.UtcNow.AddHours(1).ToBinary().ToString());
        }

        public static void ClearSession(HttpContext ctx) {
            ctx.Session.Clear();
        }

        public static string GetLoggedInUsername(HttpContext ctx)
        {
            if (ctx.Session.Get("username") == null || Int64.Parse(ctx.Session.GetString("expires")) <= DateTime.Now.ToBinary()) { return null; }
            return ctx.Session.GetString("username");
        }

    }
}
