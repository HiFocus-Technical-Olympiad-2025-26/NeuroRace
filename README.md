# NeuroRace Game – Technical olympiad 2025/26 Project

This Unity 2020.3 project is being developed for the **Technical Olympiad 2025/26**, organized by the **Plzeň Region**.  
The goal is to create a game that can be **controlled using NextMind brain-sensing technology**.

---

## 📥 Cloning the project

To clone the main repository **including submodules** (3D assets), run:
```
git clone --recurse-submodules https://github.com/HiFocus-Technical-Olympiad-2025-26/NeuroRace.git
```

If you have already cloned the project without submodules:
```
git submodule update --init --recursive
```


## 🔗 External Submodules

This project uses Git submodules for external assets.

### Cloning the repository

To clone this repository **with all submodules**, run:

```
git clone --recursive https://github.com/HiFocus-Technical-Olympiad-2025-26/NeuroRace.git
```

If you already cloned it without --recursive, initialize the submodules manually:

```
git submodule update --init --recursive
```

### Updating submodules
To pull the latest version of the submodules:

```
git submodule update --remote --merge
```

### Editing a submodule

If you make changes inside a submodule, commit and push them directly from its folder:

```
cd Assets/External/3d-models
git add .
git commit -m "Update 3D models"
git push
```

Then update the reference in the main repository:

```
cd ../../..
git add Assets/External/3d-models
git commit -m "Update submodule reference"
git push
```