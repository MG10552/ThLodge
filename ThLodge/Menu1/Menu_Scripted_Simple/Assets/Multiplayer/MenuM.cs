using UnityEngine;
using System.Collections;

public class MenuM : MonoBehaviour 
{
	public GUISkin guiSkin;
	public Texture2D background, LOGO;
	public bool DragWindow = false;
	public string levelToLoadWhenClickedPlay = "";
	public string[] AboutTextLines = new string[0];
	public bool ShowBoard;
	
	private string clicked = "", MessageDisplayOnAbout = "About \n ";
	private Rect WindowRect = new Rect((Screen.width /2) - 100, Screen.height /2, 200, 200);
	private float volume = 1.0f;
	private float music = 1.0f;
	private bool fullscreen = true;

	private void Start()
	{
		for (int x = 0; x < AboutTextLines.Length;x++ )
		{
			MessageDisplayOnAbout += AboutTextLines[x] + " \n ";
		}
		MessageDisplayOnAbout += "Press Esc To Go Back";
	}
	
	private void OnGUI()
	{
		if (ShowBoard) 
		{
			GUILayout.BeginArea(new Rect(Screen.width /4, Screen.height /4, (Screen.width) - (Screen.width /2), (Screen.height) - (Screen.height /2)), GUIContent.none, "Box");
			//score table labels
			GUILayout.BeginVertical();
			GUILayout.Label("Player name");
			GUILayout.Label("Character class");
			GUILayout.Label("Ping (ms)");
			GUILayout.Label("Shutdowns");
			GUILayout.Label("Total points");
			GUILayout.EndVertical();
			/*	foreach (Player pl in NetworkManager.Instance.PlayerList) //like this?
			{
				GUILayout.BeginVertical();

				GUILayout.EndVertical();
			}
			GUILayout.EndArea(); */
		}

		if (background != null)
			GUI.DrawTexture(new Rect(0,0,Screen.width , Screen.height),background);
		if (LOGO != null && clicked != "Back to main menu")
			GUI.DrawTexture(new Rect((Screen.width /2) - 100, 30, 200, 200), LOGO);
		
		GUI.skin = guiSkin;
		if (clicked == "")
		{
			WindowRect = GUI.Window(0, WindowRect, menuFunc, "Quick access menu");
		}
		else if (clicked == "options")
		{
			WindowRect = GUI.Window(1, WindowRect, optionsFunc, "Options");
		}
		else if (clicked == "controls")
		{
			GUILayout.BeginHorizontal();
			//sprite+
			if (GUILayout.Button("Back"))
			{
				clicked = "options";
			}
			GUILayout.EndHorizontal();
		}
		else if (clicked == "resolution")
		{
			GUILayout.BeginVertical();
			for (int x = 0; x < Screen.resolutions.Length;x++ )
			{
				if (GUILayout.Button(Screen.resolutions[x].width + "X" + Screen.resolutions[x].height))
				{
					Screen.SetResolution(Screen.resolutions[x].width,Screen.resolutions[x].height,true);
				}
			}
			GUILayout.EndVertical();
			GUILayout.BeginHorizontal();

			fullscreen = GUI.Toggle(new Rect(0,50,100,80), fullscreen, "Fullscreen");

			if (GUILayout.Button("Back"))
			{
				clicked = "options";
			}
			GUILayout.EndHorizontal();
		}
		else if (clicked == "vote kick")
		{
			GUILayout.Label("Choose the player you would like to kick please. Keep in mind that host cannot be kicked, neither you can vote kick yourself.");
			GUILayout.BeginHorizontal();
			//foreach (Player pl in NetworkManager.Instance.PlayerList)
		
			GUILayout.EndHorizontal();
			if (GUILayout.Button("Initiate kick"))
			{
				//mechanics of voting ??
			}
			if (GUILayout.Button("Back"))
			{
				clicked = "";
			}
		}
		//else if (clicked == "")
	}
	
	private void optionsFunc(int id)
	{
		if (GUILayout.Button("Resolution"))
		{
			clicked = "resolution";
		}

		GUILayout.Box("Volume");
		volume = GUILayout.HorizontalSlider(volume ,0.0f,1.0f);
		AudioListener.volume = volume;

		GUILayout.Box("Music");
		music = GUILayout.HorizontalSlider(music ,0.0f,1.0f);
		AudioListener.volume = music;

		if (GUILayout.Button("Controls' Map"))
		{
			clicked = "controls";
		}

		if (GUILayout.Button("Back"))
		{
			clicked = "";
		}
		if (DragWindow)
			GUI.DragWindow(new Rect (0,0,Screen.width,Screen.height));
	}
	
	private void menuFunc(int id)
	{
		//buttons
		if (GUILayout.Button("Continue")) //continue ?? accurate in multiplayer
		{
			//play game is clicked
			Application.LoadLevel(levelToLoadWhenClickedPlay);
		}
		if (GUILayout.Button("Vote kick")) //dispatch intruders and lazy bums to hell
		{
			clicked = "vote kick";
		}
		if (GUILayout.Button("Leave online session")) //back to M_menu or multiplayer menu?
		{
			Application.LoadLevel("Main menu");
		}
		if (GUILayout.Button("Options")) // ok
		{
			clicked = "options";
		}
		if (GUILayout.Button("Back to main menu")) //back to M_menu
		{
			Application.LoadLevel("Main menu");
		}
		if (GUILayout.Button("EXIT")) // quit the game
		{
			Application.Quit();
		}
		if (DragWindow)
			GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
	}
	
	private void Update()
	{
		if (clicked == "about" && Input.GetKey (KeyCode.Escape)) 
		{ 
			clicked = "";
		}
		if (Input.GetKeyDown (KeyCode.Tab))
			ShowBoard = !ShowBoard;
	}
}