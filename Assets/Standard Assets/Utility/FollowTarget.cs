using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class FollowTarget : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0f, 7.5f, 0f);
		public bool is_rotate_follow = false;


        private void LateUpdate()
        {
            transform.position = target.position + offset;
			if(is_rotate_follow){
				transform.rotation = target.rotation;
			}
        }
    }
}
