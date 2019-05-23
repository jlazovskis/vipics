# vipics
**V**isualizing and **I**nteracting with **P**aths **I**n **C**onfiguration **S**paces

## Purpose

The goals of this project are:
1. To create an environment in which paths in configuration spaces of R^3 can be visualized and interacted with. 
2. To visualize the Vietoris-Rips and Cech simplicial complex constructions from elements of Conf_n(R^3).
3. To visualize how such paths produce persistent homology modules.

### Installation instructions

Many issues can be resolved by saving the project and restarting Unity after every step.<br>

0. Prerequisites: Unity 2018.2, Oculus Rift and Touch controllers.<br>
1. Create a new project in Unity with a 3D Template.<br>
2. Set up the project in Unity:<br>
	1. Check the box: Edit -> Project Settings -> Player -> XR Settings -> Virtual Reality Supported<br>
	2. Install "Oculus Integration" package from the Asset Store.
3. Clone this repository into the folder "Assets".<br>
4. Set up the scene in Unity:<br>
	1. Drag the scene Assets/vipics/Scenes/ConfigurationSpace from the "Project" tab into the "Hierarchy" tab.
	2. Left-click on "Sample Scene" and click "Remove Scene".
5. Click "Play".<br>

The scene ConfigurationSpace uses the default AvatarGrab scene found in Assets/Oculus/SampleFramework/Usage/ .<br>

### Other material

* [Poster for MCL expo](https://github.com/jlazovskis/vipics/blob/master/MCL-2019-poster.pdf)
* [YouTube video demonstration of project](https://www.youtube.com/watch?v=0fR5UxImVpw)<br><br>
<a href="http://www.youtube.com/watch?feature=player_embedded&v=0fR5UxImVpw
" target="_blank"><img src="http://img.youtube.com/vi/0fR5UxImVpw/0.jpg" 
alt="VIPICS Tour" width="240" height="180" border="10" /></a>

### changelog

Nov 28 2018 Initial commit<br>
Jan 21 2019 Created new Unity project<br>
Jan 23 2019 Basic scene with add / remove interactivty<br>
Jan 28 2019 Add / remove / move interactivity finished, though not optimized. First group meeting.<br>
Feb 25 2019 Halos introduced, method to increase their radius introduced.<br>
Mar 29 2019 Cylinders introduced. They do not move when balls are moved.<br>
Apr 10 2019 Cylinders move when balls are moved.<br>
Apr 17 2019 Cylinders disappear when balls are removed.<br>
Apr 22 2019 2-dimensional Vietoris-Rips complex complete. Toggle for 1-dimensional complex for better performance.