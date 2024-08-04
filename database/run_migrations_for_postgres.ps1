# Define variables
$scriptPath = Join-Path (Get-Location) "database/db-migrations-postgres"
$containerName = "postgres"
$database = "herochallenge"
$username = "testuser"
$password = "testpassword"
$port = 5432

# Get all SQL files in the directory
$sqlFiles = Get-ChildItem -Path $scriptPath -Filter *.sql

# Loop through each SQL file and execute it
foreach ($file in $sqlFiles) {
    Write-Host "Executing script: $($file.FullName)"

    $command = "psql -h $containerName -p $port -U $username -d $database -f /scripts/$($file.Name)"
    # Run the SQL file inside the container
    docker run --rm --network hero-challenge -v "$($file.DirectoryName):/scripts" -e PGPASSWORD=$password postgres:latest bash -c $command


    if ($LASTEXITCODE -ne 0) {
        Write-Host "Error executing script: $($file.FullName)"
        exit 1
    } else {
        Write-Host "Successfully executed script: $($file.FullName)"
    }
}

Write-Host "All scripts executed successfully."
