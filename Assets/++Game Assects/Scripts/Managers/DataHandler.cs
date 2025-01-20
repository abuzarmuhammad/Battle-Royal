using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Random = UnityEngine.Random;

public class DataHandler : SingeltonBase<DataHandler>
{
	[SerializeField] private string Name;
	public Data IngameData;
	private BinaryFormatter formatter = new BinaryFormatter();
	private FileStream _stream;
	private string path;

	[Space(2)]
	public int maxPlayers;

	bool Data_avaliable;
	public override void Awake()
	{
		base.Awake();
		path = Application.persistentDataPath + "/" + Name + ".data";
		LoadData();
	}

	public void TryToLoadData(string _dataName)
	{
		Name = _dataName;
		path = Application.persistentDataPath + "/" + Name + ".data";
		LoadData();
	}
	
	private void LoadData()
	{
		try
		{
			if (File.Exists(path))
			{
				_stream = new FileStream(path, FileMode.Open);
				IngameData = (Data)formatter.Deserialize(_stream);
				_stream.Close();
                Data_avaliable = true;

            }
			else
			{
				IngameData = new Data();
                Data_avaliable = false; 
            }
			
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
	}

	public bool isFileExits()
	{
		return Data_avaliable;

    }
	
	public void SaveData()
	{ 
		try
		{
			_stream = new FileStream(path, FileMode.Create);
			formatter.Serialize(_stream,IngameData);
			_stream.Close();
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
	}

	// private void OnApplicationPause(bool pauseStatus)
	// {
	// 	SaveData();
	// }
	//
	private void OnApplicationQuit()
	{
		SaveData();
	}

}

[System.Serializable]
public class Data
{
	public Dictionary<string, string> PlayerAttributesData;
	public SettingData Setting;
	public Data()
	{
		PlayerAttributesData = new Dictionary<string, string>();

	}
}


