#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(BotController))]
public class BotControllerEditor : Editor
{
    private List<BotConfig> _botConfigs;
    private string[] _botConfigNames;
    private int _selectedIndex = -1;
    private string botName = "";
    private bool isGeneralPropertiesOpen = false;

    private void OnEnable()
    {
        LoadBotConfigs();
        UpdateSelectedIndex();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.BeginHorizontal();
        botName = EditorGUILayout.TextField(botName);
        if (GUILayout.Button("+ Create a new bot"))
        {
            if (CreateNewBot(botName))
            {
                LoadBotConfigs();
                UpdateSelectedIndex();
                BotActionRegistrar.UpdateBotActions();
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        int newIndex = EditorGUILayout.Popup("", _selectedIndex, _botConfigNames);

        if (_selectedIndex != newIndex)
        {
            _selectedIndex = newIndex;
            (target as BotController).selectedBotConfig = _botConfigs[_selectedIndex];

            EditorUtility.SetDirty(target);
        }

        if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>("Assets/Textures/Icons/Trashcan.png"), EditorStyles.iconButton))
        {
            if (DeleteSelectedBot())
            {
                LoadBotConfigs();
                UpdateSelectedIndex();
            }
        }

        EditorGUILayout.EndHorizontal();

        //BotConfig selectedConfig = (target as BotController).selectedBotConfig;
        //if (selectedConfig != null)
        //{
        //    EditorGUI.BeginChangeCheck();



        //    if (EditorGUI.EndChangeCheck())
        //    {
        //        EditorUtility.SetDirty(selectedConfig);
        //    }
        //}

        EditorGUILayout.Space();

        EditorGUI.DrawRect(GUILayoutUtility.GetRect(0, 1, GUILayout.ExpandWidth(true)), new Color(.4f, .4f, .4f));

        if ((target as BotController).selectedBotConfig)
        {
            EditorGUILayout.Space();

            foreach (IBotAction botAction in (target as BotController).selectedBotConfig.actions)
            {
                GUIStyle verticalStyle = new GUIStyle(EditorStyles.helpBox);
                verticalStyle.padding = new RectOffset(0, 0, 0, 0);
                verticalStyle.margin = new RectOffset(1, 0, 1, 0);
                verticalStyle.normal.textColor = Color.white;
                verticalStyle.fontSize = 10;

                EditorGUILayout.BeginVertical(verticalStyle);

                Rect headerRect = GUILayoutUtility.GetRect(0, 16f, verticalStyle, GUILayout.ExpandWidth(true));
                EditorGUI.DrawRect(headerRect, new Color(0.35f, 0.35f, 0.35f));

                Rect checkboxRect = new Rect(headerRect.x + 4, headerRect.y + 1, 14, 14);
                botAction.IsEnabled = EditorGUI.Toggle(checkboxRect, botAction.IsEnabled);

                GUIStyle collapsibleStyle = new GUIStyle(EditorStyles.helpBox);
                collapsibleStyle.padding = new RectOffset(0, 0, 0, 0);
                collapsibleStyle.margin = new RectOffset(1, 0, 0, 0);
                collapsibleStyle.normal.textColor = Color.white;
                collapsibleStyle.contentOffset = new Vector2(25, 0);
                collapsibleStyle.fontSize = 12;
                collapsibleStyle.hover.textColor = Color.white;

                Rect collapsibleRect = new Rect(headerRect.x - 1f, headerRect.y - 1f, headerRect.width + 1.5f, headerRect.height + 1.5f);
                if (GUI.Button(collapsibleRect, ObjectNames.NicifyVariableName(botAction.GetType().Name), collapsibleStyle))
                {
                    botAction.IsCollapsed = !botAction.IsCollapsed;
                }

                if (!botAction.IsCollapsed)
                {
                    EditorGUI.indentLevel++;
                    using (new EditorGUI.DisabledScope(!botAction.IsEnabled))
                    {
                        var fields = botAction.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        foreach (var field in fields)
                        {
                            string noSpaceFieldName = ObjectNames.NicifyVariableName(field.Name).Replace(" ", "");
                            if (noSpaceFieldName == "IsCollapsed" || noSpaceFieldName == "IsEnabled") // Blacklisting properties to not show
                                continue;

                            var value = field.GetValue(botAction);
                            object newValue = null;

                            if (field.FieldType == typeof(int))
                            {
                                newValue = EditorGUILayout.IntField(ObjectNames.NicifyVariableName(field.Name), (int)value);
                            }
                            else if (field.FieldType == typeof(float))
                            {
                                newValue = EditorGUILayout.FloatField(ObjectNames.NicifyVariableName(field.Name), (float)value);
                            }
                            else if (field.FieldType == typeof(string))
                            {
                                newValue = EditorGUILayout.TextField(ObjectNames.NicifyVariableName(field.Name), (string)value);
                            }
                            else if (field.FieldType == typeof(bool))
                            {
                                newValue = EditorGUILayout.Toggle(ObjectNames.NicifyVariableName(field.Name), (bool)value);
                            }

                            if (newValue != null && !Equals(newValue, value))
                            {
                                field.SetValue(botAction, newValue);
                                EditorUtility.SetDirty((target as BotController).selectedBotConfig);
                            }
                        }
                    }
                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.Space();

            EditorGUI.DrawRect(GUILayoutUtility.GetRect(0, 1, GUILayout.ExpandWidth(true)), new Color(.4f, .4f, .4f));

            EditorGUILayout.Space();
        }

        EditorGUILayout.BeginVertical();

        isGeneralPropertiesOpen = EditorGUILayout.Foldout(isGeneralPropertiesOpen, "General Properties", true);

        if (isGeneralPropertiesOpen)
        {
            EditorGUI.indentLevel++;

            EditorGUI.BeginChangeCheck();

            (target as BotController).playerController = (PlayerController)EditorGUILayout.ObjectField("Player Controller", (target as BotController).playerController, typeof(PlayerController), true);
            //(target as BotController).gameState = (GameState)EditorGUILayout.ObjectField("Game State", (target as BotController).gameState, typeof(GameState), true);
            //(target as BotController).parrySpark = (ParticleSystem)EditorGUILayout.ObjectField("Parry Spark", (target as BotController).parrySpark, typeof(ParticleSystem), true);

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }

            EditorGUI.indentLevel--;
        }

        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }

    private void UpdateSelectedIndex()
    {
        BotConfig currentConfig = (target as BotController).selectedBotConfig;
        _selectedIndex = _botConfigs.IndexOf(currentConfig);
        if (_selectedIndex == -1)
        {
            _selectedIndex = 0;
            if (_botConfigs.Count > 0)
            {
                (target as BotController).selectedBotConfig = _botConfigs[0];
            }
        }
    }

    private bool CreateNewBot(string botName)
    {
        if (!AssetDatabase.AssetPathExists("Assets/ScriptableObjects/Bots/" + botName + ".asset"))
        {
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<BotConfig>(), "Assets/ScriptableObjects/Bots/" + botName + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return true;
        } else
        {
            Debug.LogError("Can't create this bot configuration! The bot name is already used!");
        }

        return false;
    }

    private bool DeleteSelectedBot()
    {
        if (_botConfigNames.Length > _selectedIndex && _selectedIndex > -1)
        {
            if (!AssetDatabase.DeleteAsset("Assets/ScriptableObjects/Bots/" + _botConfigNames[_selectedIndex] + ".asset"))
            {
                Debug.LogError("Failed to delete a bot configuration file.");
                return false;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return true;
        } else
        {
            Debug.LogError("Can't delete selected bot configuration! There are no bot configurations created!");
        }

        return false;
    }

    private void LoadBotConfigs()
    {
        _botConfigs = new List<BotConfig>();

        string[] guids = AssetDatabase.FindAssets("t:BotConfig");

        foreach (string guid in guids)
        {
            _botConfigs.Add(AssetDatabase.LoadAssetAtPath<BotConfig>(AssetDatabase.GUIDToAssetPath(guid)));
        }

        _botConfigNames = new string[_botConfigs.Count];

        for (int i = 0; i < _botConfigs.Count; i++)
        {
            _botConfigNames[i] = _botConfigs[i].name;
        }
    }
}
#endif