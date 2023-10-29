using SuperFight;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSkinWeapon : MonoBehaviour
{
    public GameObject weaponHolderLeft;
    public GameObject weaponHolderRight;

    public void LoadWeapon(Weapon weapon)
    {
        Weapon currentWeapon;
        int childCount = weaponHolderLeft.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Destroy(weaponHolderLeft.transform.GetChild(i).gameObject);
        }

        int childCount2 = weaponHolderRight.transform.childCount;
        for (int i = childCount2 - 1; i >= 0; i--)
        {
            Destroy(weaponHolderRight.transform.GetChild(i).gameObject);
        }

        if (weapon == null) return;

        if (weapon.curHand == HAND.LEFT)
        {
            if (weapon != null)
            {
                currentWeapon = Instantiate(weapon, weaponHolderLeft.transform);
                currentWeapon.transform.localPosition = Vector3.zero;
                PlayerManager.Instance.PlayFXEquipWeapon();
                currentWeapon.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("UI");
                currentWeapon.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = currentWeapon.sortingLayer;
            }
        }
        else if (weapon.curHand == HAND.RIGHT)
        {
            if (weapon != null)
            {
                currentWeapon = Instantiate(weapon, weaponHolderRight.transform);
                currentWeapon.transform.localPosition = Vector3.zero;
                PlayerManager.Instance.PlayFXEquipWeapon();
                currentWeapon.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("UI");
                currentWeapon.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = currentWeapon.sortingLayer;
            }
        }
        else
        {
            if (weapon != null)
            {
                //left
                currentWeapon = Instantiate(weapon, weaponHolderLeft.transform);
                currentWeapon.transform.localPosition = Vector3.zero;
                currentWeapon.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("UI");
                currentWeapon.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = currentWeapon.sortingLayer;
                //right
                currentWeapon = Instantiate(weapon, weaponHolderRight.transform);
                currentWeapon.transform.localPosition = Vector3.zero;
                currentWeapon.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("UI");
                currentWeapon.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = currentWeapon.sortingLayer;
                PlayerManager.Instance.PlayFXEquipWeapon();
            }
        }

    }
}
