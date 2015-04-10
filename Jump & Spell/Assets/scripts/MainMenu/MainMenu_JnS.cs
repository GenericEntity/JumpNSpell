using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenu_JnS : MonoBehaviour 
{
	[SerializeField]
	private GameObject mainMenuPanel;
	[SerializeField]
	private GameObject levelSelectPanel;
	[SerializeField]
	private GameObject optionsPanel;
	[SerializeField]
	private GameObject creditsPanel;

	private List<GameObject> allPanels;

	void Awake()
	{
		allPanels = new List<GameObject>();
		allPanels.Add(mainMenuPanel);
		allPanels.Add(levelSelectPanel);
		allPanels.Add(optionsPanel);
		allPanels.Add(creditsPanel);
	}

	void Start()
	{
		ShowPanel(mainMenuPanel);
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void ShowPanel(GameObject panel)
	{
		for(int i = 0; i < allPanels.Count; ++i)
		{
			allPanels[i].SetActive(false);
		}

		panel.SetActive(true);
	}

	public void SelectLevel(int index)
	{
		Application.LoadLevel(index);
	}
}
