using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class singleShotGun : gun
{
    [SerializeField] private Camera cam;

    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    public override void use()
    {
        shoot();
    }

    void shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3 (0.5f, 0.5f));
        ray.origin = cam.transform.position;

        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            hit.collider.gameObject.GetComponent<iDamagable>()?.takeDamage(((gunInfo)itemInfo).damage);
            PV.RPC("rpc_Shoot", RpcTarget.All, hit.point, hit.normal);
        }
    }

    [PunRPC]
    void rpc_Shoot(Vector3 hitPosition, Vector3 hitNormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
        if(colliders.Length > 0)
        {
            GameObject bulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal * 0.01f, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
            Destroy(bulletImpactObj, 5);
            bulletImpactObj.transform.SetParent(colliders[0].transform);
        }

    }
}
