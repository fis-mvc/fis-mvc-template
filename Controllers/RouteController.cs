/**
* @Author: chexingyou <chexingyou>
* @Date:   2016-05-10 23:52:01
* @Last modified by:   chexingyou
* @Last modified time: 2016-05-12 00:11:04
*/



using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

namespace fis_mvc_template.Controllers
{
    public class RouteController : Controller
    {
        private static string mappingFile = "/async/UrlMapping.json";
        private static Dictionary<String, String> mapping = null;
        public ActionResult Index()
        {

            var path = HttpContext.Request.Path;
            if(Regex.IsMatch(path,"\\.cshtml$")){
                ViewBag.viewName = path;
                return View("/Views" + path);
            }
            else{
                if (mapping == null)
                {
                   mapping = getUrlMapping();
                }
                if (mapping.ContainsKey(path))
                {
                    return Json(readFile(mapping[path]));
                }
                else
                {
                    return Content("Welcome to fis-mvc !");
                }
            }
        }

        public ActionResult Error(){
            return View();
        }

        private Dictionary<String, String> getUrlMapping()
        {
            Dictionary<String, String> dic = new Dictionary<string, string>();
            String content = readFile(mappingFile);
            content = content.Replace("\r", "").Replace("\n", "").Replace(" ","");
            MatchCollection mc = Regex.Matches(content, "([\'\"])[^\'\"]+\\1\\:([\'\"])[^\'\"]+\\2");
            for (int i = 0; i < mc.Count; i++)
            {
                var arr = mc[i].ToString().Replace("'","").Replace("\"","").Split(':');
                dic.Add(arr[0], arr[1]);
            }
            return dic;
        }

        private string readFile(string file){
            Console.WriteLine(AppDomain.CurrentDomain.GetData("APP_CONTEXT_BASE_DIRECTORY"));
            string realPath = AppDomain.CurrentDomain.GetData("APP_CONTEXT_BASE_DIRECTORY") + file;
            return System.IO.File.ReadAllText(realPath, Encoding.UTF8);
        }
	}
}
