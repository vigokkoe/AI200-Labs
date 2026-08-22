# SearchBootstrapper

## Purpose

`SearchBootstrapper` is responsible for creating and validating the Azure AI Search index schema used by document chunks.

It ensures the configured index exists and creates it if missing.

## Architecture

```mermaid
flowchart LR
    A[appsettings + env vars] --> B[SearchIndexClient]
    B --> C[SearchProvisioner]
    C --> D[DocumentChunkIndexBuilder]
    D --> E[Azure AI Search index]
```

## Azure AI Search Configuration

The app reads:

- `AzureSearch:Endpoint`
- `AzureSearch:IndexName`
- `AzureSearch__ApiKey` (environment variable)

`SearchProvisioner` lists existing indexes, logs schemas, and creates the target index if it does not already exist.

## Search Index

The index schema is built from `SearchBootstrapper.Models.DocumentChunk` using `FieldBuilder`.

Fields:

- `Id` (key)
- `FileName` (searchable, filterable, sortable)
- `Title` (searchable)
- `Content` (searchable)
- `ChunkNumber` (filterable, sortable)
- `Source` (filterable)
- `ContentVector` (vector field inferred from model type)

Vector search configuration created by code:

- Profile: `vector-profile`
- Algorithm configuration: `hnsw`

## Technologies

- .NET 10 / C#
- Azure AI Search SDK (`Azure.Search.Documents`)
- Microsoft.Extensions.Hosting and options/configuration

## What I Implemented

- Index provisioning flow with existence checks.
- Code-defined schema generation from strongly typed models.
- Vector search profile and HNSW algorithm configuration in index definition.
- Index schema logging for visibility during provisioning.

## Configuration

Place non-secret values in `appsettings.shared.json` / `SearchBootstrapper/appsettings.json` and keep API keys in environment variables.

Required:

- `AzureSearch:Endpoint`
- `AzureSearch:IndexName`
- `AzureSearch__ApiKey`

## Running

From repository root:

```bash
dotnet run --project SearchBootstrapper/SearchBootstrapper.csproj
```

## Related Documentation

- [docs/learning-notes/Azure-AI-Search.md](../docs/learning-notes/Azure-AI-Search.md)
- [infra/readme.md](../infra/readme.md)
