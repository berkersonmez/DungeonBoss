using UnityEngine;
using System.Collections;
using System.Xml;

public class XMLController : MonoBehaviour {
	public static XMLController instance;
	
	public TextAsset namesXML;
	
	void Start () {
		instance = this;
	}
	
	public string[,] getRandomNicknames(int count) {
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(namesXML.text);
		XmlNodeList firstsList = xmlDoc.GetElementsByTagName("first");
		XmlNodeList lastsList = xmlDoc.GetElementsByTagName("last");
		string[,] names = new string[count, 2];
		for (int i = 0 ; i < count ; i++) {
			int randIndex = Random.Range(0, firstsList.Count);
			names[i, 0] = firstsList[randIndex].InnerText;
			randIndex = Random.Range(0, lastsList.Count);
			names[i, 1] = lastsList[randIndex].InnerText;
		}
		return names;
	}
}
