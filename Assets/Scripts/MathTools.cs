using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class MathTools {

	public static int MaxValue(List<int> vals) {
		if(vals.Count == 0) return 0; 
		int max = vals[0];
		foreach(int val in vals) {
			if(val > max ) max = val;
		}
		return max;
	}
	
	public static int MinValue(List<int> vals) {
		if(vals.Count == 0) return 0; 
		int min = vals[0];
		foreach(int val in vals) {
			if(val < min ) min = val;
		}
		return min;
	}
}
