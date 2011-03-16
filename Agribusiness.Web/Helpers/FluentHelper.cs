using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Agribusiness.Web.Helpers
{
    public static class FluentHtmlExtensions
    {
        public static T IncludeUnobtrusiveValidationAttributes<T>(this T element, HtmlHelper htmlHelper)
            where T : MvcContrib.FluentHtml.Elements.IElement
        {
            IDictionary<string, object> validationAttributes = htmlHelper
                .GetUnobtrusiveValidationAttributes(element.GetAttr("name"));

            foreach (var validationAttribute in validationAttributes)
            {
                element.SetAttr(validationAttribute.Key, validationAttribute.Value);
            }

            return element;

        }

    }


}