---------------------------------------------

### Adding the Plugin to your Project ###
When installing the Fmod Integration, you'll have to move the following folders to the ROOT of your Assets-folder:

- Editor Default Resources
- Gizmos 

(See the "FmodFolderStructure_ForAFunctioningPlugin.PNG" included in this package for a screenshot of how it should look like)

If this is not done, then the Fmod Settings-menu and the Fmod Components-view will not work properly. 
If a folder with the same name already exists in your assets folder, just add the contents of the Fmod-folders into them. 
Alternatively, use "Windows Explorer" (or "Finder" on Mac) to move the folders manually. This way, the folders should get merged automatically.

---------------------------------------------

### Location of the Fmod Banks ###
Set this in the FMOD > Edit Settings-menu. 

By default, this integration assumes that you have a folder called "FmodBanks" in the ROOT of your Assets folder, where your banks are located. 
This can of course be changed to suit your own setup.