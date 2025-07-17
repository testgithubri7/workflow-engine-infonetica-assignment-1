# workflow-engine-infonetica-assignment-1

# Workflow State Machine Service

A minimal backend service for managing configurable workflow state machines built with .NET 8 and ASP.NET Core minimal APIs.

## Features

- **Workflow Definitions**: Create and manage workflow definitions with states and actions
- **State Management**: Define states with initial/final flags and enable/disable capabilities
- **Action Transitions**: Configure actions that transition between states with validation
- **Workflow Instances**: Start and manage running workflow instances
- **History Tracking**: Track action execution history for each instance
- **Validation**: Comprehensive validation for definitions and action executions

## API Endpoints

### Workflow Definitions
- `POST /api/definitions` - Create a new workflow definition
- `GET /api/definitions` - List all workflow definitions
- `GET /api/definitions/{id}` - Get a specific workflow definition

### Workflow Instances
- `POST /api/instances` - Start a new workflow instance
- `GET /api/instances` - List all workflow instances
- `GET /api/instances/{id}` - Get a specific workflow instance
- `POST /api/instances/{id}/execute` - Execute an action on an instance

### Health
- `GET /health` - Health check endpoint

## Running the Service

```bash
dotnet run
```

The service will start on `https://localhost:7000` (or `http://localhost:5000`) with Swagger UI available at `/swagger`.

## Example Usage

### 1. Create a Workflow Definition

```json
POST /api/definitions
{
  "name": "Document Approval",
  "description": "Simple document approval workflow",
  "states": [
    {
      "id": "draft",
      "name": "Draft",
      "isInitial": true,
      "isFinal": false,
      "enabled": true
    },
    {
      "id": "review",
      "name": "Under Review",
      "isInitial": false,
      "isFinal": false,
      "enabled": true
    },
    {
      "id": "approved",
      "name": "Approved",
      "isInitial": false,
      "isFinal": true,
      "enabled": true
    },
    {
      "id": "rejected",
      "name": "Rejected",
      "isInitial": false,
      "isFinal": true,
      "enabled": true
    }
  ],
  "actions": [
    {
      "id": "submit",
      "name": "Submit for Review",
      "enabled": true,
      "fromStates": ["draft"],
      "toState": "review"
    },
    {
      "id": "approve",
      "name": "Approve",
      "enabled": true,
      "fromStates": ["review"],
      "toState": "approved"
    },
    {
      "id": "reject",
      "name": "Reject",
      "enabled": true,
      "fromStates": ["review"],
      "toState": "rejected"
    }
  ]
}
```

### 2. Start a Workflow Instance

```json
POST /api/instances
{
  "definitionId": "{definition-id-from-step-1}"
}
```

### 3. Execute an Action

```json
POST /api/instances/{instance-id}/execute
{
  "actionId": "submit"
}
```

## Design Decisions & Assumptions

### Architecture
- **Minimal APIs**: Used ASP.NET Core minimal APIs for simplicity and reduced boilerplate
- **In-Memory Storage**: Using dictionaries for persistence as specified in requirements
- **Service Layer**: Separated business logic into a service interface for testability

### Validation Rules
- Workflow definitions must have exactly one initial state
- No duplicate state or action IDs within a definition
- Actions can only reference existing states
- Actions cannot be executed from final states
- Actions must be enabled and valid for the current state

### Data Model
- **States**: Include all required fields plus optional description
- **Actions**: Support multiple source states but single target state
- **History**: Tracks action executions with timestamps
- **Instances**: Reference their definition and maintain current state

### Error Handling
- Validation errors return 400 Bad Request with descriptive messages
- Not found resources return 404
- Invalid operations return 400 with specific error details

## Potential Enhancements (TODOs)

With more time, I would add:

- [ ] Persistent storage (database integration)
- [ ] Authentication and authorization
- [ ] Workflow versioning
- [ ] Bulk operations
- [ ] Advanced querying and filtering
- [ ] Workflow templates
- [ ] Event notifications/webhooks
- [ ] Performance monitoring and metrics
- [ ] Comprehensive unit and integration tests
- [ ] API rate limiting
- [ ] Input sanitization and security hardening

## Testing

The service includes Swagger UI for interactive testing. Unit tests could be added using xUnit with the service interface making it easily mockable.

## Time Investment

This implementation was completed in approximately 2 hours, focusing on core functionality and clean architecture while keeping dependencies minimal.
