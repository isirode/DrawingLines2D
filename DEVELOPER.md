# DEVELOPER

## Contributing

You can open a PR if there is a feature you need or a bug to fix.

There is a list of features below which could be interesting.

## Features

- [ ] Collision
  - [x] Provide a EdgeCollider2D system
  - [x] Provide a PolygonCollider2D composite system
    - [ ] Provide a not-cracked PolygonCollider2D composite system (using an extra mesh between the segments)
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

- [ ] Geometry
  - [ ] Provide a way to customize the geometry easily
  - [ ] Provide a way to paint custom meshes onto the line
  - [ ] Provide a way to close the mesh if the start and the end are close

- [ ] Networking
  - [ ] Provide a networked controller

- [ ] Code quality
  - [ ] Add unit tests

- [ ] Automations
  - [ ] Automatic update of the package.json when releasing a version

