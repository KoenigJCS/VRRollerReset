using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartEntity : MonoBehaviour
{
    [SerializeField] private int carNumber = -1;
    public Transform targetTransform;
    // Start is called before the first frame update
    void Start()
    {
        CartMovement.inst.AddCart(this.gameObject, carNumber);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
