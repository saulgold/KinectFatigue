using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class PlotManager : MonoBehaviour {


	public float x;
	public float y;
	private float width;
	private float height;
	public float maxValue;
	public float minValue;

	public Plotter plot;
	private Texture2D texture;
	private Rect rect;
	//private float gapX;
	private int numberScale;
	public Vector2 scrollPosition = Vector2.zero;
	private int numberValues = 0;
	private float gapX = 20;
	private float gapY = 10;
	public GUISkin mySkin;


	void Start() {

		numberScale = (int) Math.Round ( 30*(maxValue - minValue)/height);

		texture = new Texture2D(1,1);
	
		rect = new Rect (x, y, width, height);


	}

	public bool hasValuesToShow() {
		if(numberValues >0) return true;
		return false;
	}


	public void SetPlot(Color c) {
		plot = new Plotter(c,rect,gapX, gapY);
	}

	public void SetSize(float w, float h) {
		rect = new Rect (x, y, w, h);
		width = w;
		height = h;
	}

	public void AddValue(List<int> vals) {
		numberValues = vals.Count;
		SetGap(vals);

		plot.AddValue (vals);
	}

	public void setGapPlus() {

	}

	public void setGapMinus() {
		
	}
	//Define a gap (empty space between two points)
	private void SetGap() {
		if(numberValues !=0){
			gapY = 200/(maxValue - minValue);
			gapX = 800/numberValues;
			
			if(gapX < 30) gapX = 30;
			//if(gapY < 10) gapY = 10;
			
			if(gapY > 30) gapY = 30; 
			
			
			float w = numberValues*gapX;
			float h = (maxValue - minValue)*gapY+20;
			
			
			if((maxValue - minValue) > 5) {
				numberScale = (int) Mathf.Round ((maxValue - minValue)/10);
			} else numberScale = 1;
			
			plot.SetGap(gapX, gapY);
			plot.SetRect(new Rect(50,50, w,h));
		}
	}

	public void SetGap(List<int> vals) {
		SortValues(vals);
		SetGap();
	}

	//Get the maximum and minimum values for a set of values
	private void SortValues(List<int> vals) {
		maxValue = MathTools.MaxValue(vals);
		minValue = MathTools.MinValue(vals);

	}

	public void Draw() {
		GUI.skin = mySkin;
		GUI.skin.label.fontSize =(int) Math.Round (8 * Screen.width / (800 * 1.0));

		if(numberValues!=0) {
			float w = numberValues*gapX;
			float h = (maxValue - minValue)*gapY+20;
			//float h = numberValues*gapY+20;
			if(w < width) w = width;

			scrollPosition = GUI.BeginScrollView(new Rect(x, y, width-10, height-10), scrollPosition, new Rect(0, 0, w+100, h+height),true,true);

			RendererDraw.DrawQuad (new Rect(60,50, w, h) , Color.grey, texture);

			for(int i = 0;i <=(maxValue-minValue);i+=numberScale) {
				GUI.Label(new Rect(10,  h - i*gapY +40,50,40),(minValue+i).ToString());

			}
			GUI.Label(new Rect(30, 20, 100,40), "Score");
			plot.Draw();
			for(int i = 0; i< numberValues; i++)
				GUI.Label(new Rect(60 + i*gapX, h+55, 50,40), i.ToString());
			GUI.Label(new Rect(60 + (numberValues-1)*gapX+30, h+55, 100,40), "Session");
			GUI.EndScrollView();
		}
		
	}


	public class Plotter {

		public List<int> datas = new List<int>();

		private Color _color;
		private float x;
		private float y;
		private Texture2D texture;
		private Rect _rect;
		private float gapY;
		private float gapX;
		private float min;
		//private float max;


		public Plotter(Color c, Rect rect, float gapX, float gapY ) {
			this.gapX = gapX;
			this.gapY = gapY;
			texture = new Texture2D(1,1);
			_color = c;
			_rect = rect;


		}

		public void SetGap(float valueX, float valueY) {
			gapY = valueY;
			gapX = valueX;
		}

		public void AddValue(List<int> vals) {
			datas.Clear();
			for (int i=0; i<vals.Count; i++) {
				datas.Add(vals[i]);
			}
			min = MathTools.MinValue(vals);
			//max = MathTools.MaxValue(vals);
		}

		public void SetRect(Rect rect) {
			_rect = rect;
		}


		private float getX(int i) {
			return _rect.x+(float)i*gapX;
		}

		private float getY(int i) {
			return _rect.y + _rect.height-((float)datas[i]-min)*gapY;
		}

		public void Draw() {
			for (int i=0; i<datas.Count; i++) {
				float posX = getX(i);
				float posY = getY(i);

				if((i+1)<datas.Count)
					RendererDraw.DrawLine(new Vector2(posX+10,posY),new Vector2(getX(i+1)+10,getY (i+1)),1,texture);
				RendererDraw.DrawQuad(new Rect(posX+10, posY,1.0f,1.0f),_color,texture);
			}
		}






	}
}
