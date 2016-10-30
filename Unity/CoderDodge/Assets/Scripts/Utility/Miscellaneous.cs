using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Utility
{
    public static class Miscellaneous
    {
        public enum AxisDirection
        {
            XAxis,
            YAxis,
            ZAxis
        }

        public static Vector3 VectorDivide(Vector3 dividend, Vector3 divisor)
        {
            return new Vector3(dividend.x / divisor.x, dividend.y / divisor.y,
                dividend.z / divisor.z);
        }

        public static Vector3 ClampVector3(Vector3 input, Vector3 min, Vector3 max)
        {
            float x = Mathf.Clamp(input.x, min.x, max.x);
            float y = Mathf.Clamp(input.y, min.y, max.y);
            float z = Mathf.Clamp(input.z, min.z, max.z);
            return new Vector3(x, y, z);
        }

        public static bool ContainSubstringAny(string s, string[] substrings)
        {
            bool isContain = false;
            foreach (string substring in substrings)
            {
                if (s.Contains(substring))
                {
                    isContain = true;
                    break;
                }
            }
            return isContain;
        }

        public static T GetComponentInChildrenWithNames<T>(this Transform parent, string[] identifiers)
            where T : class
        {
            if (ContainSubstringAny(parent.name.ToLower(), identifiers))
            {
                T component = parent.GetComponent<T>();
                if (component != null)
                {
                    return component;
                }
            }
            foreach (Transform child in parent)
            {
                T component = child.GetComponentInChildrenWithNames<T>(identifiers);
                if (component != null)
                {
                    return component;
                }
            }
            return null;
        }

        public static Transform FindTransformWithNames(Transform parent, string[] identifiers)
        {
            if (ContainSubstringAny(parent.name.ToLower(), identifiers))
            {
                return parent;
            }
            foreach (Transform child in parent)
            {
                Transform target = FindTransformWithNames(child, identifiers);
                if (target != null)
                {
                    return target;
                }
            }
            return null;
        }

        public static bool CheckIfAngleBetweenVectorTooBig(Vector3 a, Vector3 b, float angleMaxAllowed)
        {
            float angleBetweenVector = Vector3.Angle(a, b);
            if (angleBetweenVector > angleMaxAllowed)
            {
                return true;
            }
            return false;
        }

        public static bool CheckIfAngleBetweenVectorsTooBigAll(List<Vector3> list, Vector3 b, float angleMaxAllowed)
        {
            bool angleTooBig = true;
            int n = list.Count;
            for (int i = 0; i < n; i++)
            {
                Vector3 a = list[i];
                if (!CheckIfAngleBetweenVectorTooBig(a, b, angleMaxAllowed))
                {
                    angleTooBig = false;
                    break;
                }
            }
            return angleTooBig;
        }

        public static bool CheckIfAngleBetweenVectorsTooBigAny(List<Vector3> list, Vector3 b, float angleMaxAllowed)
        {
            bool angleTooBig = false;
            int n = list.Count;
            for (int i = 0; i < n; i++)
            {
                Vector3 a = list[i];
                if (CheckIfAngleBetweenVectorTooBig(a, b, angleMaxAllowed))
                {
                    angleTooBig = true;
                    break;
                }
            }
            return angleTooBig;
        }

        /// <summary>
        /// Remove intersection of set A and set B from set A and set B.
        /// </summary>
        /// <typeparam name="T">Type of the element in HashSet</typeparam>
        /// <param name="a">Set A</param>
        /// <param name="b">Set B</param>
        /// <param name="aRemaining">Remaining part of A after the intersection
        /// is removed.</param>
        /// <param name="bRemaining">Remaining part of B after the intersection
        /// is removed.</param>
        public static void FindHashSetDifference<T>(HashSet<T> a, HashSet<T> b,
            HashSet<T> aRemaining, HashSet<T> bRemaining)
        {
            aRemaining.Clear();
            bRemaining.Clear();
            foreach (T element in a)
            {
                aRemaining.Add(element);
            }
            foreach (T element in b)
            {
                if (aRemaining.Contains(element))
                {
                    // This element is in both A and B. Remove it from aRemaining.
                    aRemaining.Remove(element);
                }
                else
                {
                    // This element is in B but not A, so put it into bRemaining.
                    bRemaining.Add(element);
                }
            }
        }

        public static void CopyFrom<T>(this HashSet<T> set, HashSet<T> setCopyFrom)
        {
            foreach (T e in setCopyFrom)
            {
                set.Add(e);
            }
        }

        public static void UnionWithCast<T1, T2>(this HashSet<T1> set1, HashSet<T2> set2)
            where T1: class where T2: class
        {
            foreach (T2 element in set2)
            {
                T1 elementCasted = element as T1;
                if (elementCasted != null)
                {
                    set1.Add(elementCasted);
                }
            }
        }  

        public static Vector3 GenerateRandomVector3(float min, float max)
        {
            float x = UnityEngine.Random.Range(min, max);
            float y = UnityEngine.Random.Range(min, max);
            float z = UnityEngine.Random.Range(min, max);
            return new Vector3(x, y, z);
        }

        public static void InvokeEvent(EventHandler eventToInvoke, object sender)
        {
            EventHandler handler = eventToInvoke;
            if (handler != null)
            {
                handler(sender, EventArgs.Empty);
            }
        }

        public static void InvokeEvent<TEventArgs>(
            EventHandler<TEventArgs> eventToInvoke, object sender,
            TEventArgs args) where TEventArgs : EventArgs
        {
            EventHandler<TEventArgs> handler = eventToInvoke;
            if (handler != null)
            {
                handler(sender, args);
            }
        }

        public static Vector3 Vector3Abs(Vector3 v)
        {
            Vector3 ret;
            ret.x = Mathf.Abs(v.x);
            ret.y = Mathf.Abs(v.y);
            ret.z = Mathf.Abs(v.z);
            return ret;
        }

        public static Vector3 InverseTransformPointInParentCoordinate(
            Transform trans, Vector3 vWorld)
        {
            Transform parent = trans.parent;
            if (parent == null)
            {
                return vWorld;
            }
            else
            {
                return parent.InverseTransformPoint(vWorld);
            }
        }

        public static Vector3 InverseTransformDirectionInParentCoordinate(
            Transform trans, Vector3 worldDirection)
        {
            Transform parent = trans.parent;
            if (parent == null)
            {
                return worldDirection;
            }
            else
            {
                return parent.InverseTransformDirection(worldDirection);
            }
        }

        public static Vector3 TransformPointFromLocalToParent(
            Transform transform, Vector3 localPoint)
        {
            Vector3 pointInParent = Vector3.Scale(transform.localRotation * localPoint,
            transform.localScale) + transform.localPosition;
            return pointInParent;
        }

        public static Vector3 TransformDirectionFromLocalToParent(
            Transform transform, Vector3 localDirection)
        {
            Vector3 directionInParent = transform.localRotation * localDirection;
            return directionInParent;
        }

        public static float Vector3MaxComponentAbs(Vector3 v)
        {
            float max = Mathf.Abs(v.x);
            float yAbs = Mathf.Abs(v.y);
            float zAbs = Mathf.Abs(v.z);
            if (yAbs > max)
            {
                max = yAbs;
            }
            if (zAbs > max)
            {
                max = zAbs;
            }
            return max;
        }

        public static void MoveParentTransformGivenChildTransform(
            Transform parent, Transform child, Vector3 childTargetPosition,
            Quaternion childTargetRotation, Vector3 parentPositionRelativeToChild,
            Quaternion parentRotationRelativeToChild)
        {
            parent.rotation = childTargetRotation * parentRotationRelativeToChild;
            child.rotation = childTargetRotation;
            parent.position = childTargetPosition +
                child.TransformPoint(parentPositionRelativeToChild) - child.position;
            child.position = childTargetPosition;
        }

        public static bool CheckNullAndLogError<T>(T obj)
        {
            if (obj == null)
            {
                Debug.LogErrorFormat("{0} is null", typeof(T));
                return true;
            }
            return false;
        }

        public static T GetComponentOnlyInChildren<T>(this Transform t)
            where T : class
        {
            T component = null;
            foreach (Transform child in t)
            {
                component = child.GetComponentInChildren<T>();
                if (component != null)
                {
                    break;
                }
            }
            return component;
        }

        public static IEnumerator CoroutineAction<T>(Action action) where T : YieldInstruction, new()
        {
            yield return new T();
            action();
        }

        public static IEnumerator DelayCoroutine(Action action, float delayTimeInSecond)
        {
            yield return new WaitForSeconds(delayTimeInSecond);
            action();
        }

        public static float GetVectorProjectionValueNormalized(Vector3 startPoint, 
            Vector3 endPoint, Vector3 point)
        {
            Vector3 movingVector = endPoint - startPoint;
            Vector3 direction = movingVector.normalized;
            float distance = movingVector.magnitude;
            float value = Mathf.Clamp01(Vector3.Dot(point - startPoint, direction) / distance);
            return value;
        }

        public static bool IsWithinRange(this float f, float min, float max)
        {
            return f >= min && f <= max;
        }

        public static Bounds GetRendererBoundsOfChildren(Transform parent)
        {
            Renderer[] renderers = parent.GetComponentsInChildren<Renderer>();
            bool init = false;
            Bounds totalBounds = new Bounds();
            int n = renderers.Length;
            for (int i = 0; i < n; i++)
            {
                Renderer renderer = renderers[i];
                if (!init)
                {
                    totalBounds = renderer.bounds;
                    init = true;
                }
                else
                {
                    totalBounds.Encapsulate(renderer.bounds);
                }
            }
            return totalBounds;
        }

        public static void ChangeSelfAndChildrenLayer(this Transform trans, int newLayer)
        {
            Transform[] targets = trans.GetComponentsInChildren<Transform>();
            int n = targets.Length;
            for (int i = 0; i < n; i ++)
            {
                Transform target = targets[i];
                target.gameObject.layer = newLayer;
            }
        }
    }
}
