using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Nest;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(ContainerBuilder =>
{
    ContainerBuilder.RegisterType<WebApplication1.Api.Config>().As<WebApplication1.Api.IConfig>();
});
builder.Services.AddSingleton<IElasticClient>(provider =>
{
    var connectionPool = new SingleNodeConnectionPool(new Uri("http://47.94.210.202:9200/"));
    var connectionSetting = new ConnectionSettings(connectionPool).DisableDirectStreaming();
    return new ElasticClient(connectionSetting);
});


builder.Services.AddSwaggerGen(options => options.SwaggerDoc("v1", new OpenApiInfo
{
    Version = "v1.0",
    Title = "SwaggerShow",
    Description = "接口说明文档",
    Contact = new OpenApiContact { Name = "张三", Email = "59531876@sina.com" }

}));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    c.RoutePrefix = string.Empty; });


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

