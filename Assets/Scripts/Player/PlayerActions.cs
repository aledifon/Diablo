using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{    
    public bool CanInteract { get; set; }
    public InteractableObject InteractableObj { get; set; }  // Ref. to the Object the player is interacting with
    public Transform pivotWeapon;   // { get; private set; }

    void Update()
    {        
        if (CanInteract && InteractableObj != null && Input.GetKeyDown(InteractableObj.InteractKeyCode))
        {
            if (InteractableObj.equipable)
                InteractableObj.Equip();
            else
                InteractableObj.Interact();
        }                
        if(Input.GetKeyDown(KeyCode.F) && pivotWeapon.childCount > 0)
            DropWeapon();
    }
    private void DropWeapon()
    {
        GameObject weapon = InteractableObj.gameObject;
        weapon.transform.SetParent(null);

        Ray ray = new Ray();
        RaycastHit hit;

        // Set a raycast +1m Forward & +2m Upwards from the player's pos. & with Downards direction
        ray.origin = transform.position + transform.forward + transform.up*2;
        ray.direction = Vector3.down;

        if (Physics.Raycast(ray, out hit))
            weapon.transform.position = hit.point;
        else
            weapon.transform.position = transform.position + transform.forward;

        weapon.transform.rotation = Quaternion.identity;        
        InteractableObj.SetButtonAndCollider(true);
        Debug.DrawRay(ray.origin,ray.direction*10,Color.red);
    }
}
