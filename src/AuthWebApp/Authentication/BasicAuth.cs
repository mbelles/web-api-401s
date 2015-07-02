using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using System;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AuthWebApp.Authentication
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class BasicAuth : ActionFilterAttribute
	{
		public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			if (Authorize(context))
			{
				await next();
				return;
			}

			context.Result = new HttpUnauthorizedResult();
			
			var request = context.HttpContext.Request;
			var response = context.HttpContext.Response;
			var dnsSafeHost = "locahost";

			response.Headers.Add("WWW-Authenticate", new string[] { $"Basic realm={dnsSafeHost}" });
			response.Challenge();
		}

		public BasicAuthIdentity GetBasicAuth(HttpRequest request)
		{
			BasicAuthIdentity identity = null;
			var headerValue = request.Headers["Authorization"];
			if (!String.IsNullOrEmpty(headerValue))
			{
				var headerValues = headerValue.Split(' ');
				var scheme = headerValues[0];
				if (string.Compare(scheme, "Basic", true) == 0)
				{
					var encodedUsernameAndPassword = headerValues[1];
					
					var usernameAndPassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernameAndPassword));
					if (usernameAndPassword != null && usernameAndPassword != ":")
					{
						var tokens = usernameAndPassword.Split(':');
						switch (tokens.Length)
						{
							case 2:
								identity = new BasicAuthIdentity(tokens[0], tokens[1]);
								break;
							case 1:
								identity = new BasicAuthIdentity(tokens[0], null);
								break;
							default:
								identity = null;
								break;
						}
					}
				}
			}

			return identity;
		}

		public bool Authorize(FilterContext filterContext, string roles = null)
		{
			var identity = GetBasicAuth(filterContext.HttpContext.Request);
			var allowAnonymous = filterContext.Filters.Any(f => f is IAllowAnonymous);
			var authorized = Authorize(filterContext.HttpContext, identity, roles) || allowAnonymous;
			return authorized;
		}

		public bool Authorize(HttpContext httpContext, BasicAuthIdentity identity, string roles)
		{
			if (identity == null)
				return false;

			return identity.Username == identity.Password;
		}
	}

	public class BasicAuthIdentity : GenericIdentity
	{
		public BasicAuthIdentity(string username, string password)
			: base(username, "Basic")
		{
			this.Username = username;
			this.Password = password;
		}

		public string Username { get; private set; }
		public string Password { get; private set; }
	}
}
