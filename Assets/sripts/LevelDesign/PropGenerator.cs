using ChobiAssets.PTM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Properties;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

public class PropGenerator : MonoBehaviour
{
    public List<Prop> propsPoints;
    [SerializeField] private List<PropLimit> propsLimit;

    private List<int> GenerateRandomNumbers(int limit, int count)
    {
        if (count > limit)
        {
            Debug.Log("Количество чисел не может быть больше лимита.");
            return null;
        }

        System.Random rand = new System.Random();
        HashSet<int> numbers = new HashSet<int>();

        // Генерируем уникальные случайные числа
        while (numbers.Count < count)
        {
            int num = rand.Next(1, limit + 1);
            numbers.Add(num);
        }

        return new List<int>(numbers);
    }

    void Start()
    {
        var props = Resources.Load<PropCollections>("Props").props;
        var dictionary = new Dictionary<PropType, List<Prop>>();

        foreach (var propPoint in propsPoints)
        {
            if (dictionary.ContainsKey(propPoint.type))
            {
                dictionary[propPoint.type].Add(propPoint);
            }
            else
            {
                dictionary.Add(propPoint.type, new List<Prop>());
                dictionary[propPoint.type].Add(propPoint);
            }
        }
        foreach (var propPoint in dictionary)
        {
            var limit = propsLimit.FirstOrDefault(t => t.type == propPoint.Key);
            var elements = props.Where(t => t.type == propPoint.Key).ToList();

            if (limit != null)
            {
                var pointsID = GenerateRandomNumbers(propPoint.Value.Count, limit.propLimt);
                Debug.Log("Poped limitt" + limit.propLimt);

                if(pointsID == null)
                {
                    continue;
                }

                for (int i = 0; i < limit.propLimt; i++)
                {
                    var point = propPoint.Value[pointsID[i]];
                    Instantiate(elements[UnityEngine.Random.Range(0, elements.Count())].prefab, point.prefab.transform.position, Quaternion.identity);
                }
                
            }
            else
            {
                foreach(var point in propPoint.Value)
                {
                    Instantiate(elements[UnityEngine.Random.Range(0, elements.Count())].prefab, point.prefab.transform.position, Quaternion.identity);
                }
            }
        }
    }
}

[Serializable]
public class PropLimit
{
    public PropType type;
    public int propLimt;
}

[CustomEditor(typeof(PropGenerator))]
public class PropGenEditor: Editor
{
    private PropGenerator _propGenerator;
    private void OnEnable()
    {
        _propGenerator = Selection.activeGameObject.GetComponent<PropGenerator>();
    }
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Generate Prop Point", GUILayout.Width(200)))
        {
            GeneratePropPoints();
        }
    }

    private void GeneratePropPoints()
    {
        var props = Resources.Load<PropCollections>("Props").props;
        _propGenerator.propsPoints.Clear();
        var transform = Selection.activeGameObject.transform;
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            var prop = props.FirstOrDefault(t => t.prefab.name == child.gameObject.name);
            if (prop != null)
            {
                _propGenerator.propsPoints.Add(new Prop(child.gameObject, prop.type));

                //ЗАЧИСТКА ДЕТЕЙ
                for (int j = 0; j < child.childCount; j++)
                {
                    DestroyImmediate(child.GetChild(j).gameObject);
                }

                Component[] components = child.gameObject.GetComponents<Component>();
                
                foreach (Component component in components)
                {            
                    if (!(component is Transform))
                    {
                        DestroyImmediate(component);
                    }
                }
            }
        }

    }
}
