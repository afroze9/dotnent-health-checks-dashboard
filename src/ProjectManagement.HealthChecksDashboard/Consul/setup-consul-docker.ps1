param($tokenResult)

$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("X-Consul-Token", "$token")

$rules = Get-Content "rules.hcl" -Raw
$policy_name = "kv-health-checks-dashboard"
$service_name = "health-checks-dashboard"

$body = @{
    Name = "$policy_name"
    Description = "Policy for $service_name key prefix"
    Rules = $rules
} | ConvertTo-Json

$response = Invoke-RestMethod `
    -Uri "http://localhost:8500/v1/acl/policy" `
    -Method PUT `
    -Headers $headers `
    -Body $body

$policy_id = $response.ID

echo "Policy Created"
echo "Name: $policy_name"
echo "Id: $policy_id"

$body = @{
    Description = "Token for $service_name service"
    Policies = @(
    @{
        Name = "$policy_name"
    }
    )
    ServiceIdentities = @(
    @{
        ServiceName = "$service_name"
    }
    )
} | ConvertTo-Json

$response = Invoke-RestMethod `
    -Uri "http://localhost:8500/v1/acl/token" `
    -Method PUT `
    -Headers $headers `
    -Body $body

$secret_id = $response.SecretID

echo "Service Name: $service_name"
echo "Token: $secret_id"

# Add token to app-config file
$body = Get-Content -Raw -Path "app-config.json" | ConvertFrom-Json
$body.Consul.Token = $secret_id
$body.SerilogSettings.ElasticSearchSettings.Uri = "https://es01:9200"
$body.HealthCheck.Clients | where { $_.Name -eq "Company API" } | foreach { $_.Url = "http://company-api/actuator/health" }
$body.HealthCheck.Clients | where { $_.Name -eq "Project API" } | foreach { $_.Url = "http://project-api/actuator/health" }
$body.HealthCheck.Clients | where { $_.Name -eq "API Gateway" } | foreach { $_.Url = "http://api-gateway/actuator/health" }
$body | ConvertTo-Json -Depth 10 | Set-Content -Path "app-config.json"

$body = Get-Content -Raw -Path "app-config.json"

$response = Invoke-RestMethod `
    -Uri "http://localhost:8500/v1/kv/$service_name/app-config" `
    -Method PUT `
    -Headers $headers `
    -ContentType "application/json" `
    -Body $body

# Add token to appsettings.json file
$appsettings_content = Get-Content -Raw "..\appsettings.json" | ConvertFrom-Json
$appsettings_content.ConsulKV.Token = $secret_id
$appsettings_content | ConvertTo-Json -Depth 10 | Set-Content "..\appsettings.json"

Write-Host "Key updated successfully."

Set-Variable -Name $tokenResult -Value $secret_id -Scope 1
