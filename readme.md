# AI200-Labs

This repository contains a practical study project for Microsoft AI-200 preparation.
It demonstrates an end-to-end Retrieval-Augmented Generation (RAG) workflow on Azure:

- Provision an Azure AI Search index.
- Ingest source documents from storage.
- Extract and chunk text.
- Generate embeddings with Azure OpenAI.
- Store searchable chunks in Azure AI Search.
- Run a chat assistant that answers using indexed knowledge.

## Solution structure

- `infra/` contains helper scripts and notes for Azure cloud resource setup.
- `SearchBootstrapper/` creates and provisions the Azure AI Search schema.
- `DocumentIngestor/` reads source files, extracts text, chunks content, and writes vectors/documents to search.
- `HikingAssistant/` provides a chat interface that retrieves context from search and calls Azure OpenAI.
- `Shared/` contains shared configuration models and extensions used by the apps.

## Technology stack

- .NET 10
- Azure AI Search
- Azure OpenAI
- Azure Storage Blobs

## Local configuration

Use environment variables (or a local `.env` file) for secrets.
Do not commit real keys to Git.

1. Copy `.env.example` to `.env`.
2. Fill in your own values.
3. Keep `.env` local only.

Expected environment variables:

- `AzureOpenAI__ApiKey`
- `AzureSearch__ApiKey`
- `Storage__ApiKey`

Endpoints, deployment names, and non-secret defaults can stay in `appsettings*.json`.

## Build and run

From repository root:

```bash
dotnet restore
dotnet build AI200-Labs.slnx
```

Run apps as needed:

```bash
dotnet run --project SearchBootstrapper/SearchBootstrapper.csproj
dotnet run --project DocumentIngestor/DocumentIngestor.csproj
dotnet run --project HikingAssistant/HikingAssistant.csproj
```

## Security note

If a secret was ever committed or shared, rotate it in Azure and replace it locally.