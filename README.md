# SaveSystem
Simple project to demonstrated saving and loading game data in binary format.
100% commented C# code.

# Advantage
- Data stored in binary, difficult to edit and hack.
- Can store any data type information.
- Usefull to store player score, level, unlock items, etc.

# How To Use?
- Add SaveManager to the scene.
- Use SaveManager.instance.SaveData<T>() to save the data
- Use SaveManager.instance.LoadData<T>() to load the data.
