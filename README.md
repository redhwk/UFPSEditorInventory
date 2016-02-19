# UFPSEditorInventory
Editor Inventory I created to store UFPS weapon information.  This can be expanded easily enough. Let me know if you find this useful or would like me to add anything.

Purpose - The goal was to have an easy to access dictionary/list of weapons to apply to any UFPS character.  This way you could store your settings for any particular list and apply them to all of your various characters as a starting point.  I was hoping to create an easy way to share weapon settings.

You will find a few Editor Windows all located under Tools\RedHawk\:

Weapon Item Editor - This creates a Window that you can dock.  The editor is a basic weapon editor that allows you to store your settings for a specific weapon that you've created for your project.  There are fold outs to help focus your attention.  

Note - I have not created a foldout or value for every possibility.  This can be extended for anyone wanting to put in the effort
Show Item List = Shows where the list is in your project Folder

Open Item List = Opens a window

Create New Item List = CAUTION: this will create a default named list.  You should change the name as soon as you create it otherwise this could overwrite your previously created list

Prev & Next = These buttons cycle through your list

Add Item = Add an item to your list

Delete Item = Delete the current item from your list

Build a UFPS Weapon - This allows you to select your List, select a Weapon via a Dropdown menu (from above), provide a new Weapon Name (example 6Pistol), assign a weapon object, and select your UFPS Character FPSCamera (must have vp_FPCamera) parent.

Build a Melee or Grenade - This duplicates 5Knife or 4GrenadeThrow from the HeroHDWeapons player prefab that comes with UFPS as a starting point.

Create Weapon Item Defaults List Container - Same as Create New Item List in the Weapon Item Editor.
