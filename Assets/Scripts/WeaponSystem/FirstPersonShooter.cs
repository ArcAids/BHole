using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    public enum WeaponType
    {
        primary, secondary, melee
    }

    public class FirstPersonShooter : MonoBehaviour
    {
        [SerializeField] TMPro.TMP_Text gunName;
        [SerializeField]
        protected Transform gunHolder;
        [SerializeField]
        protected Transform aimPosition;
        [SerializeField]
        protected bool followCursor = false;
        [SerializeField]
        bool isMeleeEquippable=false;       //no melee for now
        
        [SerializeField]
        bool canSwitchSecondary=false;      //if false secondary can not be dropped or switched, once equipped.

        [SerializeField]
        WeaponBehaviour[] weapons;      //list of weapons, set the number of weapon slots from inspector. not really need to give them references


        int primaryWeaponIndex=-1;      //indexes of weapons
        int secondaryWeaponIndex=-1;
        int currentEquippedWeaponIndex = -1;

        protected IAttack shootable;
        protected IAttack melee;
        protected IReloadable reloadable;
        protected IAimable aimable;

        [SerializeField]
        protected CinemachineVirtualCamera cam;
        protected RaycastHit hit;

        // Start is called before the first frame update
        protected void Start()
        {
            if (cam == null)
                cam = GetComponentInChildren<CinemachineVirtualCamera>();

            int i = 0;
            foreach (var weapon in weapons)     //go throught all weapons in list at start
            {
                if (weapon == null)     //only is its not null
                {
                    //if (primaryWeaponIndex < 0)
                    //    primaryWeaponIndex = i;
                    //else if (secondaryWeaponIndex < 0)
                    //    secondaryWeaponIndex = i;
                    i++;
                    continue;
                }
                if (primaryWeaponIndex < 0 && weapon.type == WeaponType.primary)        //if first primary, update primaryIndex
                    primaryWeaponIndex = i;
                if (weapon.type == WeaponType.secondary)        // if secondary
                {
                    if (secondaryWeaponIndex < 0)               //is not assigned yet
                        secondaryWeaponIndex = i;               //set it
                    else                                        //else drop other secondaries, because there can only be one.
                    {
                        //if (primaryWeaponIndex < 0)
                        //    primaryWeaponIndex = i;
                        DropWeapon(i);                          
                        i++;
                        continue;
                    }
                }

                weapon.Init(cam);       //must init weapons
                weapon.Pick();          //weapon is already picked at start
                Dequip(weapon);         //dequip temporarily
                if (weapon is MeleeBehaviour)       //meh
                    melee = weapon.GetComponent<IAttack>();

                i++;        //next.
            }
            currentEquippedWeaponIndex = -1;

            //equip something at start.
            if(primaryWeaponIndex>=0)
                EquipWeapon(primaryWeaponIndex);
            else if(secondaryWeaponIndex>=0)
                EquipWeapon(secondaryWeaponIndex);

        }
        /// <summary>
        /// picks a weapon.
        /// </summary>
        /// <param name="weapon">weapon that is being picked</param>
        public void PickUpWeapon(WeaponBehaviour weapon)
        {
            if (weapon.type == WeaponType.secondary && !canSwitchSecondary && secondaryWeaponIndex >=0) //if theres a secondary already in slots, return.
                return;

            int emptySlot = isThereEmptySlot();

            weapon.Pick();
            weapon.Init(cam);
            
            if (emptySlot >= 0)     //if theres empty slot, add weapon in the slot and return.
            {
                weapons[emptySlot] = weapon;
                    EquipWeapon(emptySlot);
                //if (currentEquippedWeaponIndex < 0)     //if there is no weapon or player picked a primary weapon, then equip it.
                //else
                //    Dequip(weapon);         //or just leave it in other slot.
                //Debug.Log("Dropped " + weapon.name);
                return;
            }

            if (weapon.type == WeaponType.primary)
            {
                DropWeapon(primaryWeaponIndex);
                weapons[primaryWeaponIndex] = weapon;
                EquipWeapon(primaryWeaponIndex);
            }

            if (weapon.type == WeaponType.secondary)        //if secondary weapon is being picked for the first time, drop current primary weapon.
            {
                DropWeapon(currentEquippedWeaponIndex);     
                weapons[currentEquippedWeaponIndex] = weapon;
                secondaryWeaponIndex = currentEquippedWeaponIndex;
                EquipWeapon(secondaryWeaponIndex);
                
            }

            if (weapon is MeleeBehaviour)
            {
                //TODO
                melee = weapon.GetComponent<IAttack>();
            }

        }

        /// <summary>
        /// don't call directly. pass index instead.
        /// </summary>
        /// <param name="weapon"></param>
        void DropWeapon(WeaponBehaviour weapon)
        {
            Debug.Log("dropping: " + weapon.name);
            Dequip(weapon);
            weapon.gameObject.transform.position = transform.position + transform.forward*2;
            weapon.Drop();
        }

        /// <summary>
        /// drops weapon of index index.
        /// </summary>
        /// <param name="index"></param>
        void DropWeapon(int index)
        {
            DropWeapon(weapons[index]);
            if (index == currentEquippedWeaponIndex)    //if it was equipped weapon, reset values.
            {
                aimable = null;
                shootable = null;
                reloadable = null;
            }

            weapons[index] = null;
            if (index == currentEquippedWeaponIndex)        //equip something else instead.
            {
                if (secondaryWeaponIndex >= 0)
                    EquipWeapon(secondaryWeaponIndex);
                if (primaryWeaponIndex >= 0)
                    EquipWeapon(primaryWeaponIndex);
            }
        }

        /// <summary>
        /// equips weapon of index index and dequips current equipped weapon.
        /// </summary>
        /// <param name="index"></param>
        protected virtual void EquipWeapon(int index)
        {
            if (index < 0 || index >= weapons.Length || index == currentEquippedWeaponIndex)
                return;

            if(EquipWeapon(weapons[index]))
            {
                currentEquippedWeaponIndex = index;
                switch (weapons[index].type)
                {
                    case WeaponType.primary:
                        primaryWeaponIndex = index;
                        break;
                    case WeaponType.secondary:
                        if (canSwitchSecondary || secondaryWeaponIndex<0) secondaryWeaponIndex = index;
                        break;
                    case WeaponType.melee:
                        melee = weapons[index].GetComponent<IAttack>();
                        break;
                    default:
                        break;
                }

            }

        }

        /// <summary>
        /// don't call directly. pass index instead.
        /// </summary>
        /// <param name="weapon">weapons to be equipped</param>
        /// <returns></returns>
        protected bool EquipWeapon(WeaponBehaviour weapon)
        {
            if (weapon == null || (weapon is MeleeBehaviour && !isMeleeEquippable))
                return false;

            if (currentEquippedWeaponIndex >= 0)
                Dequip(weapons[currentEquippedWeaponIndex]);

            if (weapon is WeaponBehaviour)     //place it in right position
            {
                weapon.gameObject.transform.parent = gunHolder;
                weapon.transform.localPosition = Vector3.zero;
                weapon.transform.localRotation = Quaternion.identity;
                gunName.text = weapon.name;
            }

            Debug.Log("EquippingWeapon: "+ weapon.name);
            weapon.Equip();

            //set references for current gun.

            reloadable = weapon.GetComponent<IReloadable>();
            if (reloadable != null)
                shootable = reloadable;
            else
                shootable = weapon.GetComponent<IAttack>();
            aimable = weapon.GetComponent<IAimable>();
            return true;
        }

        /// <summary>
        /// dequips the weapon to switch weapon 
        /// </summary>
        /// <param name="weapon"></param>
        protected void Dequip(WeaponBehaviour weapon)
        {
            Debug.Log("Dequiping " + weapon.name);
            if (weapon is WeaponBehaviour)             //leave it on the ground if not equipped.
            {
                weapon.Dequip();
            }
            if(isWeaponEquipped(weapon))
                currentEquippedWeaponIndex = -1;
        }

        /// <summary>
        /// points gun to target
        /// </summary>
        protected virtual void PointGunAtTarget()
        {
            Ray ray;
            if (followCursor)
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            else
                ray = new Ray(cam.transform.position + cam.transform.forward, cam.transform.forward);

            Vector3 gunPointDirection;
            Quaternion targetRotation;
            if (Physics.Raycast(ray, out hit, 500))
            {
                //gunHolder.LookAt(hit.point,Vector3.up);
                gunPointDirection = (hit.point - gunHolder.position).normalized;
                targetRotation = Quaternion.LookRotation(gunPointDirection, Vector3.up);
            }
            else
                targetRotation = Quaternion.LookRotation(cam.transform.forward, Vector3.up);
            gunHolder.rotation = Quaternion.Lerp(gunHolder.rotation,targetRotation , Time.deltaTime * 6);
        }

        /// <summary>
        /// call in every frame of update
        /// </summary>
        public void UpdateControls()
        {

            if (currentEquippedWeaponIndex < 0)
                return;

            if (Input.GetButton("Fire2"))
            {
                gunHolder.localRotation = Quaternion.identity;
                if (currentEquippedWeaponIndex >= 0)
                {
                    weapons[currentEquippedWeaponIndex].transform.parent = aimPosition;
                    weapons[currentEquippedWeaponIndex].transform.localRotation = Quaternion.identity;
                }
                aimable?.Aim();
            }
            else
            {
                if (cam != null)
                    PointGunAtTarget();
                if (currentEquippedWeaponIndex >= 0)
                    weapons[currentEquippedWeaponIndex].transform.parent = gunHolder;
                aimable?.StopAiming();
            }


            if (Input.GetButton("Fire1"))
                shootable?.Attack();

            weapons[currentEquippedWeaponIndex].transform.localPosition = Vector3.Lerp(weapons[currentEquippedWeaponIndex].transform.localPosition,Vector3.zero,Time.deltaTime *10);
            //if (Input.GetKey(KeyCode.Space))
            //    melee?.Attack();
            if (Input.GetKey(KeyCode.R))
                reloadable?.Reload();

            if (weapons.Length == 0)
                return;
            if (Input.GetKey(KeyCode.Alpha1))
                EquipWeapon(0);
            if (Input.GetKey(KeyCode.Alpha2))
                EquipWeapon(1);
            if (Input.GetKey(KeyCode.Alpha3))
                EquipWeapon(2);
            if (Input.GetKey(KeyCode.Alpha4))
                EquipWeapon(3);
        }

        /// <summary>
        /// is there any empty slot?
        /// </summary>
        /// <returns>returns -1 if no and, slot index if yes</returns>
        int isThereEmptySlot()
        {
            int i = 0;
            foreach (var item in weapons)
            {
                if (item == null)
                    return i;
                i++;
            }
            return -1;
        }

        bool isWeaponEquipped(WeaponBehaviour weapon)
        {
            if (currentEquippedWeaponIndex < 0)
                return false;
            return weapons[currentEquippedWeaponIndex] == weapon;
        }

        private void Update()
        {
            UpdateControls();
        }

        private void OnTriggerEnter(Collider other)
        {
            WeaponBehaviour weapon = other.GetComponent<WeaponBehaviour>();
            if (weapon != null)
                PickUpWeapon(weapon);
        }
    }
}