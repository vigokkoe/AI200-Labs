# Set variables
RG=ai200-exam-rg
LOCATION=northeurope
STORAGE_ACCOUNT=ai200storageaccount # Globally unique
CONTAINER_NAME=ai200-container
SEARCH_NAME=ai200searchai # Globally unique

# Stop on first error
set -e

# Login to my VS Prof Subscription In use
az login --subscription e2fbc9d4-dd70-4840-9a07-5f9523648a12
# required for creating role assignments
az account set --subscription e2fbc9d4-dd70-4840-9a07-5f9523648a12

# Verify the current subscription
echo ">> account show:"
az account show --output table

###### 1. Create Azure resources
# Resource group - create
echo ">> creating resource group:"
az group create \
    --name $RG \
    --location $LOCATION

# Storage account - create
echo ">> creating storage account:"
az storage account create \
    --name $STORAGE_ACCOUNT \
    --resource-group $RG \
    --location $LOCATION \
    --sku Standard_LRS \
    --kind StorageV2
    
# Container - create
echo ">> creating storage container:"
az storage container create \
    --account-name $STORAGE_ACCOUNT \
    --name $CONTAINER_NAME \
    --auth-mode login

# Create Azure Cognitive Search service
# endpoint: "https://ai200searchai.search.windows.net"
echo ">> creating Azure Cognitive Search service:"
az search service create \
    --name $SEARCH_NAME \
    --resource-group $RG \
    --location $LOCATION \
    --sku Basic

# Upload a PDF
echo ">> uploading PDF files to storage container $CONTAINER_NAME:"
STORAGE_KEY=$(az storage account keys list \
    --account-name $STORAGE_ACCOUNT \
    --resource-group $RG \
    --query "[0].value" \
    -o tsv | tr -d '\r')

az storage blob upload \
    --account-name $STORAGE_ACCOUNT \
    --container-name $CONTAINER_NAME \
    --name dolomites-intro.pdf \
    --file ../raw/dolomites-intro.pdf \
    --account-key $STORAGE_KEY
az storage blob upload \
    --account-name $STORAGE_ACCOUNT \
    --container-name $CONTAINER_NAME \
    --name dolomites-via-ferratas.pdf \
    --file ../raw/dolomites-via-ferratas.pdf \
    --account-key $STORAGE_KEY

# List resources in the resource group
echo ">> Resources:"
az resource list \
    --resource-group $RG \
    --output table

###### 2. AI Foundry Resource - create
# make sure the Cognitive Services resource provider is registered
az provider register --namespace Microsoft.CognitiveServices

# create a Microsoft Foundry resource that can host Foundry projects
# Azure resource that hosts models, connections, identity, billing
FOUNDRY_NAME=ai200-labs-foundry # Globally unique
echo ">> creating Microsoft Foundry resource $FOUNDRY_NAME:"
az cognitiveservices account create \
    --name $FOUNDRY_NAME \
    --resource-group $RG \
    --kind AIServices \
    --sku S0 \
    --location $LOCATION \
    --allow-project-management \
    --yes

# add a custom domain to the Foundry resource
echo ">> adding custom domain to Foundry resource $FOUNDRY_NAME:"
az cognitiveservices account update \
    --name $FOUNDRY_NAME \
    --resource-group $RG \
    --custom-domain $FOUNDRY_NAME

# wait until the Foundry resource has finished provisioning.
echo "Waiting for Foundry resource..."
while true; do
    STATE=$(az cognitiveservices account show \
        --name $FOUNDRY_NAME \
        --resource-group $RG \
        --query "properties.provisioningState" \
        -o tsv)

    echo "State: $STATE"

    if [ "$STATE" = "Succeeded" ]; then
        break
    fi

    sleep 10
done

# Create AI Project in the Foundry resource
# Workspace where you develop AI applications, prompts, agents, evaluations
echo ">> creating AI Project in Foundry resource $FOUNDRY_NAME:"
AI_PROJECT_NAME=ai200-labs-project
az cognitiveservices account project create \
    --name $FOUNDRY_NAME \
    --resource-group $RG \
    --project-name $AI_PROJECT_NAME \
    --location $LOCATION

# Check whether the account endpoint is ready
# => https://ai200-labs-foundry.cognitiveservices.azure.com/
az cognitiveservices account show \
  --name $FOUNDRY_NAME \
  --resource-group $RG \
  --query "properties.endpoint"

# Deploy a model to the AI Project
echo ">> Deploying embedding model..."
az cognitiveservices account deployment create \
    --name $FOUNDRY_NAME \
    --resource-group $RG \
    --deployment-name "text-embedding-ada-002" \
    --model-name "text-embedding-ada-002" \
    --model-version "2" \
    --model-format OpenAI \
    --sku-name Standard \
    --sku-capacity 1

#