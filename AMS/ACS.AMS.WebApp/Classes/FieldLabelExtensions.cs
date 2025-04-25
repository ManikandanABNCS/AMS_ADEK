using ACS.AMS.DAL;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.CodeDom;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace ACS.AMS.WebApp
{
    public static class FieldLabelExtensions
    {
        public static IHtmlContent FieldLabelFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, bool isMandatoryField = false)
        {
            var nm = expression.Name;

            MemberExpression exp = expression.Body as MemberExpression;
            var newName = exp.Member.Name;

            //expression.Body as PropertyExpression;
            //var al = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData);

            //return LabelHelper(html, ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData), ExpressionHelper.GetExpressionText((LambdaExpression)expression), null,
            //    string.Empty, "", isMandatoryField);

            if(isMandatoryField)
                return new HtmlString($"<label for='{newName}' class='fieldLabel'>{Language.GetString(newName)}&nbsp;&nbsp;<span style=\"color: red\">*<span></label>");
            else
                return new HtmlString($"<label for='{newName}' class='fieldLabel'>{Language.GetString(newName)}</label>");
        }

        public static IHtmlContent FieldLabelFor(this IHtmlHelper html, string labelText, bool isMandatoryField = false)
        {
            if (isMandatoryField)
                return new HtmlString($"<label class='fieldLabel'>{Language.GetString(labelText)}&nbsp;&nbsp;<span style=\"color: red\">*<span></label>");
            else
                return new HtmlString($"<label class='fieldLabel'>{Language.GetString(labelText)}</label>");
        }

        public static IHtmlContent FieldLabel(this IHtmlHelper html, string labelText, bool isMandatoryField = false)
        {
            if (isMandatoryField)
                return new HtmlString($"<label class='fieldLabel'>{Language.GetString(labelText)}&nbsp;&nbsp;<span style=\"color: red\">*<span></label>");
            else
                return new HtmlString($"<label class='fieldLabel'>{Language.GetString(labelText)}</label>");
        }
    }
}