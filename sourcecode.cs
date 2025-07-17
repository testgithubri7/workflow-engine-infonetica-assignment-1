using WorkflowService.Services;
using WorkflowService.Models;
using WorkflowService.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddSingleton<IWorkflowService, WorkflowServiceImpl>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Workflow Definition endpoints
app.MapPost("/api/definitions", async (CreateWorkflowDefinitionRequest request, IWorkflowService workflowService) =>
{
    try
    {
        var definition = await workflowService.CreateDefinitionAsync(
            request.Name, 
            request.States, 
            request.Actions, 
            request.Description);
        return Results.Created($"/api/definitions/{definition.Id}", definition);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
.WithName("CreateWorkflowDefinition")
.WithOpenApi();

app.MapGet("/api/definitions", async (IWorkflowService workflowService) =>
{
    var definitions = await workflowService.GetAllDefinitionsAsync();
    return Results.Ok(definitions);
})
.WithName("GetAllDefinitions")
.WithOpenApi();

app.MapGet("/api/definitions/{id}", async (string id, IWorkflowService workflowService) =>
{
    var definition = await workflowService.GetDefinitionAsync(id);
    return definition != null ? Results.Ok(definition) : Results.NotFound();
})
.WithName("GetDefinition")
.WithOpenApi();

// Workflow Instance endpoints
app.MapPost("/api/instances", async (StartInstanceRequest request, IWorkflowService workflowService) =>
{
    try
    {
        var instance = await workflowService.StartInstanceAsync(request.DefinitionId);
        return Results.Created($"/api/instances/{instance.Id}", instance);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
.WithName("StartWorkflowInstance")
.WithOpenApi();

app.MapGet("/api/instances", async (IWorkflowService workflowService) =>
{
    var instances = await workflowService.GetAllInstancesAsync();
    return Results.Ok(instances);
})
.WithName("GetAllInstances")
.WithOpenApi();

app.MapGet("/api/instances/{id}", async (string id, IWorkflowService workflowService) =>
{
    var instance = await workflowService.GetInstanceAsync(id);
    return instance != null ? Results.Ok(instance) : Results.NotFound();
})
.WithName("GetInstance")
.WithOpenApi();

app.MapPost("/api/instances/{id}/execute", async (string id, ExecuteActionRequest request, IWorkflowService workflowService) =>
{
    try
    {
        var instance = await workflowService.ExecuteActionAsync(id, request.ActionId);
        return Results.Ok(instance);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
.WithName("ExecuteAction")
.WithOpenApi();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
.WithName("HealthCheck")
.WithOpenApi();

app.Run();
