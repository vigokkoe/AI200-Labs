# 2. Create and migrate Azure AI Search resources (indexes, aliases, later indexers/skillsets)

# Exercise 5 – Azure AI Search > create new search index
# Set variables
RG=ai200-exam-rg
LOCATION=swedencentral
CONTAINER_NAME=ai200-container
SEARCH_NAME=ai200searchai

# Set AzureSearch__ApiKey in .env: Retrieve Primary key for the service
az search admin-key show \
    --resource-group $RG \
    --service-name $SEARCH_NAME \
    --query primaryKey \
    -o tsv

# List indexes - query the Search REST API.
ADMIN_KEY=$(az search admin-key show \
  --resource-group $RG \
  --service-name $SEARCH_NAME \
  --query primaryKey -o tsv)
curl -X GET \
  "https://$SEARCH_NAME.search.windows.net/indexes?api-version=2025-09-01" \
  -H "api-key: $ADMIN_KEY"
# the same with az-command, but only names
az rest --method get --uri "https://$SEARCH_NAME.search.windows.net/indexes?api-version=2024-07-01" --headers "api-key=$ADMIN_KEY" --query "value[].name" -o tsv

# Delete current index.
ADMIN_KEY=$(az search admin-key show --service-name "$SEARCH_NAME" --resource-group "$RG" --query primaryKey -o tsv)
az rest \
  --method delete \
  --url https://$SEARCH_NAME.search.windows.net/indexes/documents_v2?api-version=2024-07-01 \
  --headers api-key=$ADMIN_KEY

#

#

#
