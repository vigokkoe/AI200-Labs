# Azure AI Search Notes

> These are personal learning and implementation notes created while developing the AI-200 project. They are retained for reference and are not part of the application documentation.

## Service and Key Checks

```bash
RG=<resource-group>
SEARCH_NAME=<search-service-name>

# Verify search service
az search service show \
  --name $SEARCH_NAME \
  --resource-group $RG \
  --output table

# Retrieve admin key for local .env value AzureSearch__ApiKey
az search admin-key show \
  --resource-group $RG \
  --service-name $SEARCH_NAME \
  --query primaryKey \
  -o tsv
```

## List Indexes

```bash
# Get admin key
ADMIN_KEY=$(az search admin-key show \
  --resource-group $RG \
  --service-name $SEARCH_NAME \
  --query primaryKey \
  -o tsv)

# List indexes via REST
curl -X GET \
  "https://$SEARCH_NAME.search.windows.net/indexes?api-version=2025-09-01" \
  -H "api-key: $ADMIN_KEY"

# List index names via az rest
az rest --method get \
  --uri "https://$SEARCH_NAME.search.windows.net/indexes?api-version=2024-07-01" \
  --headers "api-key=$ADMIN_KEY" \
  --query "value[].name" \
  -o tsv
```

## Delete an Index

```bash
ADMIN_KEY=$(az search admin-key show \
  --service-name "$SEARCH_NAME" \
  --resource-group "$RG" \
  --query primaryKey \
  -o tsv)

az rest \
  --method delete \
  --url "https://$SEARCH_NAME.search.windows.net/indexes/<index-name>?api-version=2024-07-01" \
  --headers "api-key=$ADMIN_KEY"
```

## Delete Search Service

```bash
az search service delete \
  --name $SEARCH_NAME \
  --resource-group $RG

az search service list --resource-group $RG
```

## Practical Notes

- The project bootstrapper creates the index if it does not exist.
- Index schema is code-defined in `SearchBootstrapper` and should stay as source of truth.
- Prefer deleting specific indexes over deleting the whole search service during iteration.
