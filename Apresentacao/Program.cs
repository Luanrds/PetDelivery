using Aplicacao.Fachadas.UseProduto;
using Dominio.Interfaces.InterfaceProduct;
using Infrastucture.Configuracao;
using Infrastucture.Repositorio.Repositorios;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ProdutoFacade>();

builder.Services.AddScoped<RepositoryProduct>();

builder.Services.AddScoped<IProduct, RepositoryProduct>();

builder.Services.AddControllers();

builder.Services.AddDbContext<ContextBase>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
