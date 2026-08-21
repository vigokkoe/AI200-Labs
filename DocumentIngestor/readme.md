# 3. Read PDFs → chunk → generate embeddings → upload to Azure AI Search

# Exercise 6 – Build our own ingestion pipeline
STORAGE_ACCOUNT=ai200storageaccount
# Storage end-point https://ai200storageaccount.blob.core.windows.net

# List containers - only ai200-container
az storage container list \
    --account-name ai200storageaccount \
    --auth-mode login \
    --output table

# Set Storage__ApiKey in .env: Retrieve Primary key for the storage
az storage account keys list \
    --account-name $STORAGE_ACCOUNT \
    --resource-group $RG \
    --query "[0].value" \
    -o tsv

# In AI Foundry deploy model text-embedding-ada-002
az cognitiveservices account deployment create \
    --name $FOUNDRY_NAME \
    --resource-group $RG \
    --deployment-name "text-embedding-ada-002" \
    --model-name "text-embedding-ada-002" \
    --model-version "2" \
    --model-format OpenAI \
    --sku-name Standard \
    --sku-capacity 1

# Verify model deployed
az cognitiveservices account deployment list \
    --name $FOUNDRY_NAME \
    --resource-group $RG \
    --output table

# List available models with their first sku:
az cognitiveservices model list \
    --location swedencentral \
    --query "[].{Model:model.name, SKU:model.skus[0].name}" \
    -o table

#
