# DEVELOPER

## Contributing

You can open a PR if there is a feature you need or a bug to fix.

There is a list of features below which could be interesting.

## Features

- [ ] Collision
  - [x] Provide a EdgeCollider2D system
  - [x] Provide a PolygonCollider2D composite system
    - [x] Provide a not-cracked PolygonCollider2D composite system (using an extra mesh between the segments)
    - [ ] Provide a smooth PolygonCollider2D composite system
  - [ ] Provide a BoxCollider2D composite system
  - [ ] Provide a CapsuleCollider2D composite system
  - [ ] Provide a 3D collider
    - Interest is limited for this
    - But it can be done using the bake mesh method of the LineRenderer or using a geometry mesh procedural generation

- [ ] Rendering
  - [x] Provide a LineRenderer rendering system
  - [ ] Provide another line renderer rendering system
  - [ ] Provide a way to paint onto a texture

- [ ] Line styles
  - [ ] Provide a dashed-line rendering system
  - [ ] Provide a blurred line
  - [ ] Provide a smooth line system (using Bezier for instance)
  - [ ] Provide a line which fade away over time
  - [x] Provide a way to choose a color in the Editor
  - [x] Provide a way to choose a gradient in the Editor

- [ ] Playmode
  - [ ] Allow to Undo/Redo the drawings
  - [ ] Allow to pick a color for the next line
  - [ ] Allow to select a segment / line
    - [ ] Indicate the selected line
      - [ ] Using a custom color
      - [ ] Using an outline
  - [ ] Allow to copy a segment / line
  - [ ] Allow to remove a segment / line
  - [ ] Allow to move a segment / line
  - [ ] Allow to split / cut the line
  - [ ] Provide a way to preview the drawing and a way to validate it or cancel it
    - [x] Preview
    - [x] Validation
    - [ ] Cancellation

- [ ] System
  - [ ] Callback when about to add a line (can cancel the adding)
  - [x] Callback when a line was added
  - [x] Callback when a point was added

- [ ] Playability
  - [ ] Option to remove the line after a certain time
  - [ ] Option to stop adding points if a certain distance between start point and end point is attained
    - [x] Dummy version (the limit can be slightly breached)
    - [ ] Strict version
      - Either do not add the last point if above the distance, draw the line and obtain a distance strictly inferior to the limit
      - Or obtain a last point exactly at the limit
    - [ ] Display the limit using a circle
      - [x] Outline
      - [ ] Plain
  - [ ] Option to stop adding points if a certain length of the line is attained
    - [x] Dummy version (the limit can be slightly breached)
    - [ ] Strict version
      - Either do not add the last point if above the distance, draw the line and obtain a length strictly inferior to the limit
      - Or obtain a last point exactly at the limit, by removing a certain amount of length to it
    - [ ] Display the limit using a number
    - [ ] Display the limit using a gauge, bar etc
  - [ ] Option to start drawing from an existing line (like a tree maybe)
  - [ ] Option to draw only
    - [ ] For some value of an axis
    - [ ] Inside a box
    - [ ] Inside a mesh
  - [ ] Option to move an object at the cursor's position
  - [ ] Option to not draw over an existing line
    - [ ] All lines
    - [ ] Only certain lines
  - [ ] Option to have a minimum distance line
  - [ ] Option to have a minimum length line

- [ ] Geometry
  - [ ] Provide a way to customize the geometry easily (adding elements, modifying it etc)
  - [ ] Provide a way to paint custom meshes onto the line
  - [ ] Provide a way to close the mesh if the start and the end are close

- [ ] Input system
  - [x] Support legacy input system
  - [ ] Support new input system
    - [ ] Mouse
    - [ ] Controller
    - [ ] Touch

- [ ] Camera / controllers
  - [x] Free movement
    - ❗ You need to set the virtual camera's Aim to "Do nothing" to freeze the rotation of the camera
      - Otherwise the LineRenderer will bug
      - It seems that they are other way to achieve the same things 

- [ ] Networking
  - [ ] Provide a networked controller
    - [x] Basic setup
    - [ ] Support for the other functionalities
      - [x] Live preview

- [ ] Render pipelines
  - [x] Built-in
  - [ ] URP (should be working as well, not tested)
  - [ ] HDRP(should be working as well, not tested)

- [ ] Demo
  - [ ] Make a GIF for the README.md section
  - [ ] Provide an interactive demo in a site web (link it in the README.md section)

- [ ] Examples
  - [x] No physics
  - [x] Polygon composite collider
  - [x] Color picking in the Editor
  - [x] Gradient picking in the Editor
  - [x] Color changing after each line using a color pool
  - [x] Dummy length limiter
  - [x] Dummy distance limiter
  - [x] Dummy distance limiter + display the limit using outline circle
  - [x] Live preview
  - [x] Networking
    - Make sure you have added the networking scene to the scenes of the build, in "Build Settings"
    - Click on "Build and run"
    - You can now run the example in the Editor
    - Put one of the instances in "Host + client" or "Server", the other instance in "Client"
  - [x] Networking with live preview (shared to other players)
    - Same thing as "Networking" but the lines should be synced as you draw them, with the other instances
  - [x] Free movement
  - [ ] System
    - [x] Use a prefab for the static assets at playtime so that they can be all edited at once
    - [ ] Load some static assets at playtime

- [ ] Optimizations
  - [ ] Use an object pool

- [ ] Code quality
  - [x] Use a ASM definition file
  - [ ] Add unit tests

- [ ] Automations
  - [ ] Automatic update of the package.json when releasing a version
  - [ ] Automatic generation of the .unitypackage when releasing a version
  - [ ] Find a way to add external assets without adding them to .git and without needing manual actions
    - Using Git's submodules : not familiar with it, could work without to have to fork the dependencies
    - Using Unity's package system : would need to fork the projects which don't have a package.json, less interactive
    - There is some project on the web for this, also, it seems : not familiar with them

