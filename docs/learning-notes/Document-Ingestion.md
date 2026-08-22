# Document Ingestion Notes

> These are personal learning and implementation notes created while developing the AI-200 project. They are retained for reference and are not part of the application documentation.

## Storage and Container Checks

```bash
RG=<resource-group>
STORAGE_ACCOUNT=<storage-account-name>
CONTAINER_NAME=<container-name>

# List containers
az storage container list \
  --account-name $STORAGE_ACCOUNT \
  --auth-mode login \
  --output table

# Retrieve storage account key for local .env value Storage__ApiKey
az storage account keys list \
  --account-name $STORAGE_ACCOUNT \
  --resource-group $RG \
  --query "[0].value" \
  -o tsv
```

## Embedding Model Deployment

```bash
FOUNDRY_NAME=<foundry-account-name>

# Deploy text-embedding-ada-002
az cognitiveservices account deployment create \
  --name $FOUNDRY_NAME \
  --resource-group $RG \
  --deployment-name "text-embedding-ada-002" \
  --model-name "text-embedding-ada-002" \
  --model-version "2" \
  --model-format OpenAI \
  --sku-name Standard \
  --sku-capacity 1

# Verify deployment
az cognitiveservices account deployment list \
  --name $FOUNDRY_NAME \
  --resource-group $RG \
  --output table
```

## Model Discovery

```bash
# List available models by region
az cognitiveservices model list \
  --location <azure-region> \
  --query "[].{Model:model.name, SKU:model.skus[0].name}" \
  -o table
```

## Ingestion Pipeline Notes

- The `DocumentIngestor` app reads PDFs from Blob Storage and extracts text with PdfPig.
- Text is split into sentence-based chunks with overlap using configurable token approximations.
- Embeddings are generated per chunk and written to JSON files under `output/`.
- Current implementation writes local artifacts; indexing into Azure AI Search is handled as a separate concern by `SearchBootstrapper`.
