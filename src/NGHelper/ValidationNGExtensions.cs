using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Mvc.Properties;
using System.Web.Routing;

namespace NGHelper
{
    public static class ValidationNGExtensions
    {

        public static MvcHtmlString ValidationMessageForNg<TModel, TProperty>
            (this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression,string formName)
        {
            return htmlHelper.ValidationMessageForNg<TModel, TProperty>(expression, ((object)null), formName);
        }

        public static MvcHtmlString ValidationMessageForNg<TModel, TProperty>
            (this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes, string formName)
        {
            return htmlHelper.ValidationMessageForNg<TModel, TProperty>(expression,
                ((IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)),formName);
        }

        public static MvcHtmlString ValidationMessageForNg<TModel, TProperty>
            (this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression,
            IDictionary<string, object> htmlAttributes,string formName)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var name = ExpressionHelper.GetExpressionText(expression);

            return ValidationMessageNgHelper(htmlHelper, metadata, name, htmlAttributes,formName);
        }


        private static MvcHtmlString ValidationMessageNgHelper(
            HtmlHelper htmlHelper, ModelMetadata metadata, 
            string name, IDictionary<string, object> htmlAttributes,string formName)
        {
            string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (String.IsNullOrEmpty(fullName))
            {
                throw new ArgumentException("Object FullName is Null or Empty", "name");
            }

            TagBuilder divTagBuilder = new TagBuilder("div");
            divTagBuilder.MergeAttributes(htmlAttributes);
            divTagBuilder.MergeAttribute("ng-show", string.Format("{0}.{1}.$dirty && {0}.{1}.$invalid", formName, fullName));
            divTagBuilder.AddCssClass("error");

            var validations = ModelValidatorProviders.Providers.GetValidators
                (metadata ?? ModelMetadata.FromStringExpression(name, new ViewDataDictionary()),
                new ControllerContext()).SelectMany(v => v.GetClientValidationRules());

            var validatorMessages = validations.ToDictionary
                (k => k.ValidationType, v => v.ErrorMessage);

            foreach (var item in validatorMessages)
            {
                var smallTagBuilder = new TagBuilder("small");
                smallTagBuilder.AddCssClass("error");
                switch (item.Key)
                {

                    case "required":
                        smallTagBuilder.MergeAttribute("ng-show", string.Format("{0}.{1}.$error.required", formName, fullName));
                        smallTagBuilder.SetInnerText(item.Value);
                        break;
                    case "length":
                        smallTagBuilder.MergeAttribute("ng-show",
                            string.Format("{0}.{1}.$error.minlength || {0}.{1}.$error.maxlength", formName, fullName));
                        smallTagBuilder.SetInnerText(item.Value);
                        break;
                    case "url":
                        smallTagBuilder.MergeAttribute("ng-show", string.Format("{0}.{1}.$error.url", formName, fullName));
                        smallTagBuilder.SetInnerText(item.Value);
                        break;
                    case "number":
                        smallTagBuilder.MergeAttribute("ng-show", string.Format("{0}.{1}.$error.number", formName, fullName));
                        smallTagBuilder.SetInnerText(item.Value);
                        break;
                    case "email":
                        smallTagBuilder.MergeAttribute("ng-show", string.Format("{0}.{1}.$error.email", formName, fullName));
                        smallTagBuilder.SetInnerText(item.Value);
                        break;
                    default:
                        break;
                }
                divTagBuilder.InnerHtml += smallTagBuilder.ToString();
                //ng-pattern="/a-zA-Z/"
            }

            return new MvcHtmlString(divTagBuilder.ToString());
        }
    }
}



