@echo off
echo ========================================
echo    OOP_Game GitHub Setup Script
echo ========================================
echo.

REM Check if Git is installed
git --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERROR: Git is not installed or not in PATH!
    echo.
    echo Please install Git first:
    echo 1. Download Git from: https://git-scm.com/download/windows
    echo 2. Install with default settings
    echo 3. Restart command prompt
    echo 4. Run this script again
    echo.
    pause
    exit /b 1
)

echo Git found! Setting up repository...
echo.

REM Check if already a git repository
if exist ".git" (
    echo Repository already initialized.
) else (
    echo Initializing git repository...
    git init
)

REM Add remote repository (remove if exists)
git remote remove origin >nul 2>&1
echo Adding remote repository...
git remote add origin https://github.com/hoadm-net/OOP_Game.git

REM Add all files
echo Adding files to staging...
git add .

REM Check if there are changes to commit
git diff --staged --quiet
if %errorlevel% neq 0 (
    echo Committing changes...
    git commit -m "Lab01: Initial commit - Code duplication demo with Shinobi and Samurai classes"
    
    echo Pushing to GitHub...
    git push -u origin main
    
    echo.
    echo ========================================
    echo SUCCESS! Repository setup complete!
    echo Project pushed to: https://github.com/hoadm-net/OOP_Game.git
    echo ========================================
) else (
    echo No changes to commit.
)

echo.
pause