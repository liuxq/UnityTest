/*
 * CREATED:     2015-1-5 18:46:35
 * PURPOSE:     Basic geometry for 3D
 * AUTHOR:      Wangrui
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace UEEngine
{
    ///////////////////////////////////////////////////////////////////////////
    //	
    //	Class AABB
    //	
    ///////////////////////////////////////////////////////////////////////////

    public class AABB
    {
        public static float Epsilon = 1E-4f;
        private Vector3 mCenter;
        private Vector3 mExtents;
        private Vector3 mMins;
        private Vector3 mMaxs;

	    public Vector3 Center
        {
            get { return mCenter; }
            set { mCenter = value; }
        }

	    public Vector3 Extents
        {
            get { return mExtents; }
            set { mExtents = value; }
        }

	    public Vector3 Mins
        {
            get { return mMins; }
            set { mMins = value; }
        }

	    public Vector3 Maxs
        {
            get { return mMaxs; }
            set { mMaxs = value; }
        }

        public AABB() 
        {
            mMins = Vector3.zero;
            mMaxs = Vector3.zero;
        }

	    public AABB(AABB aabb)
        {
            Center = aabb.Center;
            Extents = aabb.Extents;
            Mins = aabb.Mins;
            Maxs = aabb.Maxs;
        }

	    public AABB(Vector3 mins, Vector3 maxs)
        {
            Mins = mins;
            Maxs = maxs;
            Center = 0.5f * (mins + maxs);
            Extents = maxs - Center;
        }

        //	Compute Mins and Maxs
	    public void CompleteMinsMaxs()
	    {
		    Mins = Center - Extents;
		    Maxs = Center + Extents;
	    }

        //	Compute Center and Extents
        public void CompleteCenterExts()
        {
            Center = (Mins + Maxs) * 0.5f;
            Extents = Maxs - Center;
        }

        //extend float epsilon
        public void Extend(float ext)
        {
            Vector3 vext = new Vector3(ext, ext, ext);
            Mins -= vext;
            Maxs += vext;
            CompleteCenterExts();
        }

        // Clear the aabb
	    public void Clear()
	    {
            Mins = new Vector3(999999.0f, 999999.0f, 999999.0f);
            Maxs = new Vector3(-999999.0f, -999999.0f, -999999.0f);
		    //Mins.Set(999999.0f, 999999.0f, 999999.0f);
		    //Maxs.Set(-999999.0f, -999999.0f, -999999.0f);
	    }

        //	Add a vertex to aabb
	    public void AddVertex(Vector3 v)
        {
            mMins = Vector3.Min(mMins, v);
            mMaxs = Vector3.Max(mMaxs, v);
        }

	    //	Merge two aabb
	    public void Merge(AABB subaabb)
        {
            mMins = Vector3.Min(mMins, subaabb.Mins);
            mMaxs = Vector3.Max(mMaxs, subaabb.Maxs);
            CompleteCenterExts();
        }

        public void Merge(Vector3 mins, Vector3 maxs)
        {
            mMins = Vector3.Min(mMins, mins);
            mMaxs = Vector3.Max(mMaxs, maxs);
            CompleteCenterExts();
        }

	    //	Check whether a point is in this aabb
	    public bool IsPointIn(Vector3 v)
	    {
		    if (v.x > Maxs.x || v.x < Mins.x ||
			    v.y > Maxs.y || v.y < Mins.y ||
			    v.z > Maxs.z || v.z < Mins.z)
            {
			    return false;
            }

		    return true;
	    }

	    //	Check whether another aabb is in this aabb
	    public bool IsAABBIn(AABB aabb)
        {
            if (aabb == null)
                return false;

            Vector3 delta = aabb.Center - Center;

            delta.x = Mathf.Abs(delta.x);
            if (delta.x + aabb.Extents.x > aabb.Extents.x)
                return false;

            delta.y = Mathf.Abs(delta.y);
            if (delta.y + aabb.Extents.y > aabb.Extents.y)
                return false;

            delta.z = Mathf.Abs(delta.z);
            if (delta.z + aabb.Extents.z > aabb.Extents.z)
                return false;

            return true;
        }

	    //	Build AABB from vertices
	    public void Build(Vector3[] lstVerPos)
        {
            Clear();
            for (int i = 0; i < lstVerPos.Length; i++)
            {
                mMins = Vector3.Min(mMins, lstVerPos[i]);
                mMaxs = Vector3.Max(mMaxs, lstVerPos[i]);
            }
            CompleteCenterExts();
        }


        public static void ExpandAABB(ref Vector3 mins, ref Vector3 maxs, AABB subaabb)
        {
            mins = Vector3.Min(mins, subaabb.Mins);
            maxs = Vector3.Max(maxs, subaabb.Maxs);
        }
    }
}
