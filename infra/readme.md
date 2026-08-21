# 1. Create Infra first

# Exercise 4 – Build Your First RAG with Azure AI Search -> Infra
# Set variables
RG=ai200-exam-rg
LOCATION=swedencentral
STORAGE_ACCOUNT=ai200storageaccount
CONTAINER_NAME=ai200-container
SEARCH_NAME=ai200searchai

# Login to my VS Prof Subscription To be updated in 08/26
az login --subscription $AZURE_SUBSCRIPTION_ID

# Login to my VS Prof Subscription Second Not used
az login --subscription $AZURE_SUBSCRIPTION_ID

# Resource group - create
az group create \
    --name $RG \
    --location $LOCATION

# Storage account - create
az storage account create \
    --name $STORAGE_ACCOUNT \
    --resource-group $RG \
    --location $LOCATION \
    --sku Standard_LRS \
    --kind StorageV2
    
# Container - create
az storage container create \
    --account-name $STORAGE_ACCOUNT \
    --name $CONTAINER_NAME \
    --auth-mode login

# Signed in user ID: $SIGNED_IN_USER_ID
az ad signed-in-user show \
    --query id \
    --output tsv

# Assign a role to the user
az role assignment create \
    --assignee <OBJECT_ID> \
    --role "Storage Blob Data Contributor" \
    --scope $(az storage account show \
        --name $STORAGE_ACCOUNT \
        --resource-group $RG \
        --query id \
        --output tsv)

# /subscriptions/$AZURE_SUBSCRIPTION_ID/resourceGroups/ai200-exam-rg/providers/Microsoft.Storage/storageAccounts/ai200storageaccount
az storage account show \
        --name $STORAGE_ACCOUNT \
        --resource-group $RG \
        --query id \
        --output tsv

# Upload a PDF
az storage blob upload \
    --account-name $STORAGE_ACCOUNT \
    --container-name $CONTAINER_NAME \
    --name climbing.pdf \
    --file climbing.pdf \
    --auth-mode login

# Container list
az storage container list \
    --account-name $STORAGE_ACCOUNT \
    --auth-mode login \
    --output table

# List blobs - uploaded pdf-files
az storage blob list \
    --account-name $STORAGE_ACCOUNT \
    --container-name $CONTAINER_NAME \
    --auth-mode login \
    --output table
# only file names
    --query "[].name" \
    --output tsv
        
5. Azure AI Search
# Verify
az search service show \
    --name $SEARCH_NAME \
    --resource-group $RG \
    --output table

# Show Admin key
az search admin-key show \
  --service-name $SEARCH_NAME \
  --resource-group $RG \
  --query primaryKey \
  -o tsv

# List indexes
## Get admin key
ADMIN_KEY=$(az search admin-key show --service-name "$SEARCH_NAME" --resource-group "$RG" --query primaryKey -o tsv)
## List indexes
az rest --method get --uri "https://$SEARCH_NAME.search.windows.net/indexes?api-version=2024-07-01" --headers "api-key=$ADMIN_KEY"

# Delete Search service by name
az search service delete -n $SEARCH_NAME    --resource-group $RG
# List available Search services
 az search service list     --resource-group $RG

# Verify endpoint exists > https://ai200-labs-foundry.cognitiveservices.azure.com/
az cognitiveservices account show \
  --name ai200-labs-foundry \
  --resource-group ai200-exam-rg \
  --query properties.endpoint

# Verify deployment exists
az cognitiveservices account deployment list \
    --name ai200-labs-foundry \
    --resource-group ai200-exam-rg \
    --output table

# Verify API key -> set to .env AzureOpenAI__ApiKey
az cognitiveservices account keys list \
    --name ai200-labs-foundry \
    --resource-group ai200-exam-rg

#
