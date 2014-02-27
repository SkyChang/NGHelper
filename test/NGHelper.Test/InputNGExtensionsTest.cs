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
        public void TextBoxForNGWithSimpleExpression()
        {
            // Arrange
            HtmlHelper<FooBarModel> helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

            // Act
            MvcHtmlString html = helper.TextBoxForNG(m => m.foo);
            var tt = html.ToHtmlString();
            // Assert
            Assert.Equal(@"<input id=""foo"" name=""foo"" ng-model=""foo"" type=""text"" value=""ViewItemFoo"" />"
                ,html.ToHtmlString());
        }

        [Fact]
        public void TextBoxForWithAttributesDictionary()
        {
            // Arrange
            HtmlHelper<FooBarModel> helper = MvcHelper.GetHtmlHelper(GetTextBoxViewData());

            // Act
            MvcHtmlString html = helper.TextBoxForNG(m => m.foo, _attributesDictionary);

            // Assert
            Assert.Equal(@"<input baz=""BazValue"" id=""foo"" name=""foo"" ng-model=""foo"" type=""text"" value=""ViewItemFoo"" />", html.ToHtmlString());
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
        private class FooModel
        {
            public string foo { get; set; }

            public override string ToString()
            {
                return String.Format("{{ foo = {0} }}", foo ?? "(null)");
            }
        }

        private class FooBarModel : FooModel
        {
            public string bar { get; set; }

            public override string ToString()
            {
                return String.Format("{{ foo = {0}, bar = {1} }}", foo ?? "(null)", bar ?? "(null)");
            }
        }

        private static ViewDataDictionary<FooBarModel> GetTextBoxViewData()
        {
            ViewDataDictionary<FooBarModel> viewData = new ViewDataDictionary<FooBarModel> { { "foo", "ViewDataFoo" } };
            viewData.Model = new FooBarModel { foo = "ViewItemFoo", bar = "ViewItemBar" };

            return viewData;
        }
    }

    public static class MvcHelper
    {
        public const string AppPathModifier = "/$(SESSION)";

        public static HtmlHelper<object> GetHtmlHelper()
        {
            HttpContextBase httpcontext = GetHttpContext("/app/", null, null);
            RouteCollection rt = new RouteCollection();
            rt.Add(new Route("{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            rt.Add("namedroute", new Route("named/{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            RouteData rd = new RouteData();
            rd.Values.Add("controller", "home");
            rd.Values.Add("action", "oldaction");

            ViewDataDictionary vdd = new ViewDataDictionary();

            ViewContext viewContext = new ViewContext()
            {
                HttpContext = httpcontext,
                RouteData = rd,
                ViewData = vdd
            };
            Mock<IViewDataContainer> mockVdc = new Mock<IViewDataContainer>();
            mockVdc.Setup(vdc => vdc.ViewData).Returns(vdd);

            HtmlHelper<object> htmlHelper = new HtmlHelper<object>(viewContext, mockVdc.Object, rt);
            return htmlHelper;
        }

        public static HtmlHelper GetHtmlHelper(string protocol, int port)
        {
            HttpContextBase httpcontext = GetHttpContext("/app/", null, null, protocol, port);
            RouteCollection rt = new RouteCollection();
            rt.Add(new Route("{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            rt.Add("namedroute", new Route("named/{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            RouteData rd = new RouteData();
            rd.Values.Add("controller", "home");
            rd.Values.Add("action", "oldaction");

            ViewDataDictionary vdd = new ViewDataDictionary();

            Mock<ViewContext> mockViewContext = new Mock<ViewContext>();
            mockViewContext.Setup(c => c.HttpContext).Returns(httpcontext);
            mockViewContext.Setup(c => c.RouteData).Returns(rd);
            mockViewContext.Setup(c => c.ViewData).Returns(vdd);
            Mock<IViewDataContainer> mockVdc = new Mock<IViewDataContainer>();
            mockVdc.Setup(vdc => vdc.ViewData).Returns(vdd);

            HtmlHelper htmlHelper = new HtmlHelper(mockViewContext.Object, mockVdc.Object, rt);
            return htmlHelper;
        }

        public static HtmlHelper GetHtmlHelper(ViewDataDictionary viewData)
        {
            Mock<ViewContext> mockViewContext = new Mock<ViewContext>() { CallBase = true };
            mockViewContext.Setup(c => c.ViewData).Returns(viewData);
            mockViewContext.Setup(c => c.HttpContext.Items).Returns(new Hashtable());
            IViewDataContainer container = GetViewDataContainer(viewData);
            return new HtmlHelper(mockViewContext.Object, container);
        }

        public static HtmlHelper<TModel> GetHtmlHelper<TModel>(ViewDataDictionary<TModel> viewData)
        {
            Mock<ViewContext> mockViewContext = new Mock<ViewContext>() { CallBase = true };
            mockViewContext.Setup(c => c.ViewData).Returns(viewData);
            mockViewContext.Setup(c => c.HttpContext.Items).Returns(new Hashtable());
            IViewDataContainer container = GetViewDataContainer(viewData);
            return new HtmlHelper<TModel>(mockViewContext.Object, container);
        }

        public static HtmlHelper GetHtmlHelperWithPath(ViewDataDictionary viewData)
        {
            return GetHtmlHelperWithPath(viewData, "/");
        }

        public static HtmlHelper GetHtmlHelperWithPath(ViewDataDictionary viewData, string appPath)
        {
            ViewContext viewContext = GetViewContextWithPath(appPath, viewData);
            Mock<IViewDataContainer> mockContainer = new Mock<IViewDataContainer>();
            mockContainer.Setup(c => c.ViewData).Returns(viewData);
            IViewDataContainer container = mockContainer.Object;
            return new HtmlHelper(viewContext, container, new RouteCollection());
        }

        public static HtmlHelper<TModel> GetHtmlHelperWithPath<TModel>(ViewDataDictionary<TModel> viewData, string appPath)
        {
            ViewContext viewContext = GetViewContextWithPath(appPath, viewData);
            Mock<IViewDataContainer> mockContainer = new Mock<IViewDataContainer>();
            mockContainer.Setup(c => c.ViewData).Returns(viewData);
            IViewDataContainer container = mockContainer.Object;
            return new HtmlHelper<TModel>(viewContext, container, new RouteCollection());
        }

        public static HtmlHelper<TModel> GetHtmlHelperWithPath<TModel>(ViewDataDictionary<TModel> viewData)
        {
            return GetHtmlHelperWithPath(viewData, "/");
        }

        public static HttpContextBase GetHttpContext(string appPath, string requestPath, string httpMethod, string protocol, int port)
        {
            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();

            if (!String.IsNullOrEmpty(appPath))
            {
                mockHttpContext.Setup(o => o.Request.ApplicationPath).Returns(appPath);
                mockHttpContext.Setup(o => o.Request.RawUrl).Returns(appPath);
            }
            if (!String.IsNullOrEmpty(requestPath))
            {
                mockHttpContext.Setup(o => o.Request.AppRelativeCurrentExecutionFilePath).Returns(requestPath);
            }

            Uri uri;

            if (port >= 0)
            {
                uri = new Uri(protocol + "://localhost" + ":" + Convert.ToString(port));
            }
            else
            {
                uri = new Uri(protocol + "://localhost");
            }
            mockHttpContext.Setup(o => o.Request.Url).Returns(uri);

            mockHttpContext.Setup(o => o.Request.PathInfo).Returns(String.Empty);
            if (!String.IsNullOrEmpty(httpMethod))
            {
                mockHttpContext.Setup(o => o.Request.HttpMethod).Returns(httpMethod);
            }

            mockHttpContext.Setup(o => o.Session).Returns((HttpSessionStateBase)null);
            mockHttpContext.Setup(o => o.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(r => AppPathModifier + r);
            mockHttpContext.Setup(o => o.Items).Returns(new Hashtable());
            return mockHttpContext.Object;
        }

        public static HttpContextBase GetHttpContext(string appPath, string requestPath, string httpMethod)
        {
            return GetHttpContext(appPath, requestPath, httpMethod, Uri.UriSchemeHttp.ToString(), -1);
        }

        public static ViewContext GetViewContextWithPath(string appPath, ViewDataDictionary viewData)
        {
            HttpContextBase httpContext = GetHttpContext(appPath, "/request", "GET");

            Mock<ViewContext> mockViewContext = new Mock<ViewContext>() { DefaultValue = DefaultValue.Mock };
            mockViewContext.Setup(c => c.HttpContext).Returns(httpContext);
            mockViewContext.Setup(c => c.ViewData).Returns(viewData);
            mockViewContext.Setup(c => c.Writer).Returns(new StringWriter());
            return mockViewContext.Object;
        }

        public static ViewContext GetViewContextWithPath(ViewDataDictionary viewData)
        {
            return GetViewContextWithPath("/", viewData);
        }

        public static IViewDataContainer GetViewDataContainer(ViewDataDictionary viewData)
        {
            Mock<IViewDataContainer> mockContainer = new Mock<IViewDataContainer>();
            mockContainer.Setup(c => c.ViewData).Returns(viewData);
            return mockContainer.Object;
        }
    }
}
