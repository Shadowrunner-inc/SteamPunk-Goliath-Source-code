using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using System.Security.Cryptography;
using System.Security;
using System.Text;


public class XMLManager : MonoBehaviour {

    public static XMLManager ins;
    //list of items
    public ItemDatabase ItemDB;
    //Vector 3 of Main Characters;
    public CharacterPosition CharacPosi;

   // public string sKey = Md5("Overambitious Assholes!!!!");

    private string persistentfilePath;
    private void Awake()
    {
        ins = this;

        persistentfilePath = Application.persistentDataPath + "/" + "XML" + "/";
        CreateDirectory();
    }

    /*
    public static string Md5(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }
   */


    //Create a folder on the user's local computer
    private void CreateDirectory()
    {
        DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath + "/" + "XML");
        if (!dirInf.Exists)  //Check if directory exists
        {
            Debug.Log("Creating Folder!");
            try
            {
                dirInf.Create();
            }
            catch (UnityException e)
            {
                Debug.Log(e);
            }
        }
        else
        {

        }
    }


    private void Start()
    {
      //  LoadItems();
        CreateDirectory();
    }

    //save function
    public void SaveItems() {
        //open a new xml file
        Debug.Log("Saving!");

        XmlSerializer serializer = new XmlSerializer(typeof(ItemDatabase));

       //datapath for contents, presistentdatapath for saves
        FileStream stream = new FileStream(Application.dataPath + "/StreamingAssets/XML/item_data.xml",FileMode.Create);
/* 
        DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
        DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
        DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

        ICryptoTransform desencrypt = DES.CreateEncryptor();

        using (CryptoStream cStream = new CryptoStream(stream, desencrypt, CryptoStreamMode.Write))
   */     
        serializer.Serialize(stream, ItemDB);
        
        stream.Close();

        Debug.Log("Save Finished");
    }

    //load function
    public void LoadItems()
    {
        try
        {
            //open a new xml file
            XmlSerializer serializer = new XmlSerializer(typeof(ItemDatabase));

            //datapath for contents, presistentdatapath for saves
            FileStream stream = new FileStream(Application.dataPath + "/StreamingAssets/XML/item_data.xml", FileMode.Open);
/*
            //Create Encryption stuff
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            ICryptoTransform desdecrypt = DES.CreateDecryptor();

            using (CryptoStream cStream = new CryptoStream(stream, desdecrypt, CryptoStreamMode.Read))
*/         
            ItemDB = serializer.Deserialize(stream) as ItemDatabase;
            
            stream.Close();
        }
        catch(System.Exception e)
        {
            Debug.Log(e);
        }
        
    }

    public void SavePosition()
    {
        //open a new xml file
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterPosition));

        //datapath for contents, presistentdatapath for saves
        FileStream stream = new FileStream(Application.persistentDataPath + "/XML/character_Position.xml", FileMode.Create);
        serializer.Serialize(stream,CharacPosi);
        stream.Close();
    }

    //load function
    public void LoadPosition()
    {
        //open a new xml file
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterPosition));

        //datapath for contents, presistentdatapath for saves
        FileStream stream = new FileStream(Application.persistentDataPath + "/XML/character_Position.xml", FileMode.Open);
        CharacPosi = serializer.Deserialize(stream) as CharacterPosition;
        stream.Close();
    }
}


//Items Management :

[System.Serializable]
public class ItemEntry {
    public string itemName;
    public int value;
	public Item_Material my_Item_Color;

}

public enum Item_Material
{
    Copper,
    Sliver,
    Gold,
}

[System.Serializable]
public class ItemDatabase {
    [XmlArray("Potions")]
    public List<ItemEntry> list = new List<ItemEntry>();
}

// Items Managment above;

// Character Position:
[System.Serializable]
public class CharacterPosition
{
    public Vector3 Wolf_Position;
    public Vector3 Sondra_Position;

}

[System.Serializable]
public class PlayerPosition {
    [XmlArray("CharacterPostion")]
    public List<CharacterPosition> list = new List<CharacterPosition>(); 
}
//Character Position above

