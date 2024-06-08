using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LoaderUI : MonoBehaviour
{
	[SerializeField] private RTManager rtManager;
	[SerializeField] private Button refreshButton;
	[SerializeField] private InputField modelDirPath;
	[SerializeField] private Dropdown fileNames;
	[SerializeField] private Button openButton;

	string initPath;
	void Start()
	{
		initPath = Application.dataPath + "/Resources/Meshes/Models/";
#if UNITY_ANDROID
		initPath = AndroidPath.filePath;
#endif

		modelDirPath.text = initPath;
		Refresh();

		refreshButton.onClick.AddListener(() =>
		{
			Refresh();
		});

		openButton.onClick.AddListener(() =>
		{
			Debug.Log(fileNames.value);
			string path = Path.Combine(modelDirPath.text, fileNames.options[fileNames.value].text);
			Debug.Log(path);
			rtManager.SelectCharacter(path);
		});

		//openDressWindow();
	}

	string[] extensions = new string[] { "*.fbx", "*.bin" };
	void Refresh()
	{
		List<string> files = new List<string>();
		foreach (string ext in extensions)
		{
			files.AddRange(Directory.GetFiles(modelDirPath.text, ext));
		}

		fileNames.ClearOptions();
		Dropdown.OptionData[] options = new Dropdown.OptionData[files.Count];

		for (int n = 0; n < files.Count; n++)
		{
			options[n] = new Dropdown.OptionData()
			{
				text = Path.GetFileName(files[n])
			};
		}
		fileNames.AddOptions(options.ToList());
	}
}
