using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [field: SerializeField] private SerializableDictionary<Resource, int> Resources {get; set;}
    [field: SerializeField] private ResourceCounts _resourceCounts {get; set;} 

    public int GetResourceCount(Resource type)
    {
        if(Resources.TryGetValue(type, out int currentCount))
        {
            return currentCount;
        }
        else
        {
            return 0;
        }
    }

    public void SetCounts(ResourceCounts rc) {
        _resourceCounts = rc;
    }

    public int AddResources(Resource type, int count)
    {
        if(_resourceCounts == null) {
            return -1;
        }
        if(Resources.TryGetValue(type, out int currentCount))
        {
            int total = Resources[type] + count;
            // Debug.Log("Total:");
            // Debug.Log(total);
            _resourceCounts.UpdateResourceCount(type, total);
            if (total >= 5)
            {
                // Debug.Log("Allowing Tool Update...");
                _resourceCounts.AllowToolUpdate(type);
            }
            return Resources[type] += count;
        }
        else
        {
            _resourceCounts.UpdateResourceCount(type, count);
            Resources.Add(type, count);
            return count;
        }       
    }

}
