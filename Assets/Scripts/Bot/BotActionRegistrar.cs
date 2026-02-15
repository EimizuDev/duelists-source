#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using System;

[InitializeOnLoad]
public static class BotActionRegistrar
{
    static BotActionRegistrar()
    {
        EditorApplication.delayCall += UpdateBotActions;
    }

    public static void UpdateBotActions()
    {
        List<Type> actionTypes = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(assembly => assembly.GetTypes())
        .Where(type => typeof(IBotAction).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
        .ToList();

        string[] guids = AssetDatabase.FindAssets("t:BotConfig");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            BotConfig botConfig = AssetDatabase.LoadAssetAtPath<BotConfig>(path);

            if (botConfig == null)
                continue;

            bool modified = false;

            foreach (Type actionType in actionTypes)
            {
                bool exists = botConfig.actions.Any(action => action.GetType() == actionType);
                if (!exists)
                {
                    IBotAction actionInstance = Activator.CreateInstance(actionType) as IBotAction;
                    if (actionInstance != null)
                    {
                        botConfig.actions.Add(actionInstance);
                        modified = true;
                    }
                }
            }

            if (modified)
            {
                EditorUtility.SetDirty(botConfig);
            }
        }

        AssetDatabase.SaveAssets();
    }
}
#endif