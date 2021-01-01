## Growable crops in Unity

This repository is aimed for those who want to some inspiration to build their games using some sort of growing mechanic using the
Unity 3D Tilemaps.

If you're familiar to Stardew valley's crops , this projects aims to be look like that.


## Screenshots

![Screenshot 1](https://github.com/sikozonpc/Tile-Based-Crop-Growing-Mechanics/blob/master/Screenshots/img1.png)


## Implementation design

TLDR; 

Coroutine based growing system that invokes an event to all subscribers (crops) to change its sprite.


At the time I did this there were no tutorials or documentation on how to implement crop growing using Unity's tilemap so I've 
decided to publish my implementation.

It might not be the best way, but it worked nice for my purpose. Basically a `CropTile` is a type of `GameTile` that can be 
**cloned** to the tilemap and has some similarities to an [AnimatedTile](https://github.com/Unity-Technologies/2d-extras/blob/master/Runtime/Tiles/AnimatedTile/AnimatedTile.cs), 
the reason an `AnimatedTile` would not work for this case is because the crop sprite needs to change based on a **condition** and not by
frame time.

When a crop is cloned onto the tilemap, it calls the `StartGrowing()` that will dispatch a **Coroutine** and will loop for X stages **invocking** an event
to all subscribers (`CropTile`'s), on the `OnGrowEvent()` the crop will change its sprite and some other data as the tile description.

The good thing about this system using coroutines is that there will not be a need to do a constant pooling for the growth time nor block the 
main thread.


## How to compile and play

Altought this project is not a ready made package solution but more of a sample project to be dissected it can be pretty easy to consume it.

1. Clone the repository and open it up with the latest Unity version;
2. Open the default scene under `Scenes/SampleScene.unity`;
3. Hit play and it should work.


## How to add new tiles

The implemented solution is not the best for production code, but it can be easily replaced.

The tiles are loaded in code and not in metadata under `Scripts/Utils/TileLibrary.cs`, to add one is simple as just adding an entry to a dictionary as such:

```c#
Tiles.Add("tomato_001", new CropTile()
{
    Description = "Tomato seed - Stage 1",
    TileBase = Resources.Load<TileBase>("Sprites/tomato_001"), // location to the art sprites
    TileData = Resources.Load<Tile>("Sprites/tomato_001"),
    GrowthStageTiles = new GrowthStage[]
    {
            new GrowthStage() {
            Description = "Tomato root - Stage 2",
            Tile = Resources.Load<Tile>("Sprites/tomato_002"),
        },
            new GrowthStage() {
            Description = "Tomato root - Stage 3",
            Tile = Resources.Load<Tile>("Sprites/tomato_003"),
        },
        new GrowthStage() {
            Description = "Tomato big root - Stage 4",
            Tile = Resources.Load<Tile>("Sprites/tomato_004"),
        },
        new GrowthStage() {
            Description = "Tomato big root - Stage 5",
            Tile = Resources.Load<Tile>("Sprites/tomato_005"),
        },
        new GrowthStage() {
            Description = "Grown tomato",
            Tile = Resources.Load<Tile>("Sprites/tomato_006"),
        },
    },
    GrowthTime = 2,                                            // time for the crop to envolve to the next stage tile
});
```

After the tile is added in the library we just have to create an event to spawn it, which currently happens on a button click event with the crop icon that is called on the `CameraController.cs@SelectTileFromHUD(string tileID)` where the tile ID (key of the entry to the dictionary, in this case we added "tomato_001").
 

## Art

The art used for this project is not mine but from a free art asset package on the unity store, so I have no controll over it, it's just used as a sample, and so it can easily replaced with anything.

## Contribution

Everyone is welcome to contribute and use this code for whatever use.