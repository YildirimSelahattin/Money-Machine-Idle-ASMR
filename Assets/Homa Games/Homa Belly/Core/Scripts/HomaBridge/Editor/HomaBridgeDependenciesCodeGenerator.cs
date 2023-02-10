using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace HomaGames.HomaBelly
{
    public class HomaBridgeDependenciesCodeGenerator : IPreprocessBuildWithReport
    {
        private const string AUTO_GENERATED_SCRIPTS_PARENT_FOLDER = "Assets/Homa Games/Homa Belly/Core/Scripts/HomaBridge/Dependencies/";

        [InitializeOnLoadMethod]
        private static void AutoGenerateCode()
        {
            AutoGenerateInitializationFiles(false);
        }
        
        [MenuItem("Tools/Homa Belly/Force Homa Bridge Code Generation")]
        private static void AutoGenerateCodeForced()
        {
            AutoGenerateInitializationFiles(true);
        }

        private static void AutoGenerateInitializationFiles(bool force)
        {
            AutoGenerateInitializationFile("HomaBridgeDependenciesMediators",
                "PartialInstantiateMediators",
                "mediators",
                typeof(MediatorBase),force);
            
            AutoGenerateInitializationFile("HomaBridgeDependenciesOldMediators",
                "PartialInstantiateOldMediators",
                "oldMediators",
                typeof(IMediator),force);
            
            AutoGenerateInitializationFile("HomaBridgeDependenciesAttributions",
                "PartialInstantiateAttributions",
                "attributions",
                typeof(IAttribution),force);
            
            AutoGenerateInitializationFile("HomaBridgeDependenciesAnalytics",
                "PartialInstantiateAnalytics",
                "analytics",
                typeof(IAnalytics),force);
            
            AutoGenerateInitializationFile("HomaBridgeDependenciesCustomerSupport",
                "PartialInstantiateCustomerSupport",
                "customerSupport",
                typeof(CustomerSupportImplementation),force,true);
        }

        private static void AutoGenerateInitializationFile(string fileName,
            string methodName,
            string servicesListName,
            Type serviceType,
            bool force,
            bool singleton = false)
        {
            var completeFilePath = $"{AUTO_GENERATED_SCRIPTS_PARENT_FOLDER}{fileName}.cs";
            var availableServices = Reflection.GetHomaBellyAvailableClasses(serviceType);
            
            // If there are no services, we might be within a use case of a given
            // service being uninstalled. Hence, we want to fully reset the autogenerated
            // class without services at all
            if (availableServices == null)
            {
                availableServices = new List<Type>();
            }
            
            if (!force)
            {
                // To avoid overriding and generating a file everytime there is a domain reload,
                // check if something has changed first.
                
                bool fileExist = File.Exists(completeFilePath);
            
                // Create a hash with all available services of the same type
                // This will change if we add/remove a service so we can regenerate the code
                var servicesHash = string.Join(",", availableServices);

                bool servicesHashMatch = false;
               
                var editorPrefsServiceKey = $"Key{serviceType.Name}{Application.productName}";
                if (EditorPrefs.HasKey(editorPrefsServiceKey))
                {
                    var savedHash = EditorPrefs.GetString(editorPrefsServiceKey, "None");
                    servicesHashMatch = savedHash == servicesHash;
                }
            
                if (!servicesHashMatch)
                {
                    EditorPrefs.SetString(editorPrefsServiceKey,servicesHash);
                }

                bool hasToGenerate = !fileExist || !servicesHashMatch;

                if (!hasToGenerate)
                {
                    return;
                }
            }

            HomaGamesLog.Debug($"[HomaBelly] Auto generating code for {serviceType} Services: {string.Join(",", availableServices)}. File: {fileName}");
            if (!Directory.Exists(AUTO_GENERATED_SCRIPTS_PARENT_FOLDER))
            {
                Directory.CreateDirectory(AUTO_GENERATED_SCRIPTS_PARENT_FOLDER);
            }

            using (var streamWriter = new StreamWriter(File.Create(completeFilePath)))
            {
                streamWriter.WriteLine($"namespace HomaGames.HomaBelly");
                streamWriter.WriteLine("{");
                streamWriter.WriteLine($"\t public static partial class {nameof(HomaBridgeDependencies)}");
                streamWriter.WriteLine("\t {");
                streamWriter.WriteLine($"\t \t static partial void {methodName}()");
                streamWriter.WriteLine("\t \t {");
                streamWriter.WriteLine($"// Homa Belly services won't run on Unity Editor. This is a design decision to allow some runtime optimizations");
                streamWriter.WriteLine("#if !UNITY_EDITOR");

                streamWriter.WriteLine($"\t\t\t // This method will be filled automatically by:{nameof(HomaBridgeDependenciesCodeGenerator)} when Homa Belly services are added/removed to the project.");
                if (availableServices.Count > 0)
                {
                    if (singleton)
                    {
                        streamWriter.WriteLine($"\t\t\t {servicesListName} = new {availableServices[0].Name}();");
                    }
                    else
                        foreach (var type in availableServices)
                        {
                            streamWriter.WriteLine($"\t\t\t {servicesListName}.Add(new {type.Name}());");
                        }
                }

                streamWriter.WriteLine("#endif");
                streamWriter.WriteLine("\t \t }");
                streamWriter.WriteLine("\t }");
                streamWriter.WriteLine("}");
            }

            AssetDatabase.Refresh();
        }

        public int callbackOrder => 0;

        /// <summary>
        /// Before processing the build make sure we have all the required auto generated code
        /// </summary>
        /// <param name="report"></param>
        public void OnPreprocessBuild(BuildReport report)
        {
            AutoGenerateInitializationFiles(true);
        }
    }
}