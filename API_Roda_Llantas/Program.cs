using API_Roda_Llantas.Interfaces;
using API_Roda_Llantas.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IUsuarioModel, UsuarioModel>();
builder.Services.AddScoped<IBitacoraModel, BitacoraModel>();
builder.Services.AddScoped<ICarritoModel, CarritoModel>();
builder.Services.AddScoped<IVehiculosModel, VehiculosModel>();
builder.Services.AddScoped<IProductosModel, ProductosModel>();
builder.Services.AddScoped<ITipoProductoModel, TipoProductoModel>();
builder.Services.AddScoped<IUtilitariosModel, UtilitariosModel>();
builder.Services.AddScoped<IProveedoresModel, ProveedoresModel>();
builder.Services.AddScoped<IServiciosModel, ServiciosModel>();  
builder.Services.AddScoped<IFacturaModel, FacturaModel>();
builder.Services.AddScoped<IReservacionesModel, ReservacionesModel>();
builder.Services.AddScoped<ICompras, ComprasModel>();

builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateAudience = false,
         ValidateIssuer = false,
         ValidateIssuerSigningKey = true,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("b14ca5898a4e4133bbce2ea2315a1916")),
         ValidateLifetime = true,
         LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters) =>
         {
             if (expires != null)
             {
                 return expires > DateTime.UtcNow;
             }
             return false;
         }
     };
 });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction()) { 
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
