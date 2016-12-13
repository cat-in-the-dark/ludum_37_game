using UnityEngine;

public class Cube {
    public GameObject GameObj;
    public Vector3 LocalPos;

    public Cube(GameObject gameObj, Vector3 localPos) {
        this.GameObj = gameObj;
        this.LocalPos = localPos;
    }
}