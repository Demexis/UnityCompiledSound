using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CompiledSound.Editor {
    [InitializeOnLoad]
    internal static class CompiledSoundSource {
        private const string START_COMPILATION_AUDIO_FILE_NAME = "StartCompilationSound";
        private const string END_COMPILATION_AUDIO_FILE_NAME = "EndCompilationSound";
        private const string COMPILE_STATE_PREFS_KEY = "CompileIndicator.WasCompiling";
        private const string UNITY_EDITOR_AUDIO_UTIL_ASSEMBLY_NAME = "UnityEditor.AudioUtil";
        private const string PLAY_PREVIEW_CLIP_METHOD_NAME = "PlayPreviewClip";

        private const string WELCOME_INIT_MESSAGE = "Compilation sounds are not set. "
                                                    + "Put '<color=#FF7F00>" + START_COMPILATION_AUDIO_FILE_NAME 
                                                    + "</color>' or/and '<color=#FF7F00>" + END_COMPILATION_AUDIO_FILE_NAME 
                                                    + "</color>' audio clips under '<color=#FF7F00>Resources</color>' folder.";

        private const string ERROR_FAILED_PLAY_PREVIEW = "Failed to play sound on compilation. "
                                                         + "Can't find <color=#FF7F00>" + PLAY_PREVIEW_CLIP_METHOD_NAME 
                                                         + "</color> method in <color=#FF7F00>" + UNITY_EDITOR_AUDIO_UTIL_ASSEMBLY_NAME 
                                                         + "</color> assembly.";
        
        private static readonly AudioClip startCompilationClip;
        private static readonly AudioClip endCompilationClip;

        static CompiledSoundSource() {
            EditorApplication.update += OnUpdate;
            startCompilationClip = Resources.Load<AudioClip>(START_COMPILATION_AUDIO_FILE_NAME);
            endCompilationClip = Resources.Load<AudioClip>(END_COMPILATION_AUDIO_FILE_NAME);
        }

        private static void OnUpdate() {
            var wasCompiling = EditorPrefs.GetBool(COMPILE_STATE_PREFS_KEY);
            var isCompiling = EditorApplication.isCompiling;
            
            if (wasCompiling == isCompiling) {
                return;
            }

            if (isCompiling) {
                OnStartCompiling();
            } else {
                OnEndCompiling();
            }
            
            EditorPrefs.SetBool(COMPILE_STATE_PREFS_KEY, isCompiling);
        }

        private static void OnStartCompiling() {
            PlayStartCompilationSound();
        }

        private static void OnEndCompiling() {
            PlayEndCompilationSound();
            PlayWelcomeInitLogMessage();
        }

        private static void PlayStartCompilationSound() {
            if (startCompilationClip == null) {
                return;
            }
            
            PlayPreviewClip(startCompilationClip);
        }

        private static void PlayEndCompilationSound() {
            if (endCompilationClip == null) {
                return;
            }
            
            PlayPreviewClip(endCompilationClip);
        }

        private static void PlayWelcomeInitLogMessage() {
            if (startCompilationClip != null || endCompilationClip != null) {
                return;
            }
            
            Debug.Log(WELCOME_INIT_MESSAGE);
        }
        
        private static void PlayPreviewClip(AudioClip clip) {
            const int START_SAMPLE = 0;
            const bool IS_LOOP = false;
            
            var unityEditorAssembly = typeof(AudioImporter).Assembly;
            var audioUtilClass = unityEditorAssembly.GetType(UNITY_EDITOR_AUDIO_UTIL_ASSEMBLY_NAME);
            var method = audioUtilClass.GetMethod(PLAY_PREVIEW_CLIP_METHOD_NAME,
                BindingFlags.Static | BindingFlags.Public,
                null,
                new[] {typeof(AudioClip), typeof(int), typeof(bool)},
                null);

            if (method == null) {
                Debug.LogError(ERROR_FAILED_PLAY_PREVIEW);
                return;
            }
            
            method.Invoke(null,
                new object[] {clip, START_SAMPLE, IS_LOOP});
        }
    }
}