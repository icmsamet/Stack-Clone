using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Block
{
    public class Block : MonoBehaviour
    {
        public enum Direction
        {
            Horizontal,
            Vertical
        }

        public Direction direction;
        [SerializeField] GameObject lastObj;

        private void Start()
        {
            transform.DOMoveZ(-transform.position.z, 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        }

        [ContextMenu("Divide")]
        public void DivideObj()
        {
            bool direction = transform.localPosition.z > 0;
            Vector3 fark = transform.position - lastObj.transform.position;

            GameObject fallDivide = GameObject.CreatePrimitive(PrimitiveType.Cube);
            fallDivide.name = "Fall";
            fallDivide.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fark.z);
            fallDivide.GetComponent<MeshRenderer>().material.color = Color.cyan;

            GameObject divided = GameObject.CreatePrimitive(PrimitiveType.Cube);
            divided.name = "Divided";
            divided.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, direction ? transform.localScale.z - fark.z : transform.localScale.z + fark.z);
            divided.GetComponent<MeshRenderer>().material.color = Color.red;

            if (direction)
            {
                if (fark.z > transform.localScale.z)
                {
                    Destroy(fallDivide);
                    Destroy(divided);
                    print("game over");
                }
                else
                {
                    fallDivide.transform.position = new Vector3(0, lastObj.transform.position.y, (GetObjectBoundSizes(lastObj).z / 2) + GetObjectBoundSizes(fallDivide).z / 2);
                    divided.transform.position = new Vector3(0, transform.position.y, (GetObjectBoundSizes(lastObj).z / 2) - GetObjectBoundSizes(divided).z / 2);
                    Destroy(this.gameObject);
                }
            }
            else
            {
                if (fark.z < (transform.localScale.z * -1))
                {
                    Destroy(fallDivide);
                    Destroy(divided);
                    print("game over");
                }
                else
                {
                    fallDivide.transform.position = new Vector3(0, lastObj.transform.position.y, ((GetObjectBoundSizes(lastObj).z / 2) + GetObjectBoundSizes(fallDivide).z / 2) * -1);
                    divided.transform.position = new Vector3(0, transform.position.y, ((GetObjectBoundSizes(lastObj).z / 2) - GetObjectBoundSizes(divided).z / 2) * -1);
                    Destroy(this.gameObject);
                }
            }
        }
        Vector3 GetObjectBoundSizes(GameObject obj)
        {
            var mesh = obj.GetComponent<MeshRenderer>();
            return mesh.bounds.size;
        }
    }
}
