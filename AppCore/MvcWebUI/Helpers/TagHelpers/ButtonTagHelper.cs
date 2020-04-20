using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Text;

namespace AppCore.MvcWebUI.Helpers.TagHelpers
{
    [HtmlTargetElement("pushbutton")]
    public class ButtonTagHelper : TagHelper
    {
        public string Type { get; set; } = "";
        public string Class { get; set; } = "";
        public string Text { get; set; } = "";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            StringBuilder html = new StringBuilder();
            html.Append("<button");
            if (!String.IsNullOrWhiteSpace(Type))
                html.Append($" type=\"{Type}\"");
            if (!String.IsNullOrWhiteSpace(Class))
                html.Append($" class=\"{Class}\"");
            html.Append(">");
            if (!String.IsNullOrWhiteSpace(Text))
                html.Append(Text);
            html.Append("</button>");
            output.Content.SetHtmlContent(html.ToString());
        }
    }
}
