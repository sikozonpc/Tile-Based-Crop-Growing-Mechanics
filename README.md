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
 
## Contribution

Everyone is welcome to contribute and use this code for whatever use.