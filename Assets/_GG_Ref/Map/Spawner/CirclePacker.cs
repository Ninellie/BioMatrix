using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CirclePacker
{
    public List<CircleCollider2D> mCircles;
    public CircleCollider2D mDraggingCircle = null;
    protected Vector2 mPackingCenter;
    public float mMinSeparation = 1f;

    /// <summary>
    /// Generates a number of Packing circles in the constructor.
    /// Random distribution is linear
    /// </summary>
    public CirclePacker(Vector2 pPackingCenter, List<CircleCollider2D> mCircles)
    {
        this.mPackingCenter = pPackingCenter;
        this.mCircles = mCircles;
    }
    public CirclePacker(Vector2 pPackingCenter, int pNumCircles, double pMinRadius, double pMaxRadius)
    {
        this.mPackingCenter = pPackingCenter;

        // Create random circles
        this.mCircles.Clear();
        System.Random Rnd = new System.Random(System.DateTime.Now.Millisecond);
        for (int i = 0; i < pNumCircles; i++)
        {
            Vector2 nCenter = new Vector2((float)(this.mPackingCenter.x + Rnd.NextDouble() * pMinRadius),
                                          (float)(this.mPackingCenter.y + Rnd.NextDouble() * pMinRadius));

            float nRadius = (float)(pMinRadius + Rnd.NextDouble() * (pMaxRadius - pMinRadius));

            var addedCircleCollider2D = new CircleCollider2D();
            addedCircleCollider2D.radius = nRadius;
            addedCircleCollider2D.transform.position = nCenter;
            this.mCircles.Add(addedCircleCollider2D);
        }
    }
    public void DoPack()
    {
        float pMinRadius = 1000f;
        float pMaxRadius = 0;
        for (int i = 0; i < mCircles.Count; i++)
        {
            if(mCircles[i].radius < pMinRadius)
            {
                pMinRadius = mCircles[i].radius;
            }
        }
        for (int i = 0; i < mCircles.Count; i++)
        {
            if (mCircles[i].radius > pMaxRadius)
            {
                pMaxRadius = mCircles[i].radius;
            }
        }
        System.Random Rnd = new System.Random(System.DateTime.Now.Millisecond);
        for (int i = 0; i < mCircles.Count; i++)
        {
            Vector2 nCenter = new Vector2((float)(this.mPackingCenter.x + Rnd.NextDouble() * pMinRadius),
                                          (float)(this.mPackingCenter.y + Rnd.NextDouble() * pMinRadius));

            float nRadius = (float)(pMinRadius + Rnd.NextDouble() * (pMaxRadius - pMinRadius));

            this.mCircles[i].radius = nRadius;
            this.mCircles[i].transform.position = nCenter;
        }
    }
    /// <summary>
    ///
    /// </summary>
    /// <param name="?"></param>
    /// <returns></returns>
    private float DistanceToCenterSq(CircleCollider2D pCircle)
    {
        return ((Vector2)pCircle.transform.position - mPackingCenter).sqrMagnitude;
    }
    /// <summary>
    ///
    /// </summary>
    private int Comparer(CircleCollider2D p1, CircleCollider2D p2)
    {
        float d1 = DistanceToCenterSq(p1);
        float d2 = DistanceToCenterSq(p2);
        if (d1 < d2)
            return 1;
        else if (d1 > d2)
            return -1;
        else return 0;
    }
    public void OnFrameMove(long iteratiodnCounter)
    {
        // Sort circles based on the distance to center
        mCircles.Sort(Comparer);

        float minSeparationSq = mMinSeparation * mMinSeparation;
        for (int i = 0; i < mCircles.Count - 1; i++)
        {
            for (int j = i + 1; j < mCircles.Count; j++)
            {
                if (i == j)
                    continue;

                Vector2 AB = mCircles[j].transform.position - mCircles[i].transform.position;
                float r = mCircles[i].radius + mCircles[j].radius;

                // Length squared = (dx * dx) + (dy * dy);
                float d = AB.sqrMagnitude - minSeparationSq;
                float minSepSq = Math.Min(d, minSeparationSq);
                d -= minSepSq;

                if (d < (r * r) - 0.01)
                {
                    AB.Normalize();

                    AB *= (float)((r - Math.Sqrt(d)) * 0.5f);

                    if (mCircles[j] != mDraggingCircle)
                    {
                        Vector2 vector2 = mCircles[j].transform.position + (Vector3)AB;
                        mCircles[j].transform.position = vector2;
                        //mCircles[j].transform.position.Set(mCircles[j].transform.position.x + AB.x,
                        //                                   mCircles[j].transform.position.y + AB.y,
                        //                                   mCircles[j].transform.position.z);
                    }

                    if (mCircles[i] != mDraggingCircle)
                    {
                        Vector2 vector2 = mCircles[j].transform.position - (Vector3)AB;
                        mCircles[j].transform.position = vector2;
                        //mCircles[j].transform.position.Set(mCircles[j].transform.position.x - AB.x,
                        //                                   mCircles[j].transform.position.y - AB.y,
                        //                                   mCircles[j].transform.position.z);
                    }
                }
            }
        }


        float damping = 0.1f / (float)(iteratiodnCounter);
        for (int i = 0; i < mCircles.Count; i++)
        {
            if (mCircles[i] != mDraggingCircle)
            {
                Vector2 v = (Vector2)mCircles[i].transform.position - this.mPackingCenter;
                v *= damping;
                mCircles[i].transform.position.Set(mCircles[i].transform.position.x - v.x,
                    mCircles[i].transform.position.y - v.y,
                    mCircles[i].transform.position.z);
            }
        }
    }
}