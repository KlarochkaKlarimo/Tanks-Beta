using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditor.Tanks;


[CustomEditor(typeof(PropGenerator))]
public class PropGeneratorEditor : Editor
{
    private PropGenerator _propGenerator;
    public List<Prop> propsPoints;
    public List<PropLimit> propsLimit;

    private void OnEnable()
    {
        _propGenerator = Selection.activeGameObject.GetComponent<PropGenerator>();
        propsPoints = _propGenerator.propsPoints;
        propsLimit = _propGenerator.propsLimit;
    }
    public override void OnInspectorGUI()
    {      
        if (GUILayout.Button("Generate Prop Point", GUILayout.Width(200)))
        {
            GeneratePropPoints();
            propsPoints = _propGenerator.propsPoints;
            propsLimit = _propGenerator.propsLimit;
        }
        _propGenerator.propsPoints = propsPoints;
        _propGenerator.propsLimit = propsLimit;

        serializedObject.GenerateEditorArray("propsPoints");
        serializedObject.GenerateEditorArray("propsLimit");
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

