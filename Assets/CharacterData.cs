using UnityEngine;
using System.Collections;

public class CharacterData  {

	public  Vector3 Hips;
	public  Vector3 Ribs;
	public  Vector3 RightKnee;
	public  Vector3 LeftKnee;
	public  Vector3 LeftShoulder;
	public  Vector3 RightShoulder;

	public  Vector3 HipsOffset;
	public  Vector3 RibsOffset;
	public  Vector3 RightKneeOffset;
	public  Vector3 LeftKneeOffset;
	public  Vector3 LeftShoulderOffset;
	public  Vector3 RightShoulderOffset;

	public CharacterData(Transform Hips, Transform Ribs, Transform RightKnee, Transform LeftKnee, Transform LeftShoulder, Transform RightShoulder,
	                     Vector3 HipsOffset,Vector3 RibsOffset,Vector3 RightKneeOffset,Vector3 LeftKneeOffset, Vector3 LeftShoulderOffset,Vector3 RightShoulderOffset )
	{
		this.Hips = new Vector3(Hips.position.x, Hips.position.y, Hips.position.z);
		this.Ribs = new Vector3(Ribs.position.x, Ribs.position.y, Ribs.position.z);
		this.RightKnee = new Vector3(RightKnee.position.x, RightKnee.position.y, RightKnee.position.z);
		this.LeftKnee = new Vector3(LeftKnee.position.x, LeftKnee.position.y, LeftKnee.position.z);
		this.LeftShoulder = new Vector3(LeftShoulder.position.x, LeftShoulder.position.y, LeftShoulder.position.z);
		this.RightShoulder = new Vector3(RightShoulder.position.x, RightShoulder.position.y, RightShoulder.position.z);

		this.HipsOffset = HipsOffset;
		this.RibsOffset = RibsOffset;
		this.RightKneeOffset = RightKneeOffset;
		this.LeftKneeOffset = LeftKneeOffset;
		this.LeftShoulderOffset  = LeftShoulderOffset;
		this.RightShoulderOffset = RightShoulderOffset;

	}

	public float GetLengthArm() {
		return Mathf.Abs(Vector3.Distance(this.Ribs, this.RightShoulder));
	}
}
