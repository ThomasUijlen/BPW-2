using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// [CreateAssetMenu(fileName = "InventorySaver", menuName = "Items/InventorySaver", order = 100)]
// public class CharacterSaver : ScriptableObject
// {
//     public GameObject[] characterPrefabs;
//     public bool reset = false;

//     List<CharacterData> savedCharacterData = new List<CharacterData>();

//     private void OnEnable() {
//         if(reset) savedCharacterData.Clear();
//     }

//     public void SaveCharacter(CharacterDetails character, bool delete) {
//         if(!delete) {
//             string characterPath = GetGameObjectPath(character.gameObject);
//             savedCharacterData.Add(new CharacterData(characterPath,character.name));
//         } else {
//             string characterPath = GetGameObjectPath(character.gameObject);
//             if(ContainsKey(characterPath)) savedCharacterData.Remove(GetcharacterData(characterPath));
//         }
//     }

//     public GameObject GetSavedItem(CharacterDetails character) {
//         string characterPath = GetGameObjectPath(character.gameObject);
//         CharacterData characterData = GetcharacterData(characterPath);
//         if(characterData == null) return null;

//         foreach(GameObject item in characterPrefabs) {
//             if(item.GetComponent<Item>().itemName == characterData.characterName) return Instantiate(item);
//         }

//         return null;
//     }

//     private GameObject LoadPrefabFromFile(string filename)
//     {
//         var loadedObject = Resources.Load<GameObject>(filename);
//         return loadedObject;
//     }

//     private string GetGameObjectPath(GameObject obj)
//     {
//         string path = "/" + obj.name;
//         while (obj.transform.parent != null)
//         {
//             obj = obj.transform.parent.gameObject;
//             path = "/" + obj.name + path;
//         }
//         return path;
//     }

//     private bool ContainsKey(string key) {
//         foreach(CharacterData characterData in savedCharacterData) {
//             if(key == characterData.characterPath) return true;
//         }
//         return false;
//     }

//     private CharacterData GetcharacterData(string key) {
//         foreach(CharacterData characterData in savedCharacterData) {
//             if(key == characterData.characterPath) return characterData;
//         }
//         return null;
//     }
// }

// [System.Serializable]
// public class CharacterData {
//     public string characterPath;
//     public float health;
//     public float energy;
//     public Vector2 coord;
//     public bool dead = false;

//     public CharacterData(string characterPath, CharacterDetails characterDetails) {
//         this.characterPath = characterPath;
//         this.characterName = characterName;
//     }
// }