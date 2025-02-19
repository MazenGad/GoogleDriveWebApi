using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

public class SwaggerFileUploadOperationFilter : IOperationFilter
{
	public void Apply(OpenApiOperation operation, OperationFilterContext context)
	{
		if (operation.Parameters == null)
			operation.Parameters = new List<OpenApiParameter>();

		var fileParams = context.ApiDescription.ParameterDescriptions
			.Where(p => p.Type == typeof(IFormFile))
			.ToList();

		if (fileParams.Any())
		{
			operation.RequestBody = new OpenApiRequestBody
			{
				Content = new Dictionary<string, OpenApiMediaType>
				{
					["multipart/form-data"] = new OpenApiMediaType
					{
						Schema = new OpenApiSchema
						{
							Type = "object",
							Properties = fileParams.ToDictionary(
								p => p.Name,
								p => new OpenApiSchema { Type = "string", Format = "binary" }
							)
						}
					}
				}
			};
		}
	}
}
