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

### Lab03 - Virtual Methods & Polymorphism
- **Shinobi.cs**: Base class with virtual methods for customization
- **Samurai.cs**: Override methods for slower/stronger behavior
- **Fighter.cs**: New class with mana system, demonstrates this/base keywords
- **Solution**: Same interface, different behaviors through method overriding
- **Goal**: Show polymorphism power - same method calls, different results

### Lab04 - Abstract Classes & Interfaces
- **Character.cs**: Abstract base class with abstract and virtual methods
- **IShieldable.cs**: Interface for shield functionality
- **Shinobi.cs**: Extends Character (no shield)
- **Samurai.cs**: Extends Character + implements IShieldable
- **Fighter.cs**: Extends Character + implements IShieldable + mana system
- **Goal**: Ultimate OOP flexibility - force implementation + optional features

## Features

### Character System
- **Smooth 60 FPS animations** with sprite sheets
- **Physics-based jump** with parabolic arc
- **State management** (Idle, Walk, Run, Attack, Jump, Shield)
- **One-shot animations** (Attack/Jump complete then return to Idle)

### Controls
- **I**: Idle
- **W**: Walk (hold)
- **R**: Run (hold)
- **A**: Attack (tap)
- **J**: Jump (tap)
- **S**: Shield (toggle) - Lab04 only
- **Space**: Reset position
- **C**: Switch between characters
- **M**: Restore mana (Fighter only) - Lab03/04

### Technical Features
- **Double buffering** for smooth rendering
- **Efficient region invalidation** for performance
- **Resource management** with proper disposal
- **Sprite sheet animation** with frame calculation
- **Smart sprite loading** based on class name

## Assets
- **Shinobi sprites**: Ninja character animations
- **Samurai sprites**: Warrior character animations (includes Shield.png for Lab04)
- **Fighter sprites**: Balanced character animations (includes Shield.png for Lab04)
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

### Lab03 - Virtual Methods & Polymorphism
- **Virtual Methods**: Base class methods that can be overridden
- **Method Overriding**: Child classes customize behavior
- **Polymorphism**: Same method call, different behaviors
- **this/base Keywords**: Explicit object and parent references
- **Code Flexibility**: Easy to add new character types

### Lab04 - Abstract Classes & Interfaces
- **Abstract Classes**: Cannot be instantiated, force implementation
- **Abstract Methods**: Must be implemented by derived classes
- **Interfaces**: Contract for specific capabilities
- **Multiple Inheritance**: Class + Interface combinations
- **Ultimate Flexibility**: Mix and match capabilities

### Learning Objectives
1. **See the problem**: Code duplication is painful (Lab01)
2. **Experience relief**: Inheritance dramatically reduces code (Lab02)
3. **Understand customization**: Virtual methods enable different behaviors (Lab03)
4. **Master abstraction**: Abstract classes + interfaces provide ultimate flexibility (Lab04)
5. **Visual feedback**: Immediate results encourage learning

## Getting Started
1. Open `OOP_Game.sln` in Visual Studio
2. Set Lab01, Lab02, Lab03, or Lab04 as startup project
3. Build and run the project
4. Use controls to interact with characters
5. Press **C** to switch between characters and see behavior differences
6. Compare code between labs to see OOP evolution

## Code Evolution Comparison

### Lab01 (Code Duplication)
- **Shinobi.cs**: 150+ lines
- **Samurai.cs**: 150+ lines (identical copy)
- **Total**: 300+ lines

### Lab02 (Simple Inheritance)
- **Shinobi.cs**: 170+ lines (enhanced with smart loading)
- **Samurai.cs**: 10 lines (simple inheritance)
- **Total**: 180+ lines (40% reduction!)

### Lab03 (Virtual Methods)
- **Shinobi.cs**: 180+ lines (with virtual methods)
- **Samurai.cs**: 30 lines (method overrides)
- **Fighter.cs**: 50 lines (new class with mana)
- **Total**: 260+ lines (3 characters vs 2)

### Lab04 (Abstract Classes + Interfaces)
- **Character.cs**: 200+ lines (abstract base)
- **IShieldable.cs**: 15 lines (interface)
- **Shinobi.cs**: 25 lines (abstract implementation)
- **Samurai.cs**: 80 lines (abstract + interface)
- **Fighter.cs**: 120 lines (abstract + interface + mana)
- **Total**: 440+ lines (but maximum flexibility!)

## Educational Value
This project makes OOP concepts **tangible and visual**:
- Students see immediate results of their code
- Animation provides engaging feedback
- Progressive complexity builds understanding naturally
- Each lab solves problems from the previous one
- Perfect preparation for advanced software architecture

Perfect for demonstrating why OOP principles matter in real development!