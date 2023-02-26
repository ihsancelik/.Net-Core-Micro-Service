@echo off

set /p installationPath="Installation folder path (Without space) :"

SETX MiracleSoftware %installationPath%

echo MiracleSoftware environment variable is set to %installationPath%