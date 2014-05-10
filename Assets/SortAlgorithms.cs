using System;
using System.Collections.Generic;
using UnityEngine;

public class SortAlgorithms
{
	//http://en.wikipedia.org/wiki/Graham_scan
	public static void HeapSortGrahamScan(List<Vector3> points, ref Vector3 startPoint)
	{	
		int array_size = points.Count;
		Vector3 temp;
		for (int i = (array_size / 2)-1; i >= 0; i--)
		{
			HeapifyGrahamScan(points, i, array_size - 1,ref startPoint);
		}
		for (int i = array_size-1; i >= 1; i--)
		{//delete the current binary heap root and re-hepify
			temp = points[0];
			points[0] = points[i];
			points[i] = temp;
			HeapifyGrahamScan(points, 0, i-1,ref startPoint);
		}
	}
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="a">
	/// A <see cref="Vector3"/>
	/// </param>
	/// <param name="b">
	/// A <see cref="Vector3"/>
	/// </param>
	/// <returns> -1 if a < b, 0 otherwise
	/// A <see cref="System.Int32"/>
	/// </returns>
	private static int ComparePointGrahamScan( Vector3 a,  Vector3 b, ref Vector3 startPoint)
	{
		Vector3 ap = a-startPoint;
		Vector3 bp = b-startPoint;
		if( Mathf.Abs( ap.x - bp.x) > Mathf.Epsilon || Mathf.Abs( ap.y - bp.y) > Mathf.Epsilon || Mathf.Abs( ap.z - bp.z) > Mathf.Epsilon )
		{
			float apMag = ap.magnitude;
			if(apMag > Mathf.Epsilon)
			{
				float bpMag = bp.magnitude;
				if(bpMag > Mathf.Epsilon)
				{
					if( ap.x / apMag > bp.x / bpMag )
					{
						return -1;
					}
					else if(ap.x / apMag == bp.x / bpMag)
					{
						if(apMag < bpMag)//farther points shall be evaluated before than closer points
						{
							return -1;
						}
						else
						{
							return 0;
						}
					}
				}
				else
				{
					return 0;
				}
			}
			else
			{
				return -1;
			}
			return 0;
		}
		else
		{
			return 0;
		}
	}
	//heap is as follow: index 1 is the root children are at 2*1 (left child) and 2*i+1 (right child), parent is at floor(i/2)
	//element 0 is kept as special having only one child at index 1 (this makes it possible the child / parent relation described above 
	//which is faster than the relation children at 2*1+1 and 2*i+2 that takes place when 0 is the root and has the first two children at indesx 1 and index 2 )
	//heapify means that each parent is guaranteed to be greater than all of its two children
	private static void HeapifyGrahamScan(List<Vector3> points, int startIndex, int maxHeapIndex, ref Vector3 startPoint)
	{
		int childIndex = startIndex << 1;
		bool keepWorking = true;
		Vector3 temp;
		while(childIndex <= maxHeapIndex && keepWorking)// startIndex * 2
		{
			if(childIndex < maxHeapIndex)
			{
				//if(points[childIndex] < points[childIndex+1])
				if(ComparePointGrahamScan(points[childIndex],points[childIndex+1],ref startPoint) < 0 )
				{
					childIndex++;
				}
			}
			//if(points[startIndex] < points[childIndex])
			if(ComparePointGrahamScan(points[startIndex],points[childIndex],ref startPoint) < 0)
			{
				//swap 
				temp = points[startIndex];
				points[startIndex] = points[childIndex];
				points[childIndex] = temp;
				startIndex = childIndex;
				childIndex = startIndex << 1;
			}
			else
			{
				keepWorking = false;
			}
		}
	}
	
	public static void HeapSort(float[] values)
	{	
		int array_size = values.Length;
		float temp;
		for (int i = (array_size / 2)-1; i >= 0; i--)
		{
			Heapify(values, i, array_size - 1);
		}
		for (int i = array_size-1; i >= 1; i--)
		{//delete the current binary heap root and re-hepify
			temp = values[0];
			values[0] = values[i];
			values[i] = temp;
			Heapify(values, 0, i-1);
		}
	}
	
	//heap is as follow: index 1 is the root children are at 2*1 (left child) and 2*i+1 (right child), parent is at floor(i/2)
	//element 0 is kept as special having only one child at index 1 (this makes it possible the child / parent relation described above 
	//which is faster than the relation children at 2*1+1 and 2*i+2 that takes place when 0 is the root and has the first two children at indesx 1 and index 2 )
	//heapify means that each parent is guaranteed to be greater than all of its two children
	private static void Heapify(float[] values, int startIndex, int maxHeapIndex)
	{
		int childIndex = startIndex << 1;
		bool keepWorking = true;
		float temp;
		while(childIndex <= maxHeapIndex && keepWorking)// startIndex * 2
		{
			if(childIndex < maxHeapIndex)
			{
				if(values[childIndex] < values[childIndex+1])
				{
					childIndex++;
				}				
			}
			if(values[startIndex] < values[childIndex])
			{
				//swap 
				temp = values[startIndex];
				values[startIndex] = values[childIndex];
				values[childIndex] = temp;
				startIndex = childIndex;
				childIndex = startIndex << 1;
			}
			else
			{
				keepWorking = false;
			}
		}
	}
	
	public static void HeapSort(List<float> values)
	{	
		int array_size = values.Count;
		float temp;
		for (int i = (array_size / 2)-1; i >= 0; i--)
		{
			Heapify(values, i, array_size - 1);
		}
		for (int i = array_size-1; i >= 1; i--)
		{//delete the current binary heap root and re-hepify
			temp = values[0];
			values[0] = values[i];
			values[i] = temp;
			Heapify(values, 0, i-1);
		}
	}
	
	//heap is as follow: index 1 is the root; children are at 2*1 (left child) and 2*i+1 (right child), parent is at floor(i/2)
	//element 0 is kept as special having only one child at index 1 (this makes it possible the child / parent relation described above 
	//which is faster than the relation children at 2*1+1 and 2*i+2 that takes place when 0 is the root and has the first two children at index 1 and index 2 )
	//heapify means that each parent is guaranteed to be greater than all of its two children
	private static void Heapify(List<float> values, int startIndex, int maxHeapIndex)
	{
		int childIndex = startIndex << 1;
		bool keepWorking = true;
		float temp;
		while(childIndex <= maxHeapIndex && keepWorking)// startIndex * 2
		{
			if(childIndex < maxHeapIndex)
			{
				if(values[childIndex] < values[childIndex+1])
				{
					childIndex++;
				}				
			}
			if(values[startIndex] < values[childIndex])
			{
				//swap 
				temp = values[startIndex];
				values[startIndex] = values[childIndex];
				values[childIndex] = temp;
				startIndex = childIndex;
				childIndex = startIndex << 1;
			}
			else
			{
				keepWorking = false;
			}
		}
	}
}

