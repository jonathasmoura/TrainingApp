using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TraininApp.API.Swagger;

public sealed class TagDescriptionsDocumentFilter : IDocumentFilter
{
	private static readonly IReadOnlyDictionary<string, string> Descriptions =
		new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			["Auth"] = "Gerencia o cadastro, a autenticação, a renovação de tokens e o encerramento de sessões dos usuários.",
			["Members"] = "Gerencia a consulta, a atualização e a exclusão dos membros cadastrados na academia.",
			["Professionals"] = "Gerencia a consulta, a atualização e a exclusão dos profissionais da academia.",
			["Plans"] = "Gerencia a criação, a consulta, a atualização e a exclusão dos planos de treinamento.",
			["Enrollments"] = "Gerencia as matrículas dos membros, incluindo a criação, a alteração do plano e o cancelamento.",
			["Exercises"] = "Gerencia o cadastro, a consulta, a atualização e a exclusão dos exercícios de treinamento.",
			["Installments"] = "Gerencia a consulta das parcelas pendentes e o registro de pagamentos.",
			["WorkoutPlans"] = "Gerencia a criação, a consulta e a atualização das fichas de treino dos membros."
		};

	public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
	{
		foreach (var tag in swaggerDoc.Tags)
		{
			if (Descriptions.TryGetValue(tag.Name, out var description))
			{
				tag.Description = description;
			}
		}
	}
}
