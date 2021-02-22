Param([Alias("c")][Switch]$clear = $false, [Alias("h")][Switch]$help = $false, [Alias("ngr")][Switch]$notGenerateReport = $false)

<#
.SYNOPSIS
    Shows the basic help screen (-h or -help)
.DESCRIPTION
    Shows the basic help screen (-h or -help) with examples.
.EXAMPLE
    PS C:\AnimeBrowser_API> .\run_tests_and_generate_report.ps1 -h
    Shows the help screen
.EXAMPLE
    PS C:\AnimeBrowser_API> .\run_tests_and_generate_report.ps1 -help
    Shows the help screen
.EXAMPLE
    PS C:\AnimeBrowser_API> .\run_tests_and_generate_report.ps1 -c -h
    Shows the help screen for -c or -clear 
.EXAMPLE
    PS C:\AnimeBrowser_API> .\run_tests_and_generate_report.ps1 -c
    Clears the screen then runs the script.
.EXAMPLE
    PS C:\AnimeBrowser_API> .\run_tests_and_generate_report.ps1
    Runs the script without clearing the screen.
.NOTES
    Author: Németi Szilárd
    Github: https://github.com/szilajka
#>
function ShowBasicHelpText() {
    Write-Host "This script is used to run the tests for Anime Browser API project, generate the test coverage report, then copy the html files to test\coveragereport folder."
    Write-Host "";
    Write-Host "Parameters:"
    Write-Host "-c or -clear    Clear the screen.";
    Write-Host "-h or -help     Show this help screen.";
    Write-Host "";
    Write-Host "Basic usages: ";
    Write-Host "    .\run_tests_and_generate_report.ps1";
    Write-Host "";
    Write-Host "To clear screen before displaying any output from this script: ";
    Write-Host "    .\run_tests_and_generate_report.ps1 -c";
    Write-Host "    .\run_tests_and_generate_report.ps1 -clear";
    Write-Host "";
    Write-Host "To show help screen: ";
    Write-Host "    .\run_tests_and_generate_report.ps1 -h";
    Write-Host "    .\run_tests_and_generate_report.ps1 -help";
}

<#
.SYNOPSIS
    Shows the help screen for -c or -clear
.DESCRIPTION
    Shows the help screen for clearing the screen with examples.
.EXAMPLE
    PS C:\AnimeBrowser_API> .\run_tests_and_generate_report.ps1 -c -help
    Shows the help screen for -c
.EXAMPLE
    PS C:\AnimeBrowser_API> .\run_tests_and_generate_report.ps1 -c -h
    Shows the help screen for -c
.EXAMPLE
    PS C:\AnimeBrowser_API> .\run_tests_and_generate_report.ps1 -clear -help
    Shows the help screen for -clear
.EXAMPLE
    PS C:\AnimeBrowser_API> .\run_tests_and_generate_report.ps1 -clear -h
    Shows the help screen for -clear
.NOTES
    Author: Németi Szilárd
    Github: https://github.com/szilajka
#>
function ShowClearScreenHelpText() {
    Write-Host "";
    Write-Host "Use -c or -clear to clear the screen. This should be used when you want to clear the screen before any output is displayed from this script."
    Write-Host "    i.e: '.\run_tests_and_generate_report.ps1 -c' or '.\run_tests_and_generate_report.ps1 --clear'"   
}

<#
.SYNOPSIS
    Runs the tests then generate reports.
.DESCRIPTION
    Runs the tests in the AnimeBrowser.UnitTests project, create coverage report (now in xml format), then copy the generated html reports to test\AnimeBrowser.UnitTests\coveragereport folder.
.EXAMPLE
    PS C:\AnimeBrowser_API> .\run_tests_and_generate_report.ps1
    Runs the tests, creates the test results in html format and generates test coverage report.
.EXAMPLE
    PS C:\AnimeBrowser_API> .\run_tests_and_generate_report.ps1 -c -ngr
    Clears screen, then runs the tests then creates test results in html format (not generating test reports).
.NOTES
    Author: Németi Szilárd
    Github: https://github.com/szilajka
#>
function RunTestsThenGenerateReport() {
    $testProjectFolder = (Resolve-Path ".\test\AnimeBrowser.UnitTests").Path
    $testResultsPath = Join-Path -Path $testProjectFolder -ChildPath "TestResults"

    Write-Host "Running tests" -ForegroundColor Blue

    dotnet test --collect:"XPlat Code Coverage" -r $testResultsPath --logger "html;logfilename=test_results.html"

    Write-Host "Tests finished"
	if($notGenerateReport -eq $false){
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
	}
}


if ($clear -eq $true -and $help -eq $true) {
    ShowClearScreenHelpText;
}
elseif ($clear -eq $true) {
    Clear-Host;
}

if ($help -eq $true -and $clear -eq $false) {
    ShowBasicHelpText;
}
elseif ($help -eq $false) {
    RunTestsThenGenerateReport;
}
