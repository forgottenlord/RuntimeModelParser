using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public enum ButtonStatus
{
	BtnNone = 0,
	BtnChar,
	BtnAni,
	BtnDress
}

[RequireComponent (typeof(RTLoadImport))]
public class RTManager : MonoBehaviour
{
	[HideInInspector]
	public RTLoadImport rtImporter = null;
	[HideInInspector]
	public Material[] useMat;
	public RuntimeAnimatorController[] aniController;
	public GameObject[] hatObj;
	public GameObject[] glassObj;
	public GameObject[] shirtObj;
	public GameObject[] pantsObj;

	private FollowCamera fCam = null;

	private System.DateTime calcTime;

	private Vector2 scroll;
	void Start ()
	{
		fCam = gameObject.GetComponent<FollowCamera>();
		rtImporter = gameObject.GetComponent<RTLoadImport>();
	}

	IEnumerator loadItemCorutine(ModelInfo modelInfo)
	{
		Debug.Log(modelInfo.name + " Loading...");
		Debug.Log(modelInfo.haveTex);

		if(rtImporter)
		{
			calcTime = System.DateTime.Now;
			rtImporter.load(modelInfo);
		}
		while(!rtImporter.canLoad())
		{
			yield return null;
		}

		System.DateTime endTime = System.DateTime.Now;
		applyAnimation(aniController[0]);
	}

	void applyAnimation(RuntimeAnimatorController rac)
	{
		if(rtImporter != null)
		{
			foreach(GameObject go in rtImporter.getLoadObjects())
			{
				go.GetComponent<Animator>().runtimeAnimatorController = rac;
			}
		}
	}

	/*void openDressWindow() 
	{ 
		GUIStyle fileStyle = GUI.skin.button;
		fileStyle.fontSize = 50;
		fileStyle.normal.textColor = Color.white;

		scroll = GUILayout.BeginScrollView(scroll);
		scroll += fCam.getMovePos() * 50.0f;
		GUILayout.Label("\r\n\r\n\r\n\r\n");
		GUILayout.BeginHorizontal(); 

		for(int i = 0; i < hatObj.Length; i++)
		{
			GUILayout.BeginHorizontal(); 
			if(GUILayout.Button(("\r\n" + hatObj[i].name + "\r\n"), fileStyle) ) 
			{  
				if((rtImporter != null) && rtImporter.getLoadObjects().Count > 0)
				{
					hatObj[i].SetActive(!hatObj[i].activeSelf);
					hatObj[i].GetComponent<Dress>().setTracking(rtImporter.getLoadObject(0));

					for(int j = 0; j < hatObj.Length; j++)
					{
						if(i != j)
						{
							hatObj[j].SetActive(false);
						}
					}
				}
				
				btnStat = ButtonStatus.BtnNone;
			} 
			GUILayout.Box("", new GUIStyle());

			GUILayout.EndHorizontal(); 
		}

		for(int i = 0; i < glassObj.Length; i++)
		{
			GUILayout.BeginHorizontal(); 
			if(GUILayout.Button(("\r\n" + glassObj[i].name + "\r\n"), fileStyle) ) 
			{  
				if((rtImporter != null) && rtImporter.getLoadObjects().Count > 0)
				{
					glassObj[i].SetActive(!glassObj[i].activeSelf);
					glassObj[i].GetComponent<Dress>().setTracking(rtImporter.getLoadObject(0));

					for(int j = 0; j < glassObj.Length; j++)
					{
						if(i != j)
						{
							glassObj[j].SetActive(false);
						}
					}
				}

				btnStat = ButtonStatus.BtnNone;
			} 
			GUILayout.Box("", new GUIStyle());

			GUILayout.EndHorizontal(); 
		}

		for(int i = 0; i < shirtObj.Length; i++)
		{
			GUILayout.BeginHorizontal(); 
			if(GUILayout.Button(("\r\n" + shirtObj[i].name + "\r\n"), fileStyle) ) 
			{  
				if((rtImporter != null) && rtImporter.getLoadObjects().Count > 0)
				{
					shirtObj[i].SetActive(!shirtObj[i].activeSelf);
					shirtObj[i].GetComponent<Dress>().setTracking(rtImporter.getLoadObject(0));

					for(int j = 0; j < shirtObj.Length; j++)
					{
						if(i != j)
						{
							shirtObj[j].SetActive(false);
						}
					}
				}

				btnStat = ButtonStatus.BtnNone;
			} 
			GUILayout.Box("", new GUIStyle());

			GUILayout.EndHorizontal(); 
		}

		for(int i = 0; i < pantsObj.Length; i++)
		{
			GUILayout.BeginHorizontal(); 
			if(GUILayout.Button(("\r\n" + pantsObj[i].name + "\r\n"), fileStyle) ) 
			{  
				if((rtImporter != null) && rtImporter.getLoadObjects().Count > 0)
				{
					pantsObj[i].SetActive(!pantsObj[i].activeSelf);
					pantsObj[i].GetComponent<Dress>().setTracking(rtImporter.getLoadObject(0));

					for(int j = 0; j < pantsObj.Length; j++)
					{
						if(i != j)
						{
							pantsObj[j].SetActive(false);
						}
					}
				}

				btnStat = ButtonStatus.BtnNone;
			} 
			GUILayout.Box("", new GUIStyle());

			GUILayout.EndHorizontal(); 
		}
		GUILayout.EndHorizontal();
		GUILayout.EndScrollView();
	}*/

