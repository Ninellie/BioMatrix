# 2d_game_prototype
This is my 2D game prototype

You can find game scripts (c# classes) here: [C# Scripts](https://github.com/Ninellie/2d_game_prototype/tree/main/Assets/Scripts)

In this project, I'm trying to put into practice iterative development and plan-do-check-act cycle methodologies, as well as try out some architectural patterns. In addition, I simply want to explore the Unity3d engine.

To summarize, I'm trying to make a working layout of the game that can be conveniently expanded by adding various functionality and game mechanics in portions, so as not to drag out the addition of one feature for a long period.

Now the game is a pixel-style top-down shooter where you need to survive 5 minutes while being besieged by hordes of monsters to win.

The most important code requirements I strive for when writing:
- No repeating code
- Separation of entities into different classes
- Speaking names of classes, methotds and variables
- I try to stick to Microsoft dotnet [C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)

When adding new features, I am guided by the following principles:
- Think about added functionality. Is it necessary in the game? And how will it work? Usually I use Miro or paper to complete my thoughts.
- Add a small piece of code.
- Make sure it didn't break anything
- Add it to the game.


About code

All game entities are based on two classes: **Entity** and **Unit**. Unit Differs from Entity in that it can move and, accordingly, has a motion controller.

To describe these entities, I made two important classes: **Stat** and **Resource**.

- **Stat** describes some characteristic of an entity. For example, maximum speed or damage.

- **Resouce** describes some resource accumulated by the entity. For example health points, experience, level or ammo of weapons.