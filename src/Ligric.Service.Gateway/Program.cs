using Ligric.Rpc.Contracts;
using Ligric.Service.Gateway.ExceptionHandler;

const string CORS_POLICY = "_corsPolicy";

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddGrpc();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: CORS_POLICY,
		policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
});

builder.Services.AddControllers(options => { options.Filters.Add(new HttpResponseExceptionActionFilter()); });

builder.Services.AddGrpcClient<Auth.AuthClient>((o) =>
{
	o.Address = new Uri(builder.Configuration["ClientUrls:AuthClient"] ?? throw new ArgumentNullException("AuthClient not found inside configuration."));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.Run();
