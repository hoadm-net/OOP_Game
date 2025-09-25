# Manual Git Setup Instructions

## ?? Prerequisites
1. **Install Git**: Download from https://git-scm.com/download/windows
2. **Install with default settings**
3. **Restart command prompt/PowerShell**

## ?? Manual Setup Steps

### 1. Open Command Prompt or PowerShell in project directory
```
cd C:\Users\hoadm\Desktop\OOP_Game\
```

### 2. Initialize Git repository
```
git init
```

### 3. Add remote repository
```
git remote add origin https://github.com/hoadm-net/OOP_Game.git
```

### 4. Add files to staging
```
git add .
```

### 5. Commit changes
```
git commit -m "Lab01: Initial commit - Code duplication demo with Shinobi and Samurai classes"
```

### 6. Push to GitHub
```
git push -u origin main
```

## ?? Alternative: Using GitHub Desktop
1. Download **GitHub Desktop** from: https://desktop.github.com/
2. **Clone** the repository: https://github.com/hoadm-net/OOP_Game.git
3. **Copy** all project files into the cloned folder
4. **Commit and Push** using the GUI

## ?? Alternative: Using Visual Studio
1. In Visual Studio: **File** ? **Add to Source Control** ? **Git**
2. **Team Explorer** ? **Settings** ? **Repository Settings**
3. Add remote: `https://github.com/hoadm-net/OOP_Game.git`
4. **Changes** ? **Commit All** ? **Sync** ? **Push**

## ?? Files to be committed:
- `Lab01/` - Complete project
- `assets/` - Sprite assets
- `README.md` - Documentation
- `.gitignore` - Git ignore rules
- `setup-git.bat` - Setup script

## ?? Troubleshooting
- **Git not found**: Install Git and restart terminal
- **Permission denied**: Check GitHub repository access
- **Authentication**: Use GitHub username/password or Personal Access Token