using System.Data.Linq;
using System.ComponentModel.DataAnnotations;
using NGHelper;
using Xunit;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System;
using Moq;
using System.Web.Routing;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using NGHelper.Test.Util;


namespace NGHelper.Test
{
    public class InputNGExtensionsTest
    {
        //TEXTBOX
        private static readonly RouteValueDictionary _attributesDictionary = new RouteValueDictionary(new { baz = "BazValue" });
        private static readonly object _attributesObjectDictionary = new { baz = "BazObjValue" };
        private static readonly object _attributesObjectUnderscoresDictionary = new { foo_baz = "BazObjValue" };



        // TextBoxFor

        //[Fact]
        //public void TextBoxForWithNullExpressionThrows()
        //{
        //    // Arrange
        //    HtmlHelper<FooBarModel> helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

        //    // Act & Assert
        //    Assert.ThrowsArgumentNull(
        //        () => helper.TextBoxFor<FooBarModel, object>(null /* expression */),
        //        "expression"
        //        );
        //}

        [Fact]
        public void TextBoxForNG_With_RequiredValdation()
        {
            // Arrange
            HtmlHelper<FooBarModel> helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

            // Act
            MvcHtmlString html = helper.TextBoxForNG(m => m.fooRequired);
            // Assert
            Assert.Equal(@"<input id=""fooRequired"" name=""fooRequired"" required=""required"" type=""text"" value=""fooRequired"" />"
                ,html.ToHtmlString());
        }

        [Fact]
        public void TextBoxForNG_With_StringLengthValdation()
        {
            // Arrange
            HtmlHelper<FooBarModel> helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

            // Act
            MvcHtmlString html = helper.TextBoxForNG(m => m.fooStringLength);
            // Assert
            Assert.Equal(@"<input id=""fooStringLength"" name=""fooStringLength"" ng-maxlength=""20"" ng-minlength=""10"" type=""text"" value=""fooStringLength"" />"
                , html.ToHtmlString());
        }

        [Fact]
        public void TextBoxForNG_With_MaxLengthValdation()
        {
            // Arrange
            HtmlHelper<FooBarModel> helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

            // Act
            MvcHtmlString html = helper.TextBoxForNG(m => m.fooMaxLength);
            // Assert
            Assert.Equal(@"<input id=""fooMaxLength"" name=""fooMaxLength"" ng-maxlength=""20"" type=""text"" value=""fooMaxLength"" />"
                , html.ToHtmlString());
        }

        [Fact]
        public void TextBoxForNG_With_MinLengthValdation()
        {
            // Arrange
            HtmlHelper<FooBarModel> helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

            // Act
            MvcHtmlString html = helper.TextBoxForNG(m => m.fooMinLength);
            // Assert
            Assert.Equal(@"<input id=""fooMinLength"" name=""fooMinLength"" ng-minlength=""1"" type=""text"" value=""fooMinLength"" />"
                , html.ToHtmlString());
        }

        [Fact]
        public void TextBoxForNG_With_UrlValdation()
        {
            // Arrange
            HtmlHelper<FooBarModel> helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

            // Act
            MvcHtmlString html = helper.TextBoxForNG(m => m.fooUrl);
            // Assert
            Assert.Equal(@"<input id=""fooUrl"" name=""fooUrl"" type=""url"" value=""fooUrl"" />"
                , html.ToHtmlString());
        }

        [Fact]
        public void TextBoxForNG_With_NumberValdation()
        {
            // Arrange
            HtmlHelper<FooBarModel> helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

            // Act
            MvcHtmlString html = helper.TextBoxForNG(m => m.fooInt);
            // Assert
            Assert.Equal(@"<input id=""fooInt"" name=""fooInt"" required=""required"" type=""number"" value=""0"" />"
                , html.ToHtmlString());
        }