	void openAniWindow() 
	{ 
		GUIStyle fileStyle = GUI.skin.button;
		fileStyle.fontSize = 50;
		fileStyle.normal.textColor = Color.white;

		scroll = GUILayout.BeginScrollView (scroll); 
		scroll += fCam.getMovePos() * 50.0f;

		for(int i = 0; i < aniController.Length; i++)
		{
			if(GUILayout.Button(("\r\n" + aniController[i].name + "\r\n"), fileStyle) ) 
			{
				applyAnimation(aniController[i]);
			}
		}
	}
	
	public void SelectCharacter(string path, int spaceNum = 0, int index = 0) 
	{ 
		FileInfo fileSelection;
		DirectoryInfo directoryInfo;

		directoryInfo = new DirectoryInfo(path);
		fileSelection = new FileInfo(path);
		
		if(fileSelection != null)
		{
			ModelInfo tempModelinfo = new ModelInfo();
			string[] infoData = fileSelection.Name.Split('.');
			tempModelinfo.name = infoData[0].Trim();
			tempModelinfo.format = infoData[1].Trim();
			tempModelinfo.directory = fileSelection.Directory.FullName.Replace("\\", "/");

			foreach(FileInfo fi in directoryInfo.GetFiles("*.bin"))
			{
				if(fi.Name.ToLower().Equals(tempModelinfo.name.ToLower() + ".bin"))
				{
					tempModelinfo.format = "bin";
					break;
				}
			}

			foreach(FileInfo fi in directoryInfo.GetFiles("*.jpg"))
			{
				if(fi.Name.ToLower().Equals(tempModelinfo.name.ToLower() + ".jpg"))
				{
					tempModelinfo.haveTex = true;
					break;
				}
			}

			for(int i = 0; i < hatObj.Length; i++)
			{
				hatObj[i].SetActive(false);
			}

			for(int i = 0; i < glassObj.Length; i++)
			{
				glassObj[i].SetActive(false);
			}

			for(int i = 0; i < shirtObj.Length; i++)
			{
				shirtObj[i].SetActive(false);
			}

			for(int i = 0; i < pantsObj.Length; i++)
			{
				pantsObj[i].SetActive(false);
			}

			if((rtImporter != null) && rtImporter.getLoadObjects().Count > 0)
			{
				rtImporter.unload();
			}

			StartCoroutine(loadItemCorutine(tempModelinfo));
		}
	}
}