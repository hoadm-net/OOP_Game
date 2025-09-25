# OOP Game - C# Windows Forms Educational Project

## ?? Project Overview
This project demonstrates Object-Oriented Programming (OOP) concepts using C# Windows Forms with animated game characters. It's designed for teaching inheritance, polymorphism, encapsulation, and abstraction.

## ?? Learning Progression

### Lab01 - Code Duplication Problem
- **Shinobi.cs**: Complete character class with animation system
- **Samurai.cs**: Identical copy of Shinobi (demonstrates code duplication)
- **Problem**: 300+ lines of duplicated code
- **Goal**: Show students why inheritance is needed

### Future Labs
- **Lab02**: Inheritance solution with abstract base class
- **Lab03**: Polymorphism and virtual methods
- **Lab04**: Interfaces for special abilities

## ?? Features

### Character System
- **Smooth 60 FPS animations** with sprite sheets
- **Physics-based jump** with parabolic arc
- **State management** (Idle, Walk, Run, Attack, Jump)
- **One-shot animations** (Attack/Jump complete then return to Idle)

### Controls
- **I**: Idle
- **W**: Walk (hold)
- **R**: Run (hold)
- **A**: Attack (tap)
- **J**: Jump (tap)
- **Space**: Reset position
- **C**: Switch between Shinobi ? Samurai

### Technical Features
- **Double buffering** for smooth rendering
- **Efficient region invalidation** for performance
- **Resource management** with proper disposal
- **Sprite sheet animation** with frame calculation

## ?? Assets
- **Shinobi sprites**: Ninja character animations
- **Samurai sprites**: Warrior character animations
- **Frame-based animation**: 6-12 frames per action

## ?? Technical Stack
- **.NET Framework 4.8.1**
- **C# 7.3**
- **Windows Forms**
- **GDI+ Graphics**

## ?? OOP Concepts Demonstrated

### Lab01 - Encapsulation & Code Duplication
- **Encapsulation**: Private fields, public properties
- **Constructor**: Object initialization
- **Methods**: Character behaviors
- **State Management**: Animation and physics state
- **Problem**: Massive code duplication between classes

### Learning Objectives
1. **See the problem**: Code duplication is painful
2. **Understand encapsulation**: Data hiding and method organization
3. **Prepare for inheritance**: Students will want a better solution
4. **Visual feedback**: Immediate results encourage learning

## ?? Getting Started
1. Open `Lab01.sln` in Visual Studio
2. Build and run the project
3. Use controls to interact with characters
4. Press **C** to switch between characters and see identical behavior
5. Examine code to see the duplication problem

## ?? Educational Value
This project makes OOP concepts **tangible and visual**:
- Students see immediate results of their code
- Animation provides engaging feedback
- Code duplication creates genuine frustration
- Sets up perfect motivation for learning inheritance

Perfect for demonstrating why OOP principles matter in real development!