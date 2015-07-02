# web-api-401s

### Abstract
* This repository contains a ASP.NET 5 Web App built using MVC6.
* The goal is to demonstrate how a web api controller method that should return a 401, is redirected via something in the middleware pipeline and redirected to the login.

### Usage
* Open the solution in VS2015.
* Run the web app
* curl -u secret:secret http://localhost:15862/api/widget/3
** Returns the JSON correctly via a 200
* curl -u secret:secret2 http://localhost:15862/api/widget/3
** Redirects to the /Account/Login view, instead of returning a 401 as controlled by the BasicAuth.cs ActionFilterAttribute.

### Goals
* Discuss why this redirection happens.
* Discover a way to disable it such that any REST consumer can accurately receive 401s and not a 401 -> 302 -> 200 and Login view HTML content.
* Mostly tracked and explained here: http://stackoverflow.com/questions/30411296/asp-net-5-vnext-web-api-unauthorized-requests-returns-302-redirect-response-in
