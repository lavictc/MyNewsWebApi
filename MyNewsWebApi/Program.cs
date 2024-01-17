using MyNewsWebApi;
using MyNewsWebApi.Infrastructure;
using MyNewsWebApi.Infrastructure.IoC;
using MyNewsWebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterModule<MyNewsWebApiModule>();
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/thebeststories/{n}", async (IStoryService newsStoryService, int n) => await newsStoryService.GetTheBestStories(n)).WithName("GetTheBestStories").WithOpenApi();

app.Run();
