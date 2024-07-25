# Define variables
$scriptPath = Join-Path (Get-Location) "database/db-migrations-cassandra"
$containerName = "cassandra"
$port = 9042

# Get all SQL files in the directory
$sqlFiles = Get-ChildItem -Path $scriptPath -Filter *.cql

# Loop through each SQL file and execute it
foreach ($file in $sqlFiles) {
    Write-Host "Executing script: $($file.FullName)"

    docker run --rm --network hero-challenge -v "$($file.FullName):/scripts/data.cql" -e CQLSH_HOST=$($containerName) -e CQLSH_PORT=$($port) -e CQLVERSION=3.4.6 nuvo/docker-cqlsh

    if ($LASTEXITCODE -ne 0) {
        Write-Host "Error executing script: $($file.FullName)"
        exit 1
    } else {
        Write-Host "Successfully executed script: $($file.FullName)"
    }
}

Write-Host "All scripts executed successfully."
