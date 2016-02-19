using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;


namespace RedHawk{
	public class BuilderForWeapon : EditorWindow {

		public GameObject newWeaponObject;
		public vp_ItemType _ItemType;
		public string newWeaponName;
		public vp_FPCamera _UFPSCamera;
		public WeaponItemDefaultsList _myDefaults;
		Vector2 _ScrollPosition ;
		//Weapon Selector
		public string[] newWeaponNameList;
		public int selectedWeaponNumber;
		//End weapon selector
		[MenuItem("Tools/RedHawk/Build a UFPS Weapon",false,1)]
		public static void myWindow()
		{
			EditorWindow window = EditorWindow.GetWindow<BuilderForWeapon>(true,"Build a UFPS Weapon");
			window.minSize = new Vector2(500, 300);
		}
		void OnGUI()
		{
			bool canBuild=true;
			_ScrollPosition = GUILayout.BeginScrollView(_ScrollPosition);
			#region show my variables
			//Show Variables in the Menu
			_myDefaults = EditorGUILayout.ObjectField("Weapon Defaults List",_myDefaults,typeof(WeaponItemDefaultsList),true) as WeaponItemDefaultsList;
			if(_myDefaults!=null){
				#region Weapon Selections
				EditorGUILayout.LabelField("Select the weapon to use from the Weapon Defaults List.");
				UpdateWeaponDropDown();
				selectedWeaponNumber=EditorGUILayout.Popup(selectedWeaponNumber,newWeaponNameList);
				GUILayout.Space(12);
				newWeaponObject = EditorGUILayout.ObjectField("Assign the New Weapon",newWeaponObject,typeof(GameObject),true) as GameObject;
				newWeaponName = EditorGUILayout.TextField("Weapon Name",newWeaponName);
				if(string.IsNullOrEmpty(newWeaponName))
					EditorGUILayout.HelpBox("Examples: 1HMK, 2Colt45. This should be Order Number + Weapon Name to specify the name of the new Weapon under the FPSCamera.  Please populated this.",MessageType.Warning);
				_UFPSCamera = EditorGUILayout.ObjectField("UFPS Weapon Camera",_UFPSCamera,typeof(vp_FPCamera),true) as vp_FPCamera;
				if(_UFPSCamera==null)
					EditorGUILayout.HelpBox("Please assign the Weapon Camera for your UFPS Character.  See the AdvancedPlayer prefab for an example.",MessageType.Error);
				#endregion
				GUILayout.Space(12);
				#region Weapon Read Only view information
				EditorGUILayout.HelpBox("The below items are read-only.  To make changes, use the Weapon Item Editor Window.",MessageType.Info);
				EditorGUILayout.LabelField("Weapon Type = "+_myDefaults.weaponItems[selectedWeaponNumber].weaponType);
				EditorGUILayout.LabelField("vp_ItemType Selected = "+_myDefaults.weaponItems[selectedWeaponNumber].weaponItemType);
				#endregion
			}
			#endregion

			GUILayout.EndScrollView();
			GUILayout.Space(5);
			GUILayout.BeginHorizontal();
			GUI.enabled = canBuild; //this is where you might set the button to false if everything isn't populated
								//maybe call ShowBuildButton() and return canBuild;
			if(GUILayout.Button(newWeaponObject == null||_UFPSCamera==null||string.IsNullOrEmpty(newWeaponName) ? "Assign Components" : "Build & Assign Weapon"))
			{
				BuildRangedWeapon();
			}
			GUI.enabled = true;
			GUILayout.EndHorizontal();
		}

