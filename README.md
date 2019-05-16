# DiagnosticListener Demo

DiagnosticListener intends to decouple library/app from the Tracing system so any library/app can use DiagnosticSource 
to notify any consumer about interesting operations.

## Wiring it Up
In the ConfigureServices we have to register MyDiagnosticListenerHandler as Hosted service:

```csharp
/// <summary>
/// A Demo showing how to use DiagnosticListener to handle request events
/// </summary>
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseMvc();
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHostedService<MyDiagnosticListenerHandler>(); // Register MyDiagnosticListenerHandler as Hosted service
        services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
    }
}
```

If you run this application from Visual Studio, you'll notice new entries in the Output window: 



## Summary
If you want to rely on something available on the platform so that 
any app/library can use it without bringing new extra dependencies - DiagnosticListener is your best choice. Give it a try!

## Prerequisites
- [Visual Studio](https://www.visualstudio.com/vs/community) 2017 15.9 or greater

## Licence

Licenced under [MIT](http://opensource.org/licenses/mit-license.php).
Contact me on [LinkedIn](https://si.linkedin.com/in/matjazbravc)