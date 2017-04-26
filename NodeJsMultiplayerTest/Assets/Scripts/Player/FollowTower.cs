using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTower : MonoBehaviour,IClickable {

        public GameObject myPlayer;
        private Target myPlayerTarget;
        private NetworkEntity networkEntity;    

        private void Start(){
            networkEntity = GetComponent<NetworkEntity>();
        }

        public void OnClick(RaycastHit hit)
        {
            myPlayer.GetComponent<Target>().targetTransform = gameObject.transform;
            myPlayer.GetComponent<Target>().targetName = gameObject.name;

            var networkCommunication = GetComponent<NetworkCommunication>();
            networkCommunication.SendTowerNumberToFollow(networkEntity.Id);
        }


    

}
