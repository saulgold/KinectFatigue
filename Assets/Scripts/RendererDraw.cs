using UnityEngine;
using System.Collections;

public static class RendererDraw {

	/// <summary>
	/// Draws the line.
	/// </summary>
	/// <param name="start">Start.</param>
	/// <param name="end">End.</param>
	/// <param name="width">Width.</param>
	/// <param name="texture">Texture.</param>
	public static void DrawLine(Vector2 start, Vector2 end, int width, Texture2D texture)
	{
		Vector2 d = end - start;
		float a = Mathf.Rad2Deg * Mathf.Atan(d.y / d.x);
		if (d.x < 0)
			a += 180;
		
		int width2 = (int) Mathf.Ceil(width / 2);
		
		GUIUtility.RotateAroundPivot(a, start);
		GUI.DrawTexture(new Rect(start.x, start.y - width2, d.magnitude, width), texture);
		GUIUtility.RotateAroundPivot(-a, start);
	}

	public static void DrawQuad(Rect position, Color color, Texture2D texture) {
		
		texture.SetPixel(0,0,color);
		texture.Apply();
		GUI.skin.box.normal.background = texture;
		GUI.Box(position, GUIContent.none);
	}

}
