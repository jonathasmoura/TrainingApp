using Microsoft.OpenApi;
using TraininApp.API.Configuration;
using TraininApp.API.Realtime;
using TraininApp.API.Swagger;
using TraininApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.DocumentFilter<TagDescriptionsDocumentFilter>();

    var definitions = new[]
    {
        (Name: "auth", Title: "Autenticação"),
        (Name: "members", Title: "Membros"),
        (Name: "professionals", Title: "Profissionais"),
        (Name: "plans", Title: "Planos"),
        (Name: "enrollments", Title: "Matrículas"),
        (Name: "exercises", Title: "Exercícios"),
        (Name: "installments", Title: "Parcelas"),
        (Name: "workout-plans", Title: "Fichas de treino")
    };

    foreach (var definition in definitions)
    {
        var description = definition.Name switch
        {
            "auth" => "Funcionalidades de cadastro, login, renovação de token e encerramento de sessão dos usuários.",
            "members" => "Funcionalidades de consulta, atualização e exclusão dos membros cadastrados na academia.",
            "professionals" => "Funcionalidades de consulta, atualização e exclusão dos profissionais da academia.",
            "plans" => "Funcionalidades de criação, consulta, atualização e exclusão dos planos de treinamento.",
            "enrollments" => "Descrição geral: estes endpoints são responsáveis por criar matrículas, alterar o plano contratado e cancelar matrículas de membros da academia.",
            "exercises" => "Funcionalidades de cadastro, consulta, atualização e exclusão dos exercícios de treinamento.",
            "installments" => "Funcionalidades de consulta de parcelas pendentes e registro de pagamentos.",
            "workout-plans" => "Funcionalidades de criação, consulta e atualização das fichas de treino dos membros.",
            _ => "Funcionalidades da aplicação de gerenciamento da academia."
        };

        options.SwaggerDoc(definition.Name, new OpenApiInfo
        {
            Title = $"TraininApp API - {definition.Title}",
            Version = "v1",
            Description = description
        });
    }

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TraininApp API",
        Version = "v1",
        Description = "Documento de compatibilidade para versões anteriores do Swagger UI."
    });

    options.DocInclusionPredicate((documentName, apiDescription) =>
        documentName == "v1" || apiDescription.GroupName == documentName);

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter a valid JWT token using the Bearer scheme."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});
builder.Services.AddSignalR();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/auth/swagger.json", "Autenticação");
        options.SwaggerEndpoint("/swagger/members/swagger.json", "Membros");
        options.SwaggerEndpoint("/swagger/professionals/swagger.json", "Profissionais");
        options.SwaggerEndpoint("/swagger/plans/swagger.json", "Planos");
        options.SwaggerEndpoint("/swagger/enrollments/swagger.json", "Matrículas");
        options.SwaggerEndpoint("/swagger/exercises/swagger.json", "Exercícios");
        options.SwaggerEndpoint("/swagger/installments/swagger.json", "Parcelas");
        options.SwaggerEndpoint("/swagger/workout-plans/swagger.json", "Fichas de treino");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<PaymentsHub>("/hubs/payments");

app.Run();

