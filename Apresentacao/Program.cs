using Aplicacao;
using Dominio.Seguranca.Tokens;
using Infraestrutura;
using Infraestrutura.Extensoes;
using Infraestrutura.Migrations;
using Infraestrutura.Servicos.Background;
using Microsoft.OpenApi.Models;
using PetDelivery.API.Conversoes;
using PetDelivery.API.Filtros;
using PetDelivery.API.Token;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
	options.Filters.Add(typeof(ExceptionFilter));
})
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.Converters.Add(new ConvertaString());
		options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
	});

builder.Services.AdicioneAplicacao();
builder.Services.AdicioneInfraestrutura(builder.Configuration);
builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
	});
	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				},
				Scheme = "oauth2",
				Name = "Bearer",
				In = ParameterLocation.Header
			},
			new List<string>()
		}
	});
});

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddHttpContextAccessor();

var corsPolicy = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
	options.AddPolicy(name: corsPolicy,
		policy =>
		{
			policy.WithOrigins("http://127.0.0.1:5501")
				  .AllowAnyMethod()
				  .AllowAnyHeader();
		});
});

builder.Services.AddHostedService<ProcessadorPagamentoPendenteService>();
builder.Services.AddHostedService<ProcessadorEntregaPendenteService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors(corsPolicy);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

MigrateDataBase();

app.Run();

void MigrateDataBase()
{
	var connectionString = builder.Configuration.ConnectionString();
	var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
	BancoDeDadosMigration.Migrate(connectionString, serviceScope.ServiceProvider);
}