#!/bin/bash

# Set variables
RG=ai200-exam-rg
SUBSCRIPTION_TO_CLEAN=$AZURE_SUBSCRIPTION_ID

# Stop on first error
set -e

# Login to the subscription where resources were incorrectly created
echo ">> Logging in to subscription $SUBSCRIPTION_TO_CLEAN..."
az login --subscription $SUBSCRIPTION_TO_CLEAN

# Set the active subscription
echo ">> Setting active subscription..."
az account set --subscription $SUBSCRIPTION_TO_CLEAN

# Verify the current subscription
echo ">> Current subscription:"
az account show --output table

# Delete the resource group (this will delete all resources within it)
echo ">> Deleting resource group $RG..."
az group delete \
    --name $RG \
    --yes \
    --no-wait

echo ">> Resource group deletion initiated. Resources in $RG are being removed."
az group list
