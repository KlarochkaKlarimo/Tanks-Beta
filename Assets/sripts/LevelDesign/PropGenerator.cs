using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Properties;
using UnityEngine;

public class PropGenerator : MonoBehaviour
{
    [SerializeField] private List<Prop> propsPoints;
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
