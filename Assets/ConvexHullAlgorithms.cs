using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ConvexHullAlgorithms
{
	public const float SCAN_EPSILON = 0.001f;
	
	public static void GrahamScanSortingTest(List<Vector3> points, List<Vector3> ccwPolygonPoints)
	{
		if(points.Count < 3)
		{
			return;
		}
		if(points.Count == 3)
		{
			for(int i=0; i < points.Count; i++)
			{
				ccwPolygonPoints.Add(points[i]);
			}
			return;
		}
		//find the point having the lowest y coordinate (use x for euqual cases if any)
		//put it at the end of the list and remove it (this will be point P)
		Vector3 tmp = Vector3.zero;
		for( int i=points.Count-1; i >=0 ; i-- )
		{
			if(points[points.Count - 1].z > points[i].z)
			{
				//swap
				tmp = points[points.Count - 1];
				points[points.Count - 1] = points[i];
				points[i] = tmp;
			}
			else if(points[points.Count - 1].z == points[i].z)
			{
				if(points[points.Count - 1].x < points[i].x)
				{
					tmp = points[points.Count - 1];
					points[points.Count - 1] = points[i];
					points[i] = tmp;					
				}
			}
		}
		//sort the remaining point according the angle they form with the point P
		Vector3 P = points[points.Count - 1];
		points.RemoveAt(points.Count - 1);
		SortAlgorithms.HeapSortGrahamScan(points,ref P);
		points.Add(P);//just a test
	}
	
	public static bool GrahamScanTest(Vector3 startPoint, List<Vector3> points, List<Vector3> ccwPolygonPoints, ref int algorithmStepIndex)
	{
		if(algorithmStepIndex == 0)
		{
			ccwPolygonPoints.Add(startPoint);
			ccwPolygonPoints.Add(points[0]);
			algorithmStepIndex++;
			return true;//keep calling me
		}
		else if( algorithmStepIndex < points.Count)
		{
			if(Vector3.Cross(ccwPolygonPoints[ccwPolygonPoints.Count-1]-ccwPolygonPoints[ccwPolygonPoints.Count-2],
			                 points[algorithmStepIndex]-ccwPolygonPoints[ccwPolygonPoints.Count-2]).y < -SCAN_EPSILON )//Left turn, it should be > 0  but I am using 3D coordinate using z as y axis reported in the algorithm
			{
				ccwPolygonPoints.Add(points[algorithmStepIndex]);
				algorithmStepIndex++;
			}
			else
			{
				ccwPolygonPoints.RemoveAt(ccwPolygonPoints.Count - 1);
				if(ccwPolygonPoints.Count == 1)//in order to make the algorithm more robust
				{
					ccwPolygonPoints.Add(points[algorithmStepIndex]);
					algorithmStepIndex++;					
				}
			}        
			return true;//keep calling me
		}
		return false;//algorithm is over, it is pointless calling me
	}
	
	/// <summary>
	/// Performs the computation of the convex hull of the passed points
	/// through Graham scan http://en.wikipedia.org/wiki/Graham_scan
	/// the returned points are the vertices of the polygon sorted
	/// counter clockwise
	/// </summary>
	/// <param name="points"> The points of which to find the convex hull that contains them
	/// A <see cref="List<Vector3>"/>
	/// </param>
	/// <returns>
	/// A <see cref="List<Vector3>"/> The list of counter clockwise sorted vertices of the convex hull polygon
	/// </returns>
	public static List<Vector3> GrahamScan(List<Vector3> points)
	{
		List<Vector3> ccwPolygonPoints = new List<Vector3> ();
		/*
		if(points.Count < 3)
		{
			return ;
		}
		if(points.Count == 3)
		{
			for(int i=0; i < points.Count; i++)
			{
				ccwPolygonPoints.Add(points[i]);
			}
			return ;
		}
		*/
		if(points.Count > 3)
		{
			//find the point having the lowest y coordinate (use x for euqual cases if any)
			//put it at the end of the list and remove it (this will be point P)
			Vector3 tmp = Vector3.zero;
			for( int i=points.Count-1; i >=0 ; i-- )
			{
				if(points[points.Count - 1].z > points[i].z)
				{
					//swap
					tmp = points[points.Count - 1];
					points[points.Count - 1] = points[i];
					points[i] = tmp;
				}
				else if(points[points.Count - 1].z == points[i].z)
				{
					if(points[points.Count - 1].x < points[i].x)
					{
						tmp = points[points.Count - 1];
						points[points.Count - 1] = points[i];
						points[i] = tmp;					
					}
				}
			}
			//sort the remaining point according the angle they form with the point P
			Vector3 P = points[points.Count - 1];
			points.RemoveAt(points.Count - 1);
			SortAlgorithms.HeapSortGrahamScan(points,ref P);
			ccwPolygonPoints.Add(P);
			ccwPolygonPoints.Add(points[0]);
			//points.Add(P);
			int k = 1;
			while( k < points.Count)
			{
				//Debug.Log("while graham scan");
				if(Vector3.Cross(ccwPolygonPoints[ccwPolygonPoints.Count-1]-ccwPolygonPoints[ccwPolygonPoints.Count-2],
				                 points[k]-ccwPolygonPoints[ccwPolygonPoints.Count-2]).y < -SCAN_EPSILON )//Left turn, it should be > 0  but I am using 3D coordinate using z as y axis reported in the algorithm
				{
					ccwPolygonPoints.Add(points[k]);
					k++;
				}
				else
				{
					ccwPolygonPoints.RemoveAt(ccwPolygonPoints.Count - 1);
					if(ccwPolygonPoints.Count == 1)//in order to make the algorithm more robust
					{
						ccwPolygonPoints.Add(points[k]);
						k++;					
					}
				}			
			}
		}
		else if(points.Count == 3)
		{
			for(int i=0; i < points.Count; i++)
			{
				ccwPolygonPoints.Add(points[i]);
			}
		}
		return ccwPolygonPoints;
	}

	public static Mesh GetBounds(IEnumerable<GameObject> objs, Matrix4x4 worldToLocal){

		var hasMesh = objs.Where(o => o.GetComponent<MeshFilter>()).Where(o => o.GetComponent<MeshFilter>().sharedMesh != null);
		var vertices = hasMesh.SelectMany(m => m.GetComponent<MeshFilter>().sharedMesh.vertices.Select(v => m.transform.localToWorldMatrix.MultiplyPoint3x4(v)));
		var flattened = from v in vertices select new Vector3(v.x, 0, v.z);
		var distinct = flattened.Distinct();
		var extremes = vertices.Aggregate(
			new {min = Mathf.Infinity, max = Mathf.NegativeInfinity}, 
			(curr, v) => new {min = v.y < curr.min ? v.y : curr.min, max = v.y > curr.max ? v.y : curr.max});
		var convex = ConvexHullAlgorithms.GrahamScan(distinct.ToList());
		//Actually switching dimension order here, shouldn't matter (No Loss Of Generality);
		int[] triangulated = (new Triangulator(convex.Select(v => new Vector2(v.x, v.z)))).Triangulate();

		var verts = new List<Vector3>();
		var tris = new List<int>();

		verts.InsertRange(0, convex.Select(v => new Vector3(v.x, extremes.min, v.z)));
		tris.InsertRange(0, triangulated);

		var increment = verts.Count;
		verts.InsertRange(increment, convex.Select(v => new Vector3(v.x, extremes.max, v.z)));
		tris.InsertRange(tris.Count, triangulated.Select(i => i + increment));

		for (int i = 0 ; i < convex.Count - 1; i++){
			//convex is sorted counterclockwise looking from above y,
			//So clockwise from outside would mean going i => i + increment => i + 1
			tris.Add(i);
			tris.Add(i + increment);
			tris.Add(i + 1);
			tris.Add(i + increment);
			tris.Add(i + increment + 1);
			tris.Add(i + 1);
		}

		var outputMesh = new Mesh();
		outputMesh.vertices = verts.Select(v => worldToLocal.MultiplyPoint3x4(v)).ToArray();
		outputMesh.triangles = tris.ToArray();
		outputMesh.RecalculateNormals();
		outputMesh.RecalculateBounds();
		return outputMesh;
	}

}

