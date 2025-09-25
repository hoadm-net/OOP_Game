@echo off
echo ========================================
echo    Installing Git for Windows
echo ========================================
echo.

REM Check if Git is already installed
git --version >nul 2>&1
if %errorlevel% equ 0 (
    echo Git is already installed!
    git --version
    echo.
    echo Running repository setup...
    call setup-git.bat
    exit /b 0
)

echo Git not found. Installing Git using winget...
echo.

REM Try to install Git using winget (Windows 11/10)
winget install --id Git.Git -e --source winget

if %errorlevel% equ 0 (
    echo.
    echo Git installed successfully!
    echo Please restart this command prompt and run setup-git.bat
    echo.
    echo OR: Press any key to continue with current session...
    pause
    
    REM Add Git to PATH for current session
    set "PATH=%PATH%;C:\Program Files\Git\bin"
    
    echo Running repository setup...
    call setup-git.bat
) else (
    echo.
    echo Failed to install Git automatically.
    echo Please install Git manually:
    echo.
    echo 1. Go to: https://git-scm.com/download/windows
    echo 2. Download and install Git
    echo 3. Restart command prompt
    echo 4. Run setup-git.bat
    echo.
    pause
)

echo.
pause