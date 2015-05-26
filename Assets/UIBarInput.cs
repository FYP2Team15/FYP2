using UnityEngine;
using System.Collections;

public class UIBarInput : MonoBehaviour {

	public GUITexture StartButton;

	void Update()
	{
		if(StartButton.HitTest(Input.mousePosition))
		{
				
						Application.LoadLevel (1);
				
		}
	}

}
