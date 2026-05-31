# AI Content Assistant

This solution contains two separate Web API projects:

- Service A: Content API
- Service B: LLM Proxy API

Service A calls Service B when AI-generated content is needed.  
Because of this, both projects must be running at the same time.

## Run the projects locally

Open the solution in Visual Studio.

1. Right-click the solution in Solution Explorer
2. Choose `Properties`
3. Go to `Startup Project`
4. Select `Multiple startup projects`
5. Set both `Content API` and `LLM Proxy API` to `Start`
6. Press `Start` or `F5`

Make sure both APIs start without errors.

## Local secrets

The project uses secrets for two things:

- an internal API key between Service A and Service B
- an OpenAI API key used by Service B

User Secrets are stored per project, so the commands must be run for the correct project.

## Service A / Content API

Go to the Content API project folder and run:

```bash
dotnet user-secrets init
dotnet user-secrets set "ServiceB:BaseUrl" "https://localhost:7002"
dotnet user-secrets set "ServiceB:ApiKey" "my-internal-key-123"
```

The port in `ServiceB:BaseUrl` must match the port used by Service B locally.

## Service B / LLM Proxy API

Go to the LLM Proxy API project folder and run:

```bash
dotnet user-secrets init
dotnet user-secrets set "InternalApiKey:Value" "my-internal-key-123"
dotnet user-secrets set "OpenAI:BaseUrl" "https://api.openai.com/"
dotnet user-secrets set "OpenAI:Endpoint" "v1/responses"
dotnet user-secrets set "OpenAI:ApiKey" "YOUR_OPENAI_API_KEY"
dotnet user-secrets set "OpenAI:Model" "gpt-4.1-mini"
dotnet user-secrets set "OpenAI:TimeoutSeconds" "30"
```

The internal keys must match:

- `ServiceB:ApiKey` in Service A
- `InternalApiKey:Value` in Service B

If they do not match, Service B will return `401 Unauthorized`.

## Running a local test

Start both APIs.

Then test the API using Scalar, Postman, an `.http` file, or another API client.

Basic flow:

1. Create content in Service A
2. Call the generate endpoint in Service A
3. Service A calls Service B
4. Service B calls OpenAI
5. The generated text is returned to Service A

If the request fails, check that:

- both projects are running
- `ServiceB:BaseUrl` points to the correct Service B port
- `ServiceB:ApiKey` and `InternalApiKey:Value` are the same
- `OpenAI:ApiKey` is set in Service B
- the selected OpenAI model is valid
- Service B can reach the OpenAI API

## Production secrets

In production, secrets should be set with environment variables instead of User Secrets.

Example:

```bash
ServiceB__BaseUrl=https://your-llm-proxy-url
ServiceB__ApiKey=your-internal-api-key

InternalApiKey__Value=your-internal-api-key
OpenAI__BaseUrl=https://api.openai.com/
OpenAI__Endpoint=v1/responses
OpenAI__ApiKey=your-openai-api-key
OpenAI__Model=gpt-4.1-mini
OpenAI__TimeoutSeconds=30
```

The internal API key should be strong and should not be reused outside this application.

## Security notes

API keys must not be stored in the repository.

The project should not contain real keys in:

- `appsettings.json`
- `appsettings.Development.json`
- `.http` files
- README files
- log output

The application should also not log request headers or authorization values.

Errors from the AI provider are returned as safe `ProblemDetails` responses.  
The client receives a clear error message, but internal details and API keys are not exposed.

## AI evaluation

The file `evaluation.md` contains a short evaluation of the AI output.

It includes:

- what counts as good output
- examples of test prompts
- what worked well
- what did not work well
- limitations such as hallucinations, bias, prompt sensitivity and uncertainty
- a short conclusion about when the AI service is useful
