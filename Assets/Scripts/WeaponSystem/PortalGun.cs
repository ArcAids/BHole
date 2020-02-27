using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WeaponSystem
{
    public class PortalGun : WeaponBehaviour , IAimable , IAttack
    {
        [SerializeField]
        RectTransform gunUI;
        [SerializeField]
        float distanceMultiplier=1;
        [SerializeField]
        RecoilData recoilData;
        [SerializeField]
        CrossHair crossHair;
        [SerializeField]
        protected BulletBehaviour bulletPrefab;
        [SerializeField]
        protected Transform muzzleTransform;
        [SerializeField]
        protected float fireRate = 2;
        [SerializeField]
        float zoom = 20;
        [SerializeField] LayerMask layerMask;
        Recoil recoil;
        CinemachineVirtualCamera cam;
        Light flash;
        ParticleSystem smoke;
        Vector3 defaultPosition;
        protected float nextFire;
        protected bool isAiming = true;
        float fireDelay;

        public float FireRate { get => fireRate; private set { fireRate = value; fireDelay = 1f / value; } }

        public override void Init(CinemachineVirtualCamera cam)
        {
            this.cam = cam;
            FireRate = fireRate;
            flash = muzzleTransform.gameObject.GetComponent<Light>();
            smoke = muzzleTransform.gameObject.GetComponentInChildren<ParticleSystem>();
            crossHair.Hide();
            if (flash)
                flash.enabled = false;
            recoil = new Recoil(transform, recoilData);
            StopAiming();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public override void Equip()
        {
            defaultPosition = transform.localPosition;
            crossHair.Show();
            gameObject.SetActive(true);
            nextFire = 0;
            gunUI.localScale = Vector3.one;
        }

        public void Attack()
        {
            if (nextFire < Time.time)
            {
                StartCoroutine(MuzzleFlash());
                if (bulletPrefab != null)
                    ShootWormHole();
                recoil.StartRecoil();
                nextFire = Time.time + fireDelay;
            }
        }

        void ShootWormHole()
        {
            RaycastHit hit;
            if (Physics.Raycast(new Ray(muzzleTransform.position, muzzleTransform.forward), out hit, 500,layerMask))
            {
                if (hit.collider.GetComponent<Rigidbody>())
                    return;
                bulletPrefab.Init(0,0);
                bulletPrefab.transform.position = hit.point + hit.normal*distanceMultiplier;
                bulletPrefab.transform.forward = hit.normal;
            }
        }

        private void update()
        {
            recoil.Recoiling();
        }

        public void Aim()
        {
            if (isAiming)
                return;

            cam.m_Lens.FieldOfView = zoom;
            transform.position = cam.transform.position + cam.transform.forward - cam.transform.up;
            isAiming = true;
            crossHair?.Expand(0);
        }

        public void StopAiming()
        {
            if (!isAiming)
                return;

            transform.localPosition = defaultPosition;
            cam.m_Lens.FieldOfView = 60;

            isAiming = false;
            crossHair?.Expand(1);
        }

        IEnumerator MuzzleFlash()
        {
            if (flash != null)
            {
                smoke.Stop();
                flash.enabled = true;
                yield return new WaitForSeconds(0.1f);
                flash.enabled = false;
                smoke?.Play();
                yield return new WaitForSeconds(fireDelay);
            }
        }

        public override void Dequip()
        {
            transform.parent = null;
            gunUI.localScale = Vector3.one * 0.5f;
            gameObject.SetActive(false);
        }

        public override void Pick()
        {
            Picked = true;
            gunUI.gameObject.SetActive(true);
            crossHair.Show();
            GetComponent<Collider>().enabled = false;
        }

        public override void Drop()
        {
            Picked = false;
            GetComponent<Collider>().enabled = true;
            crossHair.Hide();
            flash.enabled = false;
            gameObject.SetActive(true);
        }
    }
}