using System;
using System.Linq;
using System.Web;

namespace FubuCore
{
    public static class UrlContext
    {
        static UrlContext()
        {
            Reset();
        }

        public static Func<string, string, string> Combine { get; private set; }
        public static Func<string, string> ToAbsolute { get; private set; }
        public static Func<string, string> ToFull { get; private set; }
        public static Func<string, string> ToPhysicalPath { get; private set; }

        public static void Reset()
        {
            if (HttpRuntime.AppDomainAppVirtualPath != null)
            {
                Live();
                return;
            }

            Stub("");
        }

        public static void Stub()
        {
            Stub("");
        }

        public static void Stub(string usingFakeUrl)
        {
            Combine = (basePath, subPath) => "{0}/{1}".ToFormat(basePath.TrimEnd('/'), subPath.TrimStart('/'));
            ToAbsolute = path => Combine(usingFakeUrl, path.Replace("~", ""));
            ToFull = path => Combine(usingFakeUrl, path.Replace("~", ""));
            ToPhysicalPath =
                virtPath => Combine(usingFakeUrl, virtPath).Replace("~", "").Replace("//", "/").Replace("/", "\\");
        }

        public static void Live()
        {
            Combine = VirtualPathUtility.Combine;
            ToAbsolute = path =>
            {
                string result = path.Replace("~", VirtualPathUtility.ToAbsolute("~"));
                return result.StartsWith("//") ? result.Substring(1) : result;
            };
            ToFull = path =>
            {
                var baseUri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
                return new Uri(baseUri, ToAbsolute(path)).ToString();
            };
            ToPhysicalPath = HttpContext.Current.Server.MapPath;
        }

        public static string GetUrl(string url)
        {
            if (!url.StartsWith("~/") || !url.StartsWith("/"))
            {
                url = ("~/" + url).Replace("~//", "~/");
            }


            return ToAbsolute(url);
        }

        public static string GetFullUrl(string path)
        {
            return ToFull(path);
        }

        public static string MapPath(this string webRelativePath)
        {
            return ToAbsolute(webRelativePath);
        }

        public static string PhysicalPath(this string webRelativePath)
        {
            return ToPhysicalPath(webRelativePath);
        }

        public static string WithQueryStringValues(this string querystring, params object[] values)
        {
            return querystring.ToFormat(values.Select(value => value.ToString().UrlEncoded()).ToArray());
        }

        public static string ToFullUrl(this string relativeUrl, params object[] args)
        {
            string formattedUrl = (args == null) ? relativeUrl : relativeUrl.ToFormat(args);

            return UrlContext.GetFullUrl(formattedUrl);
        }

        public static string UrlEncoded(this object target)
        {
            //properly encoding URI: http://blogs.msdn.com/yangxind/default.aspx
            return target != null ? Uri.EscapeDataString(target.ToString()) : string.Empty;
        }
    }
}