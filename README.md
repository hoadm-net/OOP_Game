# OOP Game - C# Windows Forms Educational Project

## Project Overview
This project demonstrates Object-Oriented Programming (OOP) concepts using C# Windows Forms with animated game characters. It's designed for teaching inheritance, polymorphism, encapsulation, and abstraction.

## Learning Progression

### Lab01 - Code Duplication Problem
- **Shinobi.cs**: Complete character class with animation system
- **Samurai.cs**: Identical copy of Shinobi (demonstrates code duplication)
- **Problem**: 300+ lines of duplicated code
- **Goal**: Show students why inheritance is needed

### Lab02 - Simple Inheritance Solution
- **Shinobi.cs**: Enhanced base class with smart sprite loading (150+ lines)
- **Samurai.cs**: Simple inheritance with minimal code (10 lines!)
- **Solution**: Samurai extends Shinobi, automatic sprite loading
- **Code Reduction**: ~90% reduction in Samurai class
- **Goal**: Demonstrate inheritance benefits and limitations

### Future Labs
- **Lab03**: Abstract classes and virtual methods
- **Lab04**: Polymorphism and interfaces for special abilities

## Features

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
- **C**: Switch between Shinobi and Samurai

### Technical Features
- **Double buffering** for smooth rendering
- **Efficient region invalidation** for performance
- **Resource management** with proper disposal
- **Sprite sheet animation** with frame calculation
- **Smart sprite loading** based on class name (Lab02)

## Assets
- **Shinobi sprites**: Ninja character animations
- **Samurai sprites**: Warrior character animations
- **Frame-based animation**: 6-12 frames per action

## Technical Stack
- **.NET Framework 4.8.1**
- **C# 7.3**
- **Windows Forms**
- **GDI+ Graphics**

## OOP Concepts Demonstrated

### Lab01 - Encapsulation & Code Duplication
- **Encapsulation**: Private fields, public properties
- **Constructor**: Object initialization
- **Methods**: Character behaviors
- **State Management**: Animation and physics state
- **Problem**: Massive code duplication between classes

### Lab02 - Simple Inheritance
- **Inheritance**: Samurai extends Shinobi
- **Code Reuse**: All functionality inherited from base class
- **Smart Loading**: Automatic sprite path resolution
- **Polymorphism**: Basic polymorphic behavior
- **Limitations**: Cannot override specific behaviors easily

### Learning Objectives
1. **See the problem**: Code duplication is painful (Lab01)
2. **Experience relief**: Inheritance dramatically reduces code (Lab02)
3. **Understand limitations**: Simple inheritance has constraints
4. **Prepare for abstraction**: Students will want more flexibility
5. **Visual feedback**: Immediate results encourage learning

## Getting Started
1. Open `OOP_Game.sln` in Visual Studio
2. Set either Lab01 or Lab02 as startup project
3. Build and run the project
4. Use controls to interact with characters
5. Press **C** to switch between characters and see behavior differences
6. Compare code between Lab01 and Lab02 to see inheritance benefits

## Code Comparison

### Lab01 (Code Duplication)
- **Shinobi.cs**: 150+ lines
- **Samurai.cs**: 150+ lines (identical copy)
- **Total**: 300+ lines

### Lab02 (Simple Inheritance)
- **Shinobi.cs**: 170+ lines (enhanced with smart loading)
- **Samurai.cs**: 10 lines (simple inheritance)
- **Total**: 180+ lines (40% reduction!)

## Educational Value
This project makes OOP concepts **tangible and visual**:
- Students see immediate results of their code
- Animation provides engaging feedback
- Code duplication creates genuine frustration
- Inheritance provides dramatic relief
- Progressive complexity builds understanding
- Sets up perfect motivation for advanced OOP concepts

Perfect for demonstrating why OOP principles matter in real development!