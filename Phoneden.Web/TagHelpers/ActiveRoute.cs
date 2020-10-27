namespace Phoneden.Web.TagHelpers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Microsoft.AspNetCore.Mvc.Rendering;
  using Microsoft.AspNetCore.Mvc.ViewFeatures;
  using Microsoft.AspNetCore.Razor.TagHelpers;

  [HtmlTargetElement(Attributes = "is-active-route")]
  public class ActiveRoute : TagHelper
  {
    private IDictionary<string, string> _routeValues;

    [HtmlAttributeName("asp-action")]
    public string Action { get; set; }

    [HtmlAttributeName("asp-controller")]
    public string Controller { get; set; }

    [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
    public IDictionary<string, string> RouteValues
    {
      get => _routeValues ?? (_routeValues =
               new Dictionary<string, string>(
                 StringComparer.OrdinalIgnoreCase));
      set => _routeValues = value;
    }

    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      base.Process(context, output);

      if (ShouldBeActive())
      {
        MakeActive(output);
      }

      output.Attributes.RemoveAll("is-active-route");
    }

    private bool ShouldBeActive()
    {
      string currentController = ViewContext.RouteData.Values["Controller"].ToString();
      string currentAction = ViewContext.RouteData.Values["Action"].ToString();

      if (!string.IsNullOrWhiteSpace(Controller) && !string.Equals(Controller, currentController, StringComparison.CurrentCultureIgnoreCase))
      {
        return false;
      }

      if (!string.IsNullOrWhiteSpace(Action) && !string.Equals(Action, currentAction, StringComparison.CurrentCultureIgnoreCase))
      {
        return false;
      }

      return RouteValues.All(routeValue => ViewContext.RouteData.Values.ContainsKey(routeValue.Key) && ViewContext.RouteData.Values[routeValue.Key].ToString() == routeValue.Value);
    }

    private static void MakeActive(TagHelperOutput output)
    {
      var classAttribute = output.Attributes.FirstOrDefault(a => a.Name == "class");
      if (classAttribute == null)
      {
        classAttribute = new TagHelperAttribute("class", "active");
        output.Attributes.Add(classAttribute);
      }
      else if (classAttribute.Value == null || classAttribute.Value.ToString().IndexOf("active", StringComparison.Ordinal) < 0)
      {
        output.Attributes.SetAttribute("class", classAttribute.Value == null
          ? "active"
          : classAttribute.Value + " active");
      }
    }
  }
}
