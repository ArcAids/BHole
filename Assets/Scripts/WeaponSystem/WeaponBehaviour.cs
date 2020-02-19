using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    public abstract class WeaponBehaviour : MonoBehaviour, IEquipableAndDequipable, IPickable
    {
        [SerializeField]
        public WeaponType type;

        public bool Picked { get; protected set; }

        public abstract void Init(Cinemachine.CinemachineVirtualCamera cam);
        public abstract void Equip();
        public abstract void Dequip();

        public abstract void Pick();

        public abstract void Drop();
    }

    public interface IEquipableAndDequipable
    {
        void Equip();
        void Dequip();
    }
}