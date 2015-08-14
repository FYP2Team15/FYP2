using UnityEngine;
using System.Collections;

public class TileController : GridController
{   
	public override void Trigger (TriggerEvent triggerEvent)
	{
		
		switch (triggerEvent) {
		case TriggerEvent.StartHover:
			((TileModel)model).StartHover ();
			break;
			
		case TriggerEvent.StopHover:
			((TileModel)model).StopHover ();
			break;
			
		case TriggerEvent.OnLeftClick:
			((TileModel)model).OnLeftClick ();
			break;
			
		default:
			break;
		}
	}
}