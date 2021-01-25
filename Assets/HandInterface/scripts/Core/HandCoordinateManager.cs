using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HPUI.utils.Extensions;

namespace HPUI.Core
{
    public class HandCoordinateManager : MonoBehaviour
    {
        public Transform skeletonRoot;
        public Dictionary<string, string> proxyToSeletonNameMapping = new Dictionary<string, string>();
        
        
        public Transform getLinkedSkepetonTransform(string descendentName)
        {
            return skeletonRoot.FindDescendentTransform(descendentName);
        }

        public Transform getProxyTrasnform(string descendentName)
        {
            return this.transform.FindDescendentTransform(descendentName);
        }
    }
}
