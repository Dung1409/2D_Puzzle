# 2D Puzzle – Drag & Drop Block Placement (Unity)

## 📌 Overview
A 2D block placement puzzle game developed with Unity.
Players drag and drop shapes onto a grid to complete rows or columns, clear them, and earn points.

---

## 🎮 Gameplay

- The board size ranges from 5x5 to 10x10.
- Players are given up to 3 shapes at a time.
- Shapes can only be placed on empty tiles.
- When a row or column is completely filled, all blocks in that line are cleared and points are awarded.
- A new shape is generated after a successful placement.
- The game ends when no remaining shapes can fit anywhere on the board.
- Supports saving/loading current state and Best Score.

---

## 🏗 Architecture & Design Patterns

### Singleton
Used to manage core game state and shared systems, ensuring only one instance exists throughout the game lifecycle.

### Observer (Event-driven)
Used to register and dispatch events such as:
- Generate new shapes
- Check game over condition
- Save game data  
This reduces tight coupling between systems.

### Factory
Used to create block objects based on type, centralizing object creation logic and improving extensibility.

### Object Pooling
Used to reuse block GameObjects instead of instantiating/destroying repeatedly, improving performance and reducing garbage collection.

### ScriptableObject
Used to store shape data (boolean matrix structure) separately from logic, allowing easy content creation from the Unity Editor.

---

## 💾 Data Management

- BestScore and Score stored using PlayerPrefs.
- Board state serialized into JSON for save/load functionality.

---

## 🚀 Requirements

- Unity 2022.3.18f1 or later
