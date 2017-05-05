using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTower : MonoBehaviour,IClickable {

        public GameObject myPlayer;
        private Target myPlayerTarget;
        private NetworkEntity networkEntity;
        private CursorController myCourseController;
        private void Start(){
            networkEntity = GetComponent<NetworkEntity>();
            myCourseController = GetComponent<CursorController>();
        }

        public void OnClick(RaycastHit hit)
        {
            myCourseController.ChangeToMoveCursor();

            myPlayer.GetComponent<Target>().targetTransform = gameObject.transform;
            myPlayer.GetComponent<Target>().targetName = gameObject.name;

            var networkCommunication = GetComponent<NetworkCommunication>();
            networkCommunication.SendTowerNumberToFollow(networkEntity.Id);
        }


    

}
