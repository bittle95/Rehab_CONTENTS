/*
© Siemens AG, 2017-2018
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class OdometrySubscriber : Subscriber<Messages.Navigation.Odometry>
    {
        public Transform PublishedTransform;
	    public Vector3 tPosition;
	    public Vector3 tPosition_2;
        private Vector3 position;
	    private Vector3 twist;
        private Quaternion rotation;
        private bool isMessageReceived;

        protected override void Start()
	{
		base.Start();
	}

	private void Update()
	{
	    if (isMessageReceived){
		
		    ProcessMessage();
		    tPosition = position;
	    }
	}
	/*	
        private void Update()
        {
	    // added area for development
            //if (isMessageReceived){
		//tPosition = position;

               // ProcessMessage();	
		//tPosition_2 = twist;
	   // }

	    //tPosition = position;
	    //
        }
*/
        protected override void ReceiveMessage(Messages.Navigation.Odometry message)
        {
            position = GetPosition(message).Ros2Unity();
	    twist = GetTwist(message).Ros2Unity();
            rotation = GetRotation(message).Ros2Unity();
            isMessageReceived = true;
        }
        private void ProcessMessage()
        {
            PublishedTransform.position = position;
            PublishedTransform.rotation = rotation;
        }

        private Vector3 GetPosition(Messages.Navigation.Odometry message)
        {	

            return new Vector3(
                message.pose.pose.position.x,
                message.pose.pose.position.y,
                message.pose.pose.position.z);
        }
	private Vector3 GetTwist(Messages.Navigation.Odometry message)
        {	

            return new Vector3(
                message.twist.twist.linear.x,
                message.twist.twist.linear.y,
                message.twist.twist.angular.y);
        }

        private Quaternion GetRotation(Messages.Navigation.Odometry message)
        {
            return new Quaternion(
                message.pose.pose.orientation.x,
                message.pose.pose.orientation.y,
                message.pose.pose.orientation.z,
                message.pose.pose.orientation.w);
        }
    }
}
