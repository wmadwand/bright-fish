This free package contains 10+ ribbon effects created via Shuriken Particle System without non-builtin script (except for the inverted sphere collider in Liberate 3 which was shared for free by someone else). They are good for magical effects like charging and buffing.

These ribbons will look broken when the transformation (position/rotation) of the prefab is animated because the transformation is not taken into account in the calculation of particle spacing interpolation of the sub-emitter. The simplest solution is to modify the "Start Speed" of the master particle emitter, but this workaround cannot handle rotation as the angle of the local simulation space is again controlled via prefab transformation.

If you know any solution to this issue (except for telling us to buy other particle engine), please share that for free just like we did with this asset.

If you want to adjust the speed/duration of the trails, change the "start lifetime" of the master emitter (big head glow) and use the same value for the "duration" of the slave/sub emitter (trail formed by smaller glow particles). The start lifetime value is preferably not randomized otherwise the slave emitter won't be able to sync with the master emitter accurately.

To change the distance of the starting point of trails to the center, change the range values of velocity over lifetime in the master emitter (you don't need to adjust the curves, only the range value at the top left corner of the curve graph). If you've increased the range, you may also need to boost the emission so the trail particles join each other cohesively, change the emission rate range of the slave emitter or vice versa (for the sake of performance).

Charge 3/3.1 and Liberate 2.2 use particle collision with plane. If you want to change the ground level of collision, adjust the y-position of the ground-ref prefab.

Special thanks to "carmine" for providing a solution to inverted sphere collider (http://answers.unity3d.com/questions/213427/sphere-collider-invert.html), but not until the code was amended with "Use UnityEngine;"

Moonflower Carnivore