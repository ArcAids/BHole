using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class CacheManager {

    public static void SaveData(TempClass data)
    {

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/UserVehicleList.txt", FileMode.Create);

        binaryFormatter.Serialize(stream, data);
        stream.Close();
    }

    public static TempClass LoadData()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        try
        {
            FileStream stream = new FileStream(Application.persistentDataPath + "/UserVehicleList.txt", FileMode.Open);
            TempClass data = binaryFormatter.Deserialize(stream) as TempClass;
            stream.Close();

            return data;
        }
        catch (System.Exception exe)
        {
            Debug.Log("Error while loading data: " + exe.ToString());
            return null;
        }
        
    }

    public static void CacheDump()
    {
        string[] fileNames=Directory.GetFiles(Application.persistentDataPath+"/","*", SearchOption.TopDirectoryOnly);
        Debug.Log("Attempting Cache Dump");
        foreach (var item in fileNames)
        {
            if (File.Exists(item))
            {
                Debug.Log("deleted items :");
                File.Delete(item);
            }

        }
        
    }
}
//[System.Serializable]
//public class CachedList
//{
//    public VehicleListData[] cachedData;

//    public CachedList(VehicleList list)
//    {
//        cachedData = new VehicleListData[list.vehicleListData.Count];
//        cachedData = list.vehicleListData.ToArray();
//    }
//}
//[System.Serializable]
//public class CachedSpecs
//{
//    public string[] cachedSpecs;

//    public CachedSpecs(VehicleSpecs specs)
//    {
        
//    }
//}
