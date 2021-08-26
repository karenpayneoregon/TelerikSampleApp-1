# About

Code sample from Telerik/Progress engineers

This branch changes how a connection is made to the backend database.

:heavy_check_mark: now uses appsetting.json


[Link](https://weblog.west-wind.com/posts/2019/May/18/Live-Reloading-Server-Side-ASPNET-Core-Apps#proxying-aspnet-core-for-code-injection) for the following

### dotnet watch run

You run this command in your Web project folder. It works the same as plain dotnet run, except it also watches files and restarts when a change is detected. watch is just a pass-through command, so all the dotnet run (or whatever other command you use) command line parameters work with dotnet watch run.

dotnet watch run monitors source files and if a file changes, shuts down the application that it started, rebuilds and publishes the project, and then restarts the application.

It's a simple, single focus tool, but it's one of the most useful dotnet tools that ship in the box. Personally, I use almost explicitly for running my .NET application during development with the exception when I'm explicitly debugging code.

```
dotnet watch run
```

### Browser Sync

dotnet watch handles the server side reloading of code and Browser Sync provides the second piece in this setup that refreshes the browser when either server side or 'client side' code and markup is changed.

Browser Sync is an easy to use web server/proxy that provides a very easy and totally generic way to provide simple browser page reloading. It's a Node component and it installs as a global NPM tool:

```
npm install -g browser-sync
```

After that you can just use browser-sync from your terminal as it's available on your Path.

### Proxying ASP.NET Core for Code Injection

Browser-sync can run as a Web server to serve static files directly out of a folder, or - more useful for server side scenarios - it can act as a proxy for an already running Web site - like say... an ASP.NET Core Web site.

The proxy intercepts each request via a different site port (localhost:3000 by default), injects some web socket script code into each HTML page it renders, and then routes the request to the original site URL (localhost:5000 typically for ASP.NET Core).

Once running any changes made to files in the folder cause the browser to refresh the currently active page and remember its scroll position.

You can run Browser Sync from the command line like this:

```
browser-sync start `
            --proxy http://localhost:5000/ `
            --files '**/*.cshtml, **/*.css, **/*.js, **/*.htm*'
```

which detects changes for all of the different files you want to monitor. You can add add folders, files or wild cards as I've done here. The above only handles client side files, plus Razor pages - I'll come back how we can also detect and refresh on server side application changes.


I put the above into a browsersync.ps1 script file in my project folder so it's easier to launch.