		void BuildRangedWeapon(){
			Transform temp = CreateChild(_UFPSCamera.transform,newWeaponName,Vector3.zero,Vector3.zero);
			temp.gameObject.AddComponent<AudioSource>();
			#region vp_FPWeapon Component Information
			vp_FPWeapon t_FPWeap=temp.gameObject.AddComponent<vp_FPWeapon>();
			t_FPWeap.WeaponPrefab = newWeaponObject;
			//IF NOT LISTED then using defaults
			t_FPWeap.PositionOffset = _myDefaults.weaponItems[selectedWeaponNumber].weaponPositionSprings;
			t_FPWeap.RotationOffset = _myDefaults.weaponItems[selectedWeaponNumber].weaponRotationSprings;
			if(_myDefaults.weaponItems[selectedWeaponNumber].weaponWieldSound!=null)t_FPWeap.SoundWield = _myDefaults.weaponItems[selectedWeaponNumber].weaponWieldSound;
			if(_myDefaults.weaponItems[selectedWeaponNumber].weaponUnWieldSound!=null)t_FPWeap.SoundUnWield = _myDefaults.weaponItems[selectedWeaponNumber].weaponUnWieldSound;
			if(_myDefaults.weaponItems[selectedWeaponNumber].weaponWieldAnim!=null)t_FPWeap.AnimationWield = _myDefaults.weaponItems[selectedWeaponNumber].weaponWieldAnim;
			if(_myDefaults.weaponItems[selectedWeaponNumber].weaponUnwieldAnim!=null)t_FPWeap.AnimationUnWield = _myDefaults.weaponItems[selectedWeaponNumber].weaponUnwieldAnim;
			//NOT SETTING 3rd person t_FPWeap.Weapon3rdPersonModel = GameObject
			if(_myDefaults.weaponItems[selectedWeaponNumber].renderingFieldOfView!=0f)t_FPWeap.RenderingFieldOfView = _myDefaults.weaponItems[selectedWeaponNumber].renderingFieldOfView;
			if(_myDefaults.weaponItems[selectedWeaponNumber].renderingZoomDamping!=0f)t_FPWeap.RenderingZoomDamping = _myDefaults.weaponItems[selectedWeaponNumber].renderingZoomDamping;
			//STATES - weaponReloadState, weaponZoomState, weaponOutOfControlState, weaponSlideState, weaponCrouchState, weaponRunState, weaponAttackState
			#region WEAPON STATES Information
			vp_State reload, zoom, crouch, run, attack;
			if(_myDefaults.weaponItems[selectedWeaponNumber].weaponReloadState!=null)	t_FPWeap.States.Add(reload=new vp_State("vp_FPWeapon","Reload",null,_myDefaults.weaponItems[selectedWeaponNumber].weaponReloadState));
			else t_FPWeap.States.Add(reload=new vp_State("vp_FPWeapon","Reload",null,null));
			if(_myDefaults.weaponItems[selectedWeaponNumber].weaponZoomState!=null)	t_FPWeap.States.Add(zoom=new vp_State("vp_FPWeapon","Zoom",null,_myDefaults.weaponItems[selectedWeaponNumber].weaponZoomState));
			else t_FPWeap.States.Add(zoom=new vp_State("vp_FPWeapon","Zoom",null,null));
			if(_myDefaults.weaponItems[selectedWeaponNumber].weaponOutOfControlState!=null)	t_FPWeap.States.Add(new vp_State("vp_FPWeapon","OutOfControl",null,_myDefaults.weaponItems[selectedWeaponNumber].weaponOutOfControlState));
			else t_FPWeap.States.Add(new vp_State("vp_FPWeapon","OutOfControl",null,null));
			if(_myDefaults.weaponItems[selectedWeaponNumber].weaponSlideState!=null)	t_FPWeap.States.Add(new vp_State("vp_FPWeapon","Slide",null,_myDefaults.weaponItems[selectedWeaponNumber].weaponSlideState));
			else t_FPWeap.States.Add(new vp_State("vp_FPWeapon","Slide",null,null));
			if(_myDefaults.weaponItems[selectedWeaponNumber].weaponCrouchState!=null) t_FPWeap.States.Add(crouch = new vp_State("vp_FPWeapon","Crouch",null,_myDefaults.weaponItems[selectedWeaponNumber].weaponCrouchState));
			else t_FPWeap.States.Add(crouch = new vp_State("vp_FPWeapon","Crouch",null,null));
			if(_myDefaults.weaponItems[selectedWeaponNumber].weaponRunState!=null)	t_FPWeap.States.Add(run = new vp_State("vp_FPWeapon","Run",null,_myDefaults.weaponItems[selectedWeaponNumber].weaponRunState));
			else t_FPWeap.States.Add(run = new vp_State("vp_FPWeapon","Run",null,null));
			if(_myDefaults.weaponItems[selectedWeaponNumber].weaponAttackState!=null)	t_FPWeap.States.Add(attack=new vp_State("vp_FPWeapon","Attack",null,_myDefaults.weaponItems[selectedWeaponNumber].weaponAttackState));
			else t_FPWeap.States.Add(attack=new vp_State("vp_FPWeapon","Attack",null,null));
			//STATE BLOCKERS
			//Things reload Blocks
			TempAddBlocker(t_FPWeap.States,reload,run);
			TempAddBlocker(t_FPWeap.States,reload,attack);
			TempAddBlocker(t_FPWeap.States,reload,crouch);
			//Things zoom Blocks
			TempAddBlocker(t_FPWeap.States,zoom,crouch);
			TempAddBlocker(t_FPWeap.States,zoom,run);
			//Things Crouch blocks
			TempAddBlocker(t_FPWeap.States,crouch,run);
			//Things attack blocks
			TempAddBlocker(t_FPWeap.States,attack,crouch);
			TempAddBlocker(t_FPWeap.States,attack,run);
			#endregion
			#endregion
			#region vp_FPWeaponShooter Component Information
			vp_FPWeaponShooter t_FPShoot=temp.gameObject.AddComponent<vp_FPWeaponShooter>();
			//Projectile - if not listed, then use defaults
			if(_myDefaults.weaponItems[selectedWeaponNumber].prjctlFireRate!=0f)t_FPShoot.ProjectileFiringRate = _myDefaults.weaponItems[selectedWeaponNumber].prjctlFireRate;
			if(_myDefaults.weaponItems[selectedWeaponNumber].prjctlTapFire!=0f)t_FPShoot.ProjectileTapFiringRate = _myDefaults.weaponItems[selectedWeaponNumber].prjctlTapFire;
			if(_myDefaults.weaponItems[selectedWeaponNumber].prjctlPreab!=null)t_FPShoot.ProjectilePrefab = _myDefaults.weaponItems[selectedWeaponNumber].prjctlPreab;
			if(_myDefaults.weaponItems[selectedWeaponNumber].prjctlCount!=0)t_FPShoot.ProjectileCount = _myDefaults.weaponItems[selectedWeaponNumber].prjctlCount;
			if(_myDefaults.weaponItems[selectedWeaponNumber].prjctlSpread!=0f)t_FPShoot.ProjectileSpread = _myDefaults.weaponItems[selectedWeaponNumber].prjctlSpread;
			//MOTION - USE DEFAULTS
			t_FPShoot.MotionPositionRecoil = _myDefaults.weaponItems[selectedWeaponNumber].shooterPositionRecoil;
			t_FPShoot.MotionRotationRecoil = _myDefaults.weaponItems[selectedWeaponNumber].shooterRotationRecoil;
			//MUZZLE FLASH - if not listed, using defaults
			if(_myDefaults.weaponItems[selectedWeaponNumber].mzlFlash!=null)t_FPShoot.MuzzleFlashPrefab = _myDefaults.weaponItems[selectedWeaponNumber].mzlFlash;
			t_FPShoot.MuzzleFlashPosition = new Vector3(0f,0f,1.5f);//Close to Defaults
			//SHELL - if not listed, using defaults
			if(_myDefaults.weaponItems[selectedWeaponNumber].shell!=null)t_FPShoot.ShellPrefab = _myDefaults.weaponItems[selectedWeaponNumber].shell;
			if(_myDefaults.weaponItems[selectedWeaponNumber].shellSize!=0f)t_FPShoot.ShellScale = _myDefaults.weaponItems[selectedWeaponNumber].shellSize;
			t_FPShoot.ShellEjectSpin = 0.5f;
			//SOUND - if not listed, using defaults
			if(_myDefaults.weaponItems[selectedWeaponNumber].sndFire!=null)t_FPShoot.SoundFire = _myDefaults.weaponItems[selectedWeaponNumber].sndFire;
			if(_myDefaults.weaponItems[selectedWeaponNumber].sndDryFire!=null)t_FPShoot.SoundDryFire = _myDefaults.weaponItems[selectedWeaponNumber].sndDryFire;
			//ANIMATION - if not listed, using defaults
			if(_myDefaults.weaponItems[selectedWeaponNumber].animFire!=null)t_FPShoot.AnimationFire = _myDefaults.weaponItems[selectedWeaponNumber].animFire;
			//STATES - Only Zoom
			if(_myDefaults.weaponItems[selectedWeaponNumber].weaponShooterZoomState!=null)	t_FPShoot.States.Add(new vp_State("vp_FPWeaponShooter","Zoom",null,_myDefaults.weaponItems[selectedWeaponNumber].weaponShooterZoomState));
			else t_FPShoot.States.Add(new vp_State("vp_FPWeaponShooter","Zoom",null,null));
			#endregion
			#region vp_FPWeaponReloader Component Information
			vp_FPWeaponReloader t_FPReload=temp.gameObject.AddComponent<vp_FPWeaponReloader>();
			if(_myDefaults.weaponItems[selectedWeaponNumber].reloadSound!=null)t_FPReload.SoundReload = _myDefaults.weaponItems[selectedWeaponNumber].reloadSound;
			if(_myDefaults.weaponItems[selectedWeaponNumber].rldDuration!=0f)t_FPReload.ReloadDuration = _myDefaults.weaponItems[selectedWeaponNumber].rldDuration;
			if(_myDefaults.weaponItems[selectedWeaponNumber].rldAnim!=null)t_FPReload.AnimationReload = _myDefaults.weaponItems[selectedWeaponNumber].rldAnim;
			#endregion
			#region vp_ItemIdentifier Component Information
			vp_ItemIdentifier t_ItemId=temp.gameObject.AddComponent<vp_ItemIdentifier>();
			if(_myDefaults.weaponItems[selectedWeaponNumber].weaponItemType!=null)t_ItemId.Type = _myDefaults.weaponItems[selectedWeaponNumber].weaponItemType;
			#endregion
		}
		//Create the new Weapon as a Child of the FPS Camera
		//Also can use this for adding MuzzleFlash positions, Fire Positions and shell Ejection
		private Transform CreateChild(Transform parent, string name, Vector3 offset, Vector3 rotation){
			Transform child = new GameObject(name).transform;
			child.parent = parent;
			child.localPosition = offset;
			child.rotation = Quaternion.Euler(rotation);
			return child;
		}
		public void SelectItemFromList(string searchFor){
			_myDefaults.weaponItems.Exists(x =>x.weaponDefaultName==searchFor);
		}
		void UpdateWeaponDropDown(){
			newWeaponNameList=new string[_myDefaults.weaponItems.Count];
			for(int i=0; i < _myDefaults.weaponItems.Count;i++){
				newWeaponNameList.SetValue(_myDefaults.weaponItems[i].weaponDefaultName,i);
			}
		}
		//There's a bug in the vp_FPWeapon AddBlocker so this function is being used instead.
		private static void TempAddBlocker(List<vp_State> states, vp_State BlockingState, vp_State StateBeingBlocked)
		{
			if (null == BlockingState.StatesToBlock)
			{
				BlockingState.StatesToBlock = new List<int>();
			}
			BlockingState.AddBlocker(StateBeingBlocked);
			int index = states.IndexOf(StateBeingBlocked);
			if (index >= 0)
			{
				BlockingState.StatesToBlock.Add(index);
			}
			else
			{
				Debug.LogError("The vp_State Blocker Needs to be Added to the States First!");
			}
		}
	}
}