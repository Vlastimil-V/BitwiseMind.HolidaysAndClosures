@echo off

REM Set the current directory to the script's directory
set "script_dir=%~dp0"
cd /d "%script_dir%"

REM List the contents of the current directory
dir

REM Run the bash script cleanup.sh using Git Bash
if exist "%ProgramFiles%\Git\bin\bash.exe" (
    "%ProgramFiles%\Git\bin\bash.exe" -c "cd '%script_dir%' && ./cleanup.sh"
) else if exist "%ProgramFiles(x86)%\Git\bin\bash.exe" (
    "%ProgramFiles(x86)%\Git\bin\bash.exe" -c "cd '%script_dir%' && ./cleanup.sh"
) else (
    echo Git Bash not found. Please install Git Bash or update the script with the correct path.
)

REM Pause to keep the window open
pause