        [Fact]
        public void TextBoxForNG_With_EmailValdation()
        {
            // Arrange
            HtmlHelper<FooBarModel> helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

            // Act
            MvcHtmlString html = helper.TextBoxForNG(m => m.fooEmail);
            // Assert
            Assert.Equal(@"<input id=""fooEmail"" name=""fooEmail"" type=""email"" value=""fooEmail"" />"
                , html.ToHtmlString());
        }

        [Fact]
        public void TextBoxForWithAttributesDictionary()
        {
            // Arrange
            HtmlHelper<FooBarModel> helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

            // Act
            MvcHtmlString html = helper.TextBoxForNG(m => m.fooRequired, _attributesDictionary);

            // Assert
            Assert.Equal(@"<input baz=""BazValue"" id=""fooRequired"" name=""fooRequired"" required=""required"" type=""text"" value=""fooRequired"" />", html.ToHtmlString());
        }

        //[Fact]
        //public void TextBoxForWithAttributesObject()
        //{
        //    // Arrange
        //    HtmlHelper<FooBarModel> helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

        //    // Act
        //    MvcHtmlString html = helper.TextBoxFor(m => m.foo, _attributesObjectDictionary);

        //    // Assert
        //    Assert.Equal(@"<input baz=""BazObjValue"" id=""foo"" name=""foo"" type=""text"" value=""ViewItemFoo"" />", html.ToHtmlString());
        //}

        //[Fact]
        //public void TextBoxForWithAttributesObjectWithUnderscores()
        //{
        //    // Arrange
        //    HtmlHelper<FooBarModel> helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

        //    // Act
        //    MvcHtmlString html = helper.TextBoxFor(m => m.foo, _attributesObjectUnderscoresDictionary);

        //    // Assert
        //    Assert.Equal(@"<input foo-baz=""BazObjValue"" id=""foo"" name=""foo"" type=""text"" value=""ViewItemFoo"" />", html.ToHtmlString());
        //}

        //[Fact]
        //public void TextBoxForWithSimpleExpression_Unobtrusive()
        //{
        //    // Arrange
        //    HtmlHelper<FooBarModel> helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());
        //    helper.ViewContext.ClientValidationEnabled = true;
        //    helper.ViewContext.UnobtrusiveJavaScriptEnabled = true;
        //    helper.ViewContext.FormContext = new FormContext();
        //    helper.ClientValidationRuleFactory = (name, metadata) => new[] { new ModelClientValidationRule { ValidationType = "type", ErrorMessage = "error" } };

        //    // Act
        //    MvcHtmlString html = helper.TextBoxFor(m => m.foo);

        //    // Assert
        //    Assert.Equal(@"<input data-val=""true"" data-val-type=""error"" id=""foo"" name=""foo"" type=""text"" value=""ViewItemFoo"" />", html.ToHtmlString());
        //}



        // MODELS
        private class FooBarModel
        {
            [Required]
            public string fooRequired { get; set; }
            
            [StringLength(20,MinimumLength=10)]
            public string fooStringLength { get; set; }

            [MaxLength(20)]
            public string fooMaxLength { get; set; }

            [MinLength(1)]
            public string fooMinLength { get; set; }

            [Url]
            public string fooUrl { get; set; }

            public int fooInt { get; set; }

            [EmailAddress]
            public string fooEmail { get; set; }
            //public override string ToString()
            //{
            //    return String.Format("{{ foo = {0}, bar = {1} }}", foo ?? "(null)", bar ?? "(null)");
            //}
        }

        private static ViewDataDictionary<FooBarModel> GetTextBoxViewData()
        {
            ViewDataDictionary<FooBarModel> viewData = new ViewDataDictionary<FooBarModel> { { "foo", "ViewDataFoo" } };
            //viewData.Model = new FooBarModel { foo = "ViewItemFoo", bar = "ViewItemBar" };
            viewData.Model = new FooBarModel { 
                fooRequired = "fooRequired", 
                fooStringLength = "fooStringLength",
                fooMaxLength = "fooMaxLength",
                fooMinLength = "fooMinLength",
                fooUrl = "fooUrl",
                fooInt = 0,
                fooEmail = "fooEmail"
            };

            return viewData;
        }
    }
}
