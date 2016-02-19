using UnityEngine;
using System.Collections;
using UnityEditor;

namespace RedHawk{
	public enum meleeType{melee,grenade}

	public class TestMeleeGrenade : EditorWindow {
		public GameObject baseHero;
		public GameObject meleeWeapon;
		public GameObject newWeaponObject;
		public string newWeaponName;
		public vp_FPCamera _UFPSCamera;
		public meleeType selectedType;
		Vector2 _ScrollPosition ;
		[MenuItem("Tools/RedHawk/Build a Melee or Grenade Weapon",false,1)]
		public static void myWindow()
		{
			EditorWindow window = EditorWindow.GetWindow<TestMeleeGrenade>(true,"Copy a Melee or Grenade Weapon");
			window.minSize = new Vector2(500, 300);
		}

		void OnGUI(){
			_ScrollPosition = GUILayout.BeginScrollView(_ScrollPosition);
			EditorGUILayout.HelpBox("Select the Type of weapon to use (Melee or Grenade).  This uses the HeroHDWeapon as the basis.",MessageType.Info);
			selectedType = (meleeType)EditorGUILayout.EnumPopup("Weapon Type",selectedType);

			newWeaponObject = EditorGUILayout.ObjectField("Assign the New Weapon",newWeaponObject,typeof(GameObject),true) as GameObject;
			newWeaponName = EditorGUILayout.TextField("Weapon Name",newWeaponName);
			_UFPSCamera = EditorGUILayout.ObjectField("UFPS Weapon Camera",_UFPSCamera,typeof(vp_FPCamera),true) as vp_FPCamera;
			if(GUILayout.Button("Add Melee Weapon")){
				if(selectedType == meleeType.melee)
					getTheKnife();
				else
					getTheGrenade();
			}
			GUILayout.EndScrollView();
		}
		void getTheKnife(){
			baseHero = Instantiate( EditorGUIUtility.Load("Assets/UFPS/Base/Content/Prefabs/Players/HeroHDWeapons.prefab") as GameObject);
			//Debug.Log("GO for baseHero is "+baseHero);
			meleeWeapon = baseHero.transform.FindChild("FPSCamera/5Knife").gameObject;
			meleeWeapon.name = newWeaponName;
			//Debug.Log("GO for meleeWeapon is "+meleeWeapon);
			meleeWeapon.transform.parent = _UFPSCamera.gameObject.transform;
			meleeWeapon.GetComponent<vp_FPWeapon>().WeaponPrefab = newWeaponObject;
			DestroyImmediate(baseHero);
		}
		void getTheGrenade(){
			baseHero = Instantiate( EditorGUIUtility.Load("Assets/UFPS/Base/Content/Prefabs/Players/HeroHDWeapons.prefab") as GameObject);
			//Debug.Log("GO for baseHero is "+baseHero);
			meleeWeapon = baseHero.transform.FindChild("FPSCamera/4GrenadeThrow").gameObject;
			meleeWeapon.name = newWeaponName;
			//Debug.Log("GO for meleeWeapon is "+meleeWeapon);
			meleeWeapon.transform.parent = _UFPSCamera.gameObject.transform;
			meleeWeapon.GetComponent<vp_FPWeapon>().WeaponPrefab = newWeaponObject;
			DestroyImmediate(baseHero);
		}

	}
}