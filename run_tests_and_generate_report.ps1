$testProjectFolder = (Resolve-Path ".\test\AnimeBrowser.UnitTests").Path
$testResultsPath = Join-Path -Path $testProjectFolder -ChildPath "TestResults"

Write-Host "Running tests" -ForegroundColor Blue

dotnet test --collect:"XPlat Code Coverage" -r $testResultsPath

Write-Host "Tests finished"
Write-Host "Checking if TestResults folder exists" -ForegroundColor Blue

if ((Test-Path -Path $testResultsPath) -eq $true) {
    Write-Host "TestResults folder exists" -ForegroundColor Blue

    $testResultsFolder = (Resolve-Path $testResultsPath).Path
    $lastReportFolder = Get-ChildItem -Path $testResultsFolder -Directory | Sort-Object LastWriteTime -Descending | Where-Object { $_.Name -match "[0-9a-zA-Z]{8}-[0-9a-zA-Z]{4}-[0-9a-zA-Z]{4}-[0-9a-zA-Z]{4}-[0-9a-zA-Z]{12}" }  | Select-Object -First 1
    Write-Host "Checking if report folders were created" -ForegroundColor Blue
    if (-not ($null -eq $lastReportFolder)) {
        Write-Host "Report folder was created: [$lastReportFolder]" -ForegroundColor Blue
        reportgenerator "-reports:$lastReportFolder\coverage.cobertura.xml" "-targetdir:$testProjectFolder\coveragereport" -reporttypes:Html
        Write-Host "ReportGenerator finished successfully!" -ForegroundColor Green
    }
    else {
        Write-Host "Report folders were NOT created!" -ForegroundColor Red
    }
}
else {
    Write-Host "TestResults folder does NOT exist!" -ForegroundColor Red
}
