using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace GameUtils {
//public class GameUtils : MonoBehaviour
//{
//
//	// Use this for initialization
//	void Start ()
//	{
//	
//	}
//	
//	// Update is called once per frame
//	void Update ()
//	{
//	
//	}
//}

//	public class 

	public class ParamData{
		public int number;
		public Vector2 data;
		public ParamData (int paramNumber, Vector2 data){
			number = paramNumber;
			this.data= data;
		}
	}

	public class Functional{
		public static IEnumerable<T> Zip<A, B, T>(
			IEnumerable<A> seqA, IEnumerable<B> seqB, Func<A, B, T> func)
		{
			using (var iteratorA = seqA.GetEnumerator())
				using (var iteratorB = seqB.GetEnumerator())
			{
				while (iteratorA.MoveNext() && iteratorB.MoveNext())
				{
					yield return func(iteratorA.Current, iteratorB.Current);
				}
			}
		}
	}
	
}