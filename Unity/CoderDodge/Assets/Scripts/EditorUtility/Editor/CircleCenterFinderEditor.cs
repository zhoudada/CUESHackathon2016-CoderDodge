using System;
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.Assertions;

[CustomEditor(typeof(CircleCenterFinder))]
public class CircleCenterFinderEditor : Editor {
    private CircleCenterFinder _script;

    void OnEnable()
    {
        _script = serializedObject.targetObject
            as CircleCenterFinder;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();

        if (_script == null)
        {
            return;
        }
        if (GUILayout.Button("Find Center"))
        {
            FindCenter(_script.transform, _script.PointA, _script.PointB, _script.PointC);
        }
        if (GUILayout.Button("Copy center to target"))
        {
            CopyWorldPositionToTransform(_script.transform.position, _script.WorldPositionCopyTo);
        }
        if (GUILayout.Button("Test FindMinimum"))
        {
            FindMinimumTest();
        }
        serializedObject.ApplyModifiedProperties();
    }

    private void FindCenter(Transform center, Transform pointATransform,
        Transform pointBTransform, Transform pointCTransform)
    {
        if (pointATransform == null || pointBTransform == null || pointCTransform == null)
        {
            return;
        }
        Vector3 pointA = pointATransform.position;
        Vector3 pointB = pointBTransform.position;
        Vector3 pointC = pointCTransform.position;
        Vector3 directionAB = pointB - pointA;
        Vector3 directionBC = pointC - pointB;

        float minDistAllowed = 1E-5f;
        if (directionAB.magnitude < minDistAllowed || directionBC.magnitude < minDistAllowed)
        {
            return;
        }
        directionAB = directionAB.normalized;
        directionBC = directionBC.normalized;
        Vector3 circleNormal = Vector3.Cross(directionAB, directionBC).normalized;
        Vector3 lineDirectionAB = Vector3.Cross(directionAB, circleNormal);
        Vector3 lineDirectionBC = Vector3.Cross(directionBC, circleNormal);
        Vector3 centerAB = (pointA + pointB) / 2;
        Vector3 centerBC = (pointB + pointC) / 2;
        Vector3 coeff1 = centerAB - centerBC;
        Vector3 coeff2 = lineDirectionAB;
        Vector3 coeff3 = -lineDirectionBC;
        Vector3[] coeff = new Vector3[] { coeff1, coeff2, coeff3 };
        float[] unknowns = FindMinimum(coeff, _script.ConvergeRate);
        Vector3 centerPosition = centerAB + unknowns[0] * lineDirectionAB;
        Undo.RecordObject(center, "Relocate center");
        center.position = centerPosition;
    }

    private void CopyWorldPositionToTransform(Vector3 worldPosition, Transform target)
    {
        if (target == null)
        {
            return;
        }
        Undo.RecordObject(target, "Copy world position to target");
        target.position = worldPosition;
    }

    void OnSceneGUI()
    {
        Transform pointA = _script.PointA;
        Transform pointB = _script.PointB;
        Transform pointC = _script.PointC;
        if (pointA == null || pointB == null || pointC == null)
        {
            return;
        }
        Undo.RecordObject(pointA, "Relocate PointA");
        Undo.RecordObject(pointB, "Relocate PointB");
        Undo.RecordObject(pointC, "Relocate PointC");
        pointA.position = Handles.PositionHandle(
            pointA.position, pointA.rotation);
        pointB.position = Handles.PositionHandle(
            pointB.position, pointB.rotation);
        pointC.position = Handles.PositionHandle(
            pointC.position, pointC.rotation);
    }

    private void FindMinimumTest()
    {
        Vector3[] coeff = new Vector3[] { new Vector3(1, 1, 3), new Vector3(1f, 1f, 2f) };
        float[] unknowns = FindMinimum(coeff);
        foreach (float x in unknowns)
        {
            Debug.Log(x);
        }
    }

    private static float ComputeCost(Vector3[] coeff, float[] unknowns)
    {
        Vector3 sum = Vector3.zero;
        int n = coeff.Length;
        Assert.AreEqual(n - 1, unknowns.Length);
        for (int i = 0; i < n; i++)
        {
            if (i == 0)
            {
                sum += coeff[i];
            }
            else
            {
                sum += unknowns[i - 1] * coeff[i];
            }
        }
        return sum.magnitude;
    }

    private static float ComputeDerivative(Vector3[] coeff,
        float[] unknownValuesOriginal, int unknownIndex, float increment = 1E-5f)
    {
        int n = unknownValuesOriginal.Length;
        float[] unknownValues = new float[n];
        Array.Copy(unknownValuesOriginal, unknownValues, n);
        float costOriginal = ComputeCost(coeff, unknownValues);
        unknownValues[unknownIndex] += increment;
        float costNew = ComputeCost(coeff, unknownValues);
        float deltaCost = costNew - costOriginal;
        return deltaCost / increment;
    }

    private static float[] FindMinimum(Vector3[] coeff,
        float convergeRate = 1f, float expectedMin = 0f, int maxIter = 10000, float tol = 1E-3f)
    {
        int n = coeff.Length - 1;
        if (n <= 0)
        {
            return null;
        }
        float[] unknowns = new float[n];
        for (int i = 0; i < n; i++)
        {
            unknowns[i] = 0;
        }
        float[] derivatives = new float[n];
        int iter = 0;
        float cost = ComputeCost(coeff, unknowns);
        float lastCost = cost;
        int adjustPeriod = 100;
        float varianceMax = 0.01f;
        float varianceMin = 1E-4f;
        float[] costHistory = new float[adjustPeriod];
        int adjustCountMax = 5;
        int adjustCount = 0;
        while (iter < maxIter && Mathf.Abs(cost - expectedMin) > tol)
        {
            costHistory[iter % adjustPeriod] = cost;
            // Compute partial derivatives for each unknown
            for (int i = 0; i < n; i++)
            {
                derivatives[i] = ComputeDerivative(coeff, unknowns, i);
            }
            // Compute next value of each unknown based on the derivative
            for (int i = 0; i < n; i++)
            {
                if (derivatives[i] >= 0 && derivatives[i] < float.Epsilon)
                {
                    derivatives[i] = float.Epsilon;
                }
                else if (derivatives[i] <= 0 && derivatives[i] > -float.Epsilon)
                {
                    derivatives[i] = -float.Epsilon;
                }
                unknowns[i] += -convergeRate * (cost - expectedMin) / derivatives[i];
            }
            cost = ComputeCost(coeff, unknowns);
            float costDiff = cost - lastCost;
            lastCost = cost;
            //if (costDiff > 0)
            //{
            //    adjustCount++;
            //    if (adjustCount >= adjustCountMax || costDiff > 20)
            //    {
            //        convergeRate /= 2;
            //        adjustCount = 0;
            //    }
            //}
            //else
            //{
            //    adjustCount = 0;
            //}
            iter++;
            //if (iter % adjustPeriod == 0)
            //{
            //    float variance = GetVariance(costHistory);
            //    if (variance > varianceMax)
            //    {
            //        convergeRate /= 2;
            //    }
            //    else if (variance < varianceMin && Mathf.Abs(cost - expectedMin) > 1)
            //    {
            //        convergeRate *= 2;
            //    }
            //}
        }
        if (iter == maxIter)
        {
            Debug.LogWarning("Max iteration is reached.");
        }
        Debug.LogFormat("Final cost: {0}", cost);
        return unknowns;
    }

    private static float GetAverage(float[] arr)
    {
        int n = arr.Length;
        if (arr.Length == 0)
        {
            return 0;
        }
        float sum = 0;
        for (int i = 0; i < n; i++)
        {
            sum += arr[i];
        }
        return sum / n;
    }

    private static float GetVariance(float[] arr)
    {
        int n = arr.Length;
        if (n == 0)
        {
            return 0;
        }
        float avg = GetAverage(arr);
        float variance = 0;
        for (int i = 0; i < n; i++)
        {
            float diff = arr[i] - avg;
            variance += diff * diff;
        }
        variance /= n;
        return variance;
    }
}
