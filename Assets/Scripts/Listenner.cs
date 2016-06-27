using UnityEngine;
using System.Collections;

 public abstract class Listener : MonoBehaviour {

	abstract public void EventAction(MonoBehaviour mono = null);
	abstract public void EventAction(Component compo);
	abstract public void EventActionTrue();
	abstract public void EventActionFalse();

}
