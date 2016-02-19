using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace RedHawk{
	public class WeaponItemListEditor : EditorWindow {
		public WeaponItemDefaultsList DefaultsList;
		private int viewIndex = 1;
		Vector2 scrollPosition;
		public string[] newWeaponNameList;
		#region Foldouts
		bool weaponBasicsFoldout=false;
		bool weaponWeaponFoldout=false;
		bool weaponWeaponSoundFoldout=false;
		bool weaponProjectileFoldout=false;
		bool weaponMuzzShellFoldout=false;
		bool weaponSoundsAnims=false;
		bool weaponReloadFoldout=false;
		bool weaponShooterFoldout=false;
		bool motionFoldout=false;
		#endregion
		[MenuItem("Tools/RedHawk/Weapon Item Editor %#e")]
		static void Init(){
			EditorWindow.GetWindow(typeof(WeaponItemListEditor));
		}
		void OnEnable(){
			if(EditorPrefs.HasKey("ObjectPath")){
				string objectPath = EditorPrefs.GetString("ObjectPath");
				DefaultsList = AssetDatabase.LoadAssetAtPath(objectPath,typeof(WeaponItemDefaultsList)) as WeaponItemDefaultsList;
			}
		}
		void createNewListForMe(){
			WeaponItemDefaultsList newList;
			newList=CreateWeaponItemDefault.Create();
			Debug.Log("New LIst was created "+newList);
			DefaultsList = newList;

		}
		void OnGUI(){
			GUILayout.BeginHorizontal();
			GUILayout.Label("Weapon Item Editor",EditorStyles.boldLabel);
			if(DefaultsList != null){
				if(GUILayout.Button("Show Item List")){
					EditorUtility.FocusProjectWindow();
					Selection.activeObject = DefaultsList;
				}
			}
			if(GUILayout.Button("Open Item List")){
				OpenItemList();
			}
			if(GUILayout.Button("Create New Item List")){
				createNewListForMe();
				//EditorUtility.FocusProjectWindow();
				//Selection.activeObject = DefaultsList;
			}
			GUILayout.EndHorizontal();
			if(DefaultsList == null){
				GUILayout.BeginHorizontal();
				GUILayout.Space(15);
				if(GUILayout.Button("Open Existing Weapon Item List",GUILayout.ExpandWidth(false))){
					OpenItemList();
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.Space(20);
			if(DefaultsList != null){
				DefaultsList = EditorGUILayout.ObjectField("Current Weapon Item list",DefaultsList,typeof(WeaponItemDefaultsList),false) as WeaponItemDefaultsList;
				GUILayout.BeginHorizontal();
				GUILayout.Space(10);
				if(GUILayout.Button("Prev",GUILayout.ExpandWidth(false))){
					if(viewIndex > 1)
						viewIndex --;
				}
				GUILayout.Space(5);
				if(GUILayout.Button("Next",GUILayout.ExpandWidth(false))){
					if(viewIndex < DefaultsList.weaponItems.Count){
						viewIndex ++;
					}
				}
				GUILayout.Space(60);
				if(GUILayout.Button("Add Item",GUILayout.ExpandWidth(false))){
					AddItem();
				}
				if(GUILayout.Button("Delete Item",GUILayout.ExpandWidth(false))){
					DeleteItem(viewIndex - 1);
				}
				GUILayout.EndHorizontal();
				//Item Stuff
				if(DefaultsList.weaponItems==null){
					GUILayout.Label("This inventory list is Empty.");
					return;
				}
				else if(DefaultsList.weaponItems.Count > 0){
					GUILayout.BeginHorizontal();
					viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Weapon",viewIndex,GUILayout.ExpandWidth(false)),1,DefaultsList.weaponItems.Count);
					EditorGUILayout.LabelField("of "+DefaultsList.weaponItems.Count.ToString()+" weapons","",GUILayout.ExpandWidth(false));
					GUILayout.EndHorizontal();
					#region List specific Item Details
					scrollPosition=GUILayout.BeginScrollView(scrollPosition);
					DefaultsList.weaponItems[viewIndex-1].weaponDefaultName = EditorGUILayout.TextField("Weapon Name",DefaultsList.weaponItems[viewIndex-1].weaponDefaultName as string);
//RANGED ONLY					DefaultsList.weaponItems[viewIndex-1].weaponType = (weaponTypes)EditorGUILayout.EnumPopup("Weapon Type",DefaultsList.weaponItems[viewIndex-1].weaponType);
					#endregion
					GUILayout.Space(12);
					weaponBasicsFoldout = EditorGUILayout.Foldout(weaponBasicsFoldout,"Weapon Icon, Item Type, and FOV");
					if(weaponBasicsFoldout){
						GUILayout.Label("Weapon Icon");
						DefaultsList.weaponItems[viewIndex-1].weaponIcon = EditorGUILayout.ObjectField("",DefaultsList.weaponItems[viewIndex-1].weaponIcon,typeof(Texture2D),false,GUILayout.Width(200f)) as Texture2D;
						DefaultsList.weaponItems[viewIndex-1].weaponItemType = EditorGUILayout.ObjectField("UFPS Weapon Item Type",DefaultsList.weaponItems[viewIndex-1].weaponItemType,typeof(vp_ItemType),false) as vp_ItemType;
						DefaultsList.weaponItems[viewIndex-1].renderingFieldOfView = EditorGUILayout.FloatField("Field of View",DefaultsList.weaponItems[viewIndex-1].renderingFieldOfView);
						DefaultsList.weaponItems[viewIndex-1].renderingZoomDamping = EditorGUILayout.FloatField("Zoom Damping",DefaultsList.weaponItems[viewIndex-1].renderingZoomDamping);
						DefaultsList.weaponItems[viewIndex-1].weaponPositionSprings = EditorGUILayout.Vector3Field("Position Offset Springs",DefaultsList.weaponItems[viewIndex-1].weaponPositionSprings);
					}
					GUILayout.Space(12);
					//Weapon Foldout
					weaponWeaponSoundFoldout = EditorGUILayout.Foldout(weaponWeaponSoundFoldout,"Wield/Unwield Sounds & Anims");
					if(weaponWeaponSoundFoldout){
						DefaultsList.weaponItems[viewIndex-1].weaponWieldSound = EditorGUILayout.ObjectField("Wield Sound",DefaultsList.weaponItems[viewIndex-1].weaponWieldSound,typeof(AudioClip),true) as AudioClip;
						DefaultsList.weaponItems[viewIndex-1].weaponUnWieldSound = EditorGUILayout.ObjectField("Unwield Sound",DefaultsList.weaponItems[viewIndex-1].weaponUnWieldSound,typeof(AudioClip),true) as AudioClip;
						DefaultsList.weaponItems[viewIndex-1].weaponWieldAnim = EditorGUILayout.ObjectField("Wield Animation",DefaultsList.weaponItems[viewIndex-1].weaponWieldAnim,typeof(AnimationClip),true) as AnimationClip;
						DefaultsList.weaponItems[viewIndex-1].weaponUnwieldAnim = EditorGUILayout.ObjectField("Unwield Animation",DefaultsList.weaponItems[viewIndex-1].weaponUnwieldAnim,typeof(AnimationClip),true) as AnimationClip;
					}
					GUILayout.Space(12);
					weaponWeaponFoldout = EditorGUILayout.Foldout(weaponWeaponFoldout,"Weapon State Presets");
					if(weaponWeaponFoldout){
						//weaponReloadState, weaponZoomState, weaponOutOfControlState, weaponSlideState, weaponCrouchState, weaponRunState, weaponAttackState
						DefaultsList.weaponItems[viewIndex-1].weaponReloadState = EditorGUILayout.ObjectField("Reload State",DefaultsList.weaponItems[viewIndex-1].weaponReloadState,typeof(TextAsset),true) as TextAsset;
						DefaultsList.weaponItems[viewIndex-1].weaponZoomState = EditorGUILayout.ObjectField("Zoom State",DefaultsList.weaponItems[viewIndex-1].weaponZoomState,typeof(TextAsset),true) as TextAsset;
						DefaultsList.weaponItems[viewIndex-1].weaponOutOfControlState = EditorGUILayout.ObjectField("Out of Control State",DefaultsList.weaponItems[viewIndex-1].weaponOutOfControlState,typeof(TextAsset),true) as TextAsset;
						DefaultsList.weaponItems[viewIndex-1].weaponSlideState = EditorGUILayout.ObjectField("Slide State",DefaultsList.weaponItems[viewIndex-1].weaponSlideState,typeof(TextAsset),true) as TextAsset;
						DefaultsList.weaponItems[viewIndex-1].weaponCrouchState = EditorGUILayout.ObjectField("Crouch State",DefaultsList.weaponItems[viewIndex-1].weaponCrouchState,typeof(TextAsset),true) as TextAsset;
						DefaultsList.weaponItems[viewIndex-1].weaponRunState = EditorGUILayout.ObjectField("Run State",DefaultsList.weaponItems[viewIndex-1].weaponRunState,typeof(TextAsset),true) as TextAsset;
						DefaultsList.weaponItems[viewIndex-1].weaponAttackState = EditorGUILayout.ObjectField("Attack State",DefaultsList.weaponItems[viewIndex-1].weaponAttackState,typeof(TextAsset),true) as TextAsset;
					}
					GUILayout.Space(12);
					EditorGUILayout.LabelField("WEAPON SHOOTER COMPONENTS");
					GUILayout.Space(12);
					//Projectile Shooter Foldout
					weaponProjectileFoldout = EditorGUILayout.Foldout(weaponProjectileFoldout,"Weapon Projectile Information");
					if(weaponProjectileFoldout){
						DefaultsList.weaponItems[viewIndex-1].prjctlFireRate = EditorGUILayout.FloatField("Projectile Fire Rate",DefaultsList.weaponItems[viewIndex-1].prjctlFireRate);
						DefaultsList.weaponItems[viewIndex-1].prjctlTapFire = EditorGUILayout.FloatField("Projectile Tap Fire Rate",DefaultsList.weaponItems[viewIndex-1].prjctlTapFire);
						DefaultsList.weaponItems[viewIndex-1].prjctlPreab = EditorGUILayout.ObjectField("Weapon Projectile Object",DefaultsList.weaponItems[viewIndex-1].prjctlPreab,typeof(GameObject),false) as GameObject;
						DefaultsList.weaponItems[viewIndex-1].prjctlCount = EditorGUILayout.IntField("Projectile Count",DefaultsList.weaponItems[viewIndex-1].prjctlCount);
						GUILayout.BeginHorizontal();
						GUILayout.Label("Projectile Spread (0 to 360)");
						DefaultsList.weaponItems[viewIndex-1].prjctlSpread = EditorGUILayout.FloatField(Mathf.Clamp(DefaultsList.weaponItems[viewIndex-1].prjctlSpread,0,360),GUILayout.Width(600f));
						GUILayout.EndHorizontal();
					}
					GUILayout.Space(12);
					motionFoldout = EditorGUILayout.Foldout(motionFoldout,"Weapon Shooter Motion Settings");
					if(motionFoldout){
						DefaultsList.weaponItems[viewIndex-1].shooterPositionRecoil = EditorGUILayout.Vector3Field("Position Recoil",DefaultsList.weaponItems[viewIndex-1].shooterPositionRecoil);
						DefaultsList.weaponItems[viewIndex-1].shooterRotationRecoil = EditorGUILayout.Vector3Field("Rotation Recoil",DefaultsList.weaponItems[viewIndex-1].shooterRotationRecoil);
					}
					GUILayout.Space(12);
					//Weapon Shooter States
					weaponShooterFoldout = EditorGUILayout.Foldout(weaponShooterFoldout,"Weapon Shooter State Presets");
					if(weaponShooterFoldout){
						DefaultsList.weaponItems[viewIndex-1].weaponShooterZoomState = EditorGUILayout.ObjectField("Shooter Zoom State",DefaultsList.weaponItems[viewIndex-1].weaponShooterZoomState,typeof(TextAsset),true) as TextAsset;
					}
					GUILayout.Space(12);
					weaponMuzzShellFoldout = EditorGUILayout.Foldout(weaponMuzzShellFoldout,"Muzzle Flash and Shell Prefabs");
					if(weaponMuzzShellFoldout){
						DefaultsList.weaponItems[viewIndex-1].mzlFlash = EditorGUILayout.ObjectField("Muzzle Flash Prefab",DefaultsList.weaponItems[viewIndex-1].mzlFlash,typeof(GameObject),false) as GameObject;
						DefaultsList.weaponItems[viewIndex-1].shell = EditorGUILayout.ObjectField("Shell Prefab",DefaultsList.weaponItems[viewIndex-1].shell,typeof(GameObject),false) as GameObject;
						DefaultsList.weaponItems[viewIndex-1].shellSize = EditorGUILayout.FloatField("Shell Size",Mathf.Clamp(DefaultsList.weaponItems[viewIndex-1].shellSize,0,2));
					}
					GUILayout.Space(12);
					weaponSoundsAnims = EditorGUILayout.Foldout(weaponSoundsAnims,"Fire Sounds and Animations");
					if(weaponSoundsAnims){
						DefaultsList.weaponItems[viewIndex-1].sndFire = EditorGUILayout.ObjectField("Fire Sound",DefaultsList.weaponItems[viewIndex-1].sndFire,typeof(AudioClip),false) as AudioClip;
						DefaultsList.weaponItems[viewIndex-1].sndDryFire = EditorGUILayout.ObjectField("Dry Fire Sound",DefaultsList.weaponItems[viewIndex-1].sndDryFire,typeof(AudioClip),false) as AudioClip;
						DefaultsList.weaponItems[viewIndex-1].animFire = EditorGUILayout.ObjectField("Fire Animation",DefaultsList.weaponItems[viewIndex-1].animFire,typeof(AnimationClip),false) as AnimationClip;
					}
					GUILayout.Space(12);
					EditorGUILayout.LabelField("WEAPON RELOAD COMPONENTS");
					weaponReloadFoldout = EditorGUILayout.Foldout(weaponReloadFoldout,"Reload Sound and Animations");
					if(weaponReloadFoldout){
						DefaultsList.weaponItems[viewIndex-1].reloadSound = EditorGUILayout.ObjectField("Reload Sound",DefaultsList.weaponItems[viewIndex-1].reloadSound,typeof(AudioClip),false) as AudioClip;
						DefaultsList.weaponItems[viewIndex-1].rldDuration = EditorGUILayout.FloatField("Reload Duration",DefaultsList.weaponItems[viewIndex-1].rldDuration );
						DefaultsList.weaponItems[viewIndex-1].rldAnim = EditorGUILayout.ObjectField("Reload Animation",DefaultsList.weaponItems[viewIndex-1].rldAnim,typeof(AnimationClip),false) as AnimationClip;
					}
					GUILayout.EndScrollView();
				}
				else{
					GUILayout.Label("This inventory list is Empty.");
				}
			}
			if(GUI.changed){
				EditorUtility.SetDirty(DefaultsList);
			}
		}

		void OpenItemList(){
			string absPath = EditorUtility.OpenFilePanel("Select Weapon Defaults List","Assets/RedHawk/ScriptableObject/","");
			if(absPath.StartsWith(Application.dataPath)){
				string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
				DefaultsList = AssetDatabase.LoadAssetAtPath(relPath,typeof(WeaponItemDefaultsList)) as WeaponItemDefaultsList;
				if(DefaultsList){
					EditorPrefs.SetString("ObjectPath",relPath);
				}
			}
		}
		void AddItem(){
			WeaponItemDefaults newItem = new WeaponItemDefaults();
			newItem.weaponDefaultName = "NewWeapon"+(viewIndex);
			newItem.weaponWieldSound = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/UFPS/Base/Sounds/Weapons/PistolWield.ogg",typeof(AudioClip));
			newItem.weaponUnWieldSound = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/UFPS/Base/Sounds/Weapons/UnWield.ogg",typeof(AudioClip));
			newItem.sndFire = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/UFPS/Base/Sounds/Weapons/PistolFire.ogg",typeof(AudioClip));
			newItem.sndDryFire = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/UFPS/Base/Sounds/Weapons/PistolDryFire.ogg",typeof(AudioClip));
			newItem.reloadSound = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/UFPS/Base/Sounds/Weapons/PistolReload.ogg",typeof(AudioClip));
			newItem.weaponReloadState = (TextAsset)AssetDatabase.LoadAssetAtPath("Assets/UFPS/Base/Scripts/Presets/Standard/Weapon/WeaponPistolReload.txt",typeof(TextAsset));
			newItem.weaponZoomState = (TextAsset)AssetDatabase.LoadAssetAtPath("Assets/UFPS/Base/Scripts/Presets/Standard/Weapon/WeaponPistolZoom.txt",typeof(TextAsset));
			newItem.weaponOutOfControlState = (TextAsset)AssetDatabase.LoadAssetAtPath("Assets/UFPS/Base/Scripts/Presets/Standard/Weapon/WeaponPistolOutOfControl.txt",typeof(TextAsset));
			newItem.weaponSlideState = (TextAsset)AssetDatabase.LoadAssetAtPath("Assets/UFPS/Base/Scripts/Presets/Standard/Weapon/WeaponGenericSlide.txt",typeof(TextAsset));
			newItem.weaponCrouchState = (TextAsset)AssetDatabase.LoadAssetAtPath("Assets/UFPS/Base/Scripts/Presets/Standard/Weapon/WeaponPistolCrouch.txt",typeof(TextAsset));
			newItem.weaponRunState = (TextAsset)AssetDatabase.LoadAssetAtPath("Assets/UFPS/Base/Scripts/Presets/Standard/Weapon/WeaponPistolRun.txt",typeof(TextAsset));
			newItem.weaponShooterZoomState =(TextAsset)AssetDatabase.LoadAssetAtPath("Assets/UFPS/Base/Scripts/Presets/Standard/Shooter/ShooterPistolZoom.txt",typeof(TextAsset));
			DefaultsList.weaponItems.Add (newItem);
			viewIndex = DefaultsList.weaponItems.Count;
		}
		void DeleteItem(int index){
			DefaultsList.weaponItems.RemoveAt (index);
		}
		public void SelectItemFromList(string searchFor){
			DefaultsList.weaponItems.Exists(x =>x.weaponDefaultName==searchFor);
		}
	}
}