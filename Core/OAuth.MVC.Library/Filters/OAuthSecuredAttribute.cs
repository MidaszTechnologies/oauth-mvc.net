using System;
using System.Web.Mvc;
using Ninject.Core;
using OAuth.MVC.Library.Interfaces;
using OAuth.MVC.Library.Results;
using System.Linq;
namespace OAuth.MVC.Library.Filters
{
  public class OAuthSecuredAttribute : ActionFilterAttribute, IAuthorizationFilter
  {
    [Inject]
    public IOAuthService OAuthService { get; set; }


    public void OnAuthorization(AuthorizationContext filterContext)
    {
      if (OAuthService == null)
      {
        throw new NullReferenceException("OAuthService needs to be set for OAuthSecuredAttribute, please use an IOC Container to do this");
      }
      var request = filterContext.HttpContext.Request;
      var oauthRequest = OAuthService.BuildRequest(filterContext.HttpContext.Request.Url, request.HttpMethod,request.Params,
                                                   request.Headers, OAuthConstants.EndPointType.AccessRequest);
      if (!oauthRequest.IsValid())
      {
        filterContext.Result = new OAuthUnauthorizedResult();

        return;
      }
    }
  }
}