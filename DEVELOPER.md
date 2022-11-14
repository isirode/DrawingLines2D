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

- [ ] Networking
  - [ ] Provide a networked controller

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

