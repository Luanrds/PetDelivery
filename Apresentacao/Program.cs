using Aplicacao;
using Infraestrutura;
using Infraestrutura.Extensoes;
using Infraestrutura.Migrations;
using PetDelivery.API.Conversoes;
using PetDelivery.API.Filtros;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new ConvertaString()));

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

builder.Services.AdicioneAplicacao();
builder.Services.AdicioneInfraestrutura(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

var corsPolicy = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: corsPolicy,
		policy =>
		{
			policy.WithOrigins("http://127.0.0.1:5501") // Adicione a origem do seu frontend
				  .AllowAnyMethod()
				  .AllowAnyHeader();
		});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
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
