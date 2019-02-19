using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlecryTrigger : TriggerBase {
	// Subscribe the effectinput's function to the correct event/events.
	protected override void Subscribe() {
		GetComponent<UnitCard>().OnBattlecry += effectInputScript.onDoThing;
	}
}
