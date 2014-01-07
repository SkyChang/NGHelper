﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Mvc.Html;

namespace NGHelper
{
    public static class EditorForNGExtensions
    {
        public static MvcHtmlString NgValFor<TModel, TProperty>
            (this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return NgValFor(htmlHelper, expression, new RouteValueDictionary());
        }

        public static MvcHtmlString NgValFor<TModel, TProperty>
            (this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes)
        {
            return NgValFor(htmlHelper, expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString TextBoxForNG<TModel, TProperty>
            (this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var name = ExpressionHelper.GetExpressionText(expression);

            //var validatorMessages = validations.ToDictionary(k => k.ValidationType, v => v.ErrorMessage);

            var htmlAttributeDictionary = HtmlAttributesForNG(metadata, name, null);
            return htmlHelper.TextBoxFor(expression, htmlAttributeDictionary);
        }

        public static MvcHtmlString TextBoxForNG<TModel, TProperty>
            (this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel,
            TProperty>> expression, object htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var name = ExpressionHelper.GetExpressionText(expression);

            var htmlAttributeDictionary = (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            //var validatorMessages = validations.ToDictionary(k => k.ValidationType, v => v.ErrorMessage);

            htmlAttributeDictionary = HtmlAttributesForNG(metadata, name, htmlAttributeDictionary);
            return htmlHelper.TextBoxFor(expression, htmlAttributeDictionary);
        }

        private static IDictionary<string, object> HtmlAttributesForNG(ModelMetadata metadata,
            string name, IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null)
            {
                htmlAttributes = new Dictionary<string, object>();
            }

            var validations = ModelValidatorProviders.Providers.GetValidators
                (metadata ?? ModelMetadata.FromStringExpression(name, new ViewDataDictionary()),
                new ControllerContext()).SelectMany(v => v.GetClientValidationRules());

            var validatorMessages = validations.ToDictionary
                (k => k.ValidationType, v => v.ValidationParameters);


            foreach (var item in validatorMessages)
            {
                switch (item.Key)
                {
                    case "required":
                        break;
                    case "length":
                        if (item.Value.Keys.Contains("max")) htmlAttributes.Add("ng-maxlength", item.Value["max"]);
                        if (item.Value.Keys.Contains("min")) htmlAttributes.Add("ng-minlength", item.Value["min"]);
                        break;
                    case "url":
                        htmlAttributes.Add("type", "url");
                        break;
                    case "number":
                        htmlAttributes.Add("type", "number");
                        break;
                    case "email":
                        htmlAttributes.Add("type", "email");
                        break;
                    default:
                        break;
                }

                //ng-pattern="/a-zA-Z/"

            }
            htmlAttributes.Add("ng-model", metadata.PropertyName);
            return htmlAttributes;
        }
    }
}
