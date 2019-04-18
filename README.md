# vipics
**V**isualizing and **I**nteracting with **P**aths **I**n **C**onfiguration **S**paces

## Purpose

The goals of this project are:
1. To create an environment in which paths in configuration spaces of R^3 can be visualized and interacted with. 
2. To visualize the Vietoris-Rips and Cech simplicial complex constructions from elements of Conf_n(R^3).
3. To visualize how such paths produce persistent homology modules.

### changelog

Nov 28 2018 Initial commit<br>
Jan 21 2019 Created new Unity project<br>
Jan 23 2019 Basic scene with add / remove interactivty<br>
Jan 28 2019 Add / remove / move interactivity finished, though not optimized. First group meeting.<br>
Feb 25 2019 Halos introduced, method to increase their radius introduced.<br>
Mar 29 2019 Cylinders introduced. They do not move when balls are moved.<br>
Apr 10 2019 Cylinders move when balls are moved.<br>
Apr 17 2019 Cylinders disappear when balls are removed.

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

The scene ConfigurationSpace uses the default AvatarGrab scene found in Assets/Oculus/SampleFramework/Usage/ .

---

## Spring 2019 MCL project weekly assignments

*Week of January 28:*
1. Read Chapters III.1 and III.2 in Edelsbrunner and Harer's "Computational Topology." [Link here.](https://www.researchgate.net/publication/220692408_Computational_Topology_An_Introduction)
2. Go to the MCL and:<br>
    1. Log into the computer with the Oculus attached (by the door)<br>
    2. Open Unity, creating an account if necessary<br>
    3. Download this repository using `git clone` through the "Git Bash" program on the Desktop<br>
    4. Open the project in Unity, start the scene, and add (B and Y) / remove (A and X) / move (grip / grab / side trigger) points with the Oculus hand controls.<br>
	
*Week of February 4:*
1. (A/S/V) At every B/Y click, generate another ball at the same spot. This ball should have the following properties:<br>
    1. When the original ball associated with it is moved by the user, the new ball moves as well (has the same center).<br>
    2. It is slightly transparent.<br>
	3. It will grow and shrink when right-hand joystick moves up/down. <br>
2. (J) Create visual interface that allows user to show or hide these slightly transparent balls.<br>

*Week of February 11:*
1. Continue with previous week's goals. <br> 

*Week of February 18:*
1. Make the halo transparent with the alpha of the color of the standard shader. <br>
2. Make the halo be creatd as a child of the ball.<br>
3. Learn about the joystick controls and how they canbe paired with the scale of the halo.<br>

*Week of February 25:*
1. Work on a method that updates the radius of all the halos in the scene.<br>
2. Work together to create a 5-10 minutes presentation in Google Slides for the other groups.<br>

*Week of March 11:*
1. (A/S/V) Work on the cylinders / lines / connectors between balls:<br>
	1. First figure out how to show connectors between all pairs of balls in scene.
	2. The ends of the connectors should be the centers of the balls (clones).
	3. When a new ball is added, connectors should appear to all other balls in the scene.
	4. When a ball is deleted, the connectors between it and all other balls should disappear.
	5. Once this is done, work on how to make the connector appear / disappear based on how close the balls at its ends are.
2. (J) Create visual interface that allows user to show or hide halos.<br>

*Week of April 8:*
1. (A/S/V) Work on how to remove cylinders when balls are removed by the user.<br>
2. (A/S/V) Start thinking about the poster at the end of the semester.<br>
3. (J) Do project clean up:<br>
	1. Remove unnecessary files from git repo.
	2. Make clear installation instructions.
	
*Week of April 15:*
1. (A/S/V) Work on how to show / hide cylinders as radius changes.<br>
2. (A/S/V) Install LaTeX and typeset .tex file for poster.<br>
3. (J) Create method for toggling visibility of halos and edges.
