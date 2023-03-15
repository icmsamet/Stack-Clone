using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;

namespace Block
{
    public class Block : MonoBehaviour
    {
        public bool isFirst = false;
        public enum Direction
        {
            Horizontal,
            Vertical
        }
        [HideIf("isFirst")] public Direction directionType;
        [HideIf("isFirst")] public GameObject lastObj;
        float moveTime;
        GameObject divided;
        bool isDivided = false;

        public void Move()
        {
            if (isFirst)
                return;
            switch (directionType)
            {
                case Direction.Horizontal:
                    transform.DOMoveX(-transform.position.x, moveTime).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                    break;
                case Direction.Vertical:
                    transform.DOMoveZ(-transform.position.z, moveTime).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                    break;
            }
        }
        private void Update()
        {
            if (isFirst)
                return;
            if (Input.GetMouseButtonDown(0) && !isDivided)
            {
                DivideObj();
                isDivided = true;
                transform.DOKill();
            }
        }
        public void DivideObj()
        {
            float hangOver = GetHangOver();

            float max = directionType == Direction.Vertical ? lastObj.transform.localScale.z : lastObj.transform.localScale.x;

            if (Mathf.Abs(hangOver) > max)
            {
                GameManager.GameManager.instance.FailGame();
                return;
            }
            else
            {
                float direction = hangOver > 0 ? 1f : -1f;
                SplitCube(hangOver, direction);
                divided = this.gameObject;

                GameManager.GameManager.instance.AddPosition();
                GameManager.GameManager.instance.SetLastObj(divided);
                GameManager.GameManager.instance.Spawn();
                GameManager.GameManager.instance.CutEffect();
                GameManager.GameManager.instance.AddScore();
            }
        }
        private float GetHangOver()
        {
            if (directionType == Direction.Vertical)
            {
                return transform.position.z - lastObj.transform.position.z;
            }
            else
            {
                return transform.position.x - lastObj.transform.position.x;
            }
        }
        private void SplitCube(float hangOver, float direction)
        {
            switch (directionType)
            {
                case Direction.Horizontal:
                    float newXsize = lastObj.transform.localScale.x - Mathf.Abs(hangOver);
                    float fallingBlockXSize = transform.localScale.x - newXsize;

                    float newXposition = lastObj.transform.position.x + (hangOver / 2);
                    transform.localScale = new Vector3(newXsize, transform.localScale.y, transform.localScale.z);
                    transform.position = new Vector3(newXposition, transform.position.y, transform.position.z);

                    float cubeEdgeX = transform.position.x + (newXsize / 2f * direction);
                    float fallingBlockxPosition = cubeEdgeX + fallingBlockXSize / 2f * direction;

                    SpawnDropCube(fallingBlockxPosition, fallingBlockXSize);
                    break;
                case Direction.Vertical:
                    float newZsize = lastObj.transform.localScale.z - Mathf.Abs(hangOver);
                    float fallingBlockZSize = transform.localScale.z - newZsize;

                    float newZposition = lastObj.transform.position.z + (hangOver / 2);
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZsize);
                    transform.position = new Vector3(transform.position.x, transform.position.y, newZposition);

                    float cubeEdgeZ = transform.position.z + (newZsize / 2f * direction);
                    float fallingBlockZPosition = cubeEdgeZ + fallingBlockZSize / 2f * direction;

                    SpawnDropCube(fallingBlockZPosition, fallingBlockZSize);
                    break;
            }
        }
        private void SpawnDropCube(float fallingBlockZPosition, float fallingBlockSize)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

            if (directionType == Direction.Vertical)
            {
                cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
                cube.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockZPosition);
            }
            else
            {
                cube.transform.localScale = new Vector3(fallingBlockSize, transform.localScale.y, transform.localScale.z);
                cube.transform.position = new Vector3(fallingBlockZPosition, transform.position.y, transform.position.z);
            }
            cube.AddComponent<Rigidbody>();
            cube.GetComponent<MeshRenderer>().material.color = GetComponent<MeshRenderer>().material.color;
            Destroy(cube, 2);
        }
        public void SetDirection(bool isHorizontal)
        {
            directionType = isHorizontal == true ? Direction.Horizontal : Direction.Vertical;
        }
        public void SetMoveTime(float time)
        {
            moveTime = time;
        }
        public void SetColor(Color color)
        {
            GetComponent<MeshRenderer>().material.color = color;
        }
        public void SetLastObj(GameObject _lastObj)
        {
            lastObj = _lastObj;
        }
        private void OnBecameInvisible()
        {
            gameObject.SetActive(false);
        }
    }
}