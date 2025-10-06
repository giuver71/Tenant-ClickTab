param (
    [Parameter(Mandatory=$true)]
    [string]$MigrationName
)

# Cambia questo con il nome del tuo DbContext
$contextName = "MSSQL_DbContext"

# Lista dei tenant
$tenants = @("dev", "tenant1", "tenant2")

foreach ($tenant in $tenants) {
    Write-Host "Tenant: $tenant"

    & dotnet ef database update $MigrationName --context $contextName -- "$tenant"

    if ($LASTEXITCODE -eq 0) {
        Write-Host "OK"
    }
    else {
        Write-Host "FAIL"
    }
}