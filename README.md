# space_testing
A space coop game in Unity C#

Description

A demo project featuring custom ship movement, RigidBody physics, and multiplayer netcode; focussing on simplifying development and physics interactions.

Unity's Rigidbody3D physics have always been buggy. Getting them working reliably is possible, though difficulty arises when we need to keep a RigidBody player pinned to a RigidBody vehicle parent without things falling apart or 'flying away' when buggy collisions occur. Even more problematic is the implementation of Netwcode with these physics; cient latency often causes players to stutter and rubberband between positions on both their own client and other client machines. To tackle this problem, I have implemented an Inverted Movement system style. Rather than moving the ship around the world with Physics, the ship instead if made a static object, in which the world moves around. This simplified the interaction between both the ship and players greatly, and removes the frustration of implementing relative physics interactions between players and their ship parent object.

FEATURING
- CO-OP multiplayer networking using Unity Netcode, Relay, and Lobby
- Custom ship Matrix multiplication

This project is under development, features specified may be disabled in the project.
