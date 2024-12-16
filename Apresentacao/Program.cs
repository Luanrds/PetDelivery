using Aplicacao;
using Infrastucture;
using PetDelivery.API.Filtros;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddScoped<ProdutoFacade>();

//builder.Services.AddScoped<ProdutoRepository>();

builder.Services.AddControllers();

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

builder.Services.AdicioneAplicacao();
builder.Services.AdicioneInfraestrutura();

//builder.Services.AddDbContext<PetDeliveyContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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
