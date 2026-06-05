# Initialize git if not already initialized
if (-not (Test-Path .git)) {
    git init
    Write-Host "Initialized empty Git repository."
}

# Configure local repository credentials for safety
git config user.name "bao.nc"
git config user.email "bao.nc2630@gnt.com.vn"

# Add files and commit
git add .
git commit -m "Initial release of Swagger DTO Generator UPM package"

# Check if origin remote already exists, if so update it
$remoteExists = git remote
if ($remoteExists -contains "origin") {
    git remote set-url origin https://github.com/365chibao/unity-swagger-generator.git
} else {
    git remote add origin https://github.com/365chibao/unity-swagger-generator.git
}

# Rename branch to main
git branch -M main

# Push to GitHub
Write-Host "Pushing to GitHub (https://github.com/365chibao/unity-swagger-generator.git)..."
git push -u origin main
