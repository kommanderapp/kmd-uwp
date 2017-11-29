using DiffMatchPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kmd.Core.Comparer
{
    public static class DiffsExtensions
    {
        public static string ToPrettyHtml(this IEnumerable<Diff> diffs, bool isDark)
        {
            if (diffs == null) throw new ArgumentNullException(nameof(diffs));
            StringBuilder html = new StringBuilder();

            AppendStyles(html);

            if (isDark)
            {
                html.Append("<body class=\"dark-mode\">");
            }
            else
            {
                html.Append("<body class=\"light-mode\">");
            }

            html.Append("<ol class=\"line-no-counter\">");
            html.Append("<li><span class=\"content\">");
            foreach (Diff aDiff in diffs)
            {
                string text = aDiff.text.Replace("&", "&amp;").Replace("<", "&lt;")
                  .Replace(">", "&gt;").Replace("\n", "</span></li><li><span class=\"content\">");
                switch (aDiff.operation)
                {
                    case Operation.INSERT:
                        html.Append("<ins style=\"background:#6BB26B;\">").Append(text)
                            .Append("</ins>");
                        break;

                    case Operation.DELETE:
                        html.Append("<del style=\"background:#B26B6B;\">").Append(text)
                            .Append("</del>");
                        break;

                    case Operation.EQUAL:
                        html.Append("<span>").Append(text).Append("</span>");
                        break;
                }
            }
            html.Append("<li>");
            html.Append("<ol>");
            html.Append("</body>");
            return html.ToString();
        }

        private static void AppendStyles(StringBuilder html)
        {
            html.Append("<style>");
            html.Append("body {font-size: 20px;}");
            html.Append("body.dark-mode ins,body.dark-mode del{color:#000;}");
            html.Append("body.dark-mode{background-color:#000;color:#FFF;} body.dark-mode::selection{background-color:#FFF;color:#000;}");
            html.Append("body.light-mode{background-color:#FFF;color:#000;} body.dark-mode::selection{background-color:#000;color:#FFF;}");
            html.Append(".line-no-counter{margin:0;padding:0;list-style-type:none}.line-no-counter li{counter-increment:step-counter;margin-bottom:5px;margin-left:50px;min-height:20px;-ms-user-select:none}.line-no-counter li .content{-ms-user-select:text}.line-no-counter li::before{content:counter(step-counter);color:#9B9696;position:absolute;left:10px;user-select:none}");
            html.Append("</style>");
        }
    }
}
