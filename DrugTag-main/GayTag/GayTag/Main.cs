using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using BepInEx;
using ExitGames.Client.Photon;
using GorillaLocomotion;
using GorillaNetworking;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;
using Utilla;
using static OVRPlugin;

namespace Colossal {
    [BepInPlugin("org.ColossusYTTV", "GayTag", "1.0.0")]
    public class Main : BaseUnityPlugin {
        private void OnEnable() {
            HarmonyPatches.ApplyHarmonyPatches();
        }

        private void OnDisable() {
            HarmonyPatches.RemoveHarmonyPatches();
        }

        private bool enabled = true;
        private bool normal = false;
        private Shader meshrendershader;

        private float glossiness = 0f;
        private float matalicness = 0f; //spelling go brrr

        private float colorr = 0f;
        private float colorg = 0f;
        private float colorb = 0f;

        private bool rgb = false;
        private float rgbspeed = 0.5f;
        private float rgbcolorTimer = 0f;
        private Color rgbcolour = Color.red;

        private void Start() {
            MeshRenderer[] meshrender = GameObject.FindObjectsOfType<MeshRenderer>();
            foreach (MeshRenderer rend in meshrender) {
                meshrendershader = rend.material.shader;
            }
        }

        private void OnGUI() {
            enabled = GUILayout.Toggle(enabled, "ENABLE");
            if(enabled) {
                GUI.color = Color.magenta;

                glossiness = GUILayout.HorizontalSlider(glossiness, 0f, 3);
                matalicness = GUILayout.HorizontalSlider(matalicness, 0f, 3);

                GUILayout.Space(10);

                GUI.color = Color.red;
                colorr = GUILayout.HorizontalSlider(colorr, 0f, 1f);

                GUI.color = Color.green;
                colorg = GUILayout.HorizontalSlider(colorg, 0f, 1f);

                GUI.color = Color.blue;
                colorb = GUILayout.HorizontalSlider(colorb, 0f, 1f);

                GUILayout.Space(10);

                GUI.color = rgbcolour;
                rgb = GUILayout.Toggle(rgb, $"<color={rgbcolour}>RGB</color>");
                if (rgb) {
                    rgbspeed = GUILayout.HorizontalSlider(rgbspeed, 0f, 4);
                }
            }
        }
        private void Update() {
            if(enabled) {
                if (rgb) {
                    float r = Mathf.Lerp(0f, 1f, Mathf.Abs(Mathf.Sin(rgbcolorTimer * 0.4f)));
                    float g = Mathf.Lerp(0f, 1f, Mathf.Abs(Mathf.Sin(rgbcolorTimer * 0.5f)));
                    float b = Mathf.Lerp(0f, 1f, Mathf.Abs(Mathf.Sin(rgbcolorTimer * 0.6f)));
                    rgbcolour = new Color(r, g, b);
                    rgbcolorTimer += Time.deltaTime * rgbspeed;
                }

                MeshRenderer[] meshrender = GameObject.FindObjectsOfType<MeshRenderer>();
                foreach (MeshRenderer rend in meshrender) {
                    rend.material.shader = Shader.Find("Standard");
                    if (rgb) {
                        rend.material.color = rgbcolour;
                    } else {
                        rend.material.color = new Color(colorr, colorg, colorb);
                    }
                    rend.material.SetFloat("_Glossiness", glossiness);
                    rend.material.SetFloat("_Metallic", matalicness);
                }
                SkinnedMeshRenderer[] skinnedmeshrender = GameObject.FindObjectsOfType<SkinnedMeshRenderer>();
                foreach (SkinnedMeshRenderer rend in skinnedmeshrender) {
                    rend.material.shader = Shader.Find("Standard");
                    if (rgb) {
                        rend.material.color = rgbcolour;
                    } else {
                        rend.material.color = new Color(colorr, colorg, colorb);
                    }
                    rend.material.SetFloat("_Glossiness", glossiness);
                    rend.material.SetFloat("_Metallic", matalicness);
                }
                Renderer[] render = GameObject.FindObjectsOfType<Renderer>();
                foreach (Renderer rend in render) {
                    rend.material.shader = Shader.Find("Standard");
                    if (rgb) {
                        rend.material.color = rgbcolour;
                    } else {
                        rend.material.color = new Color(colorr, colorg, colorb);
                    }
                    rend.material.SetFloat("_Glossiness", glossiness);
                    rend.material.SetFloat("_Metallic", matalicness);
                }

                normal = false;
            } else {
                if(!normal) {
                    MeshRenderer[] meshrender = GameObject.FindObjectsOfType<MeshRenderer>();
                    foreach (MeshRenderer rend in meshrender) {
                        rend.material.shader = meshrendershader;
                        rend.material.color = Color.white;
                    }
                    SkinnedMeshRenderer[] skinnedmeshrender = GameObject.FindObjectsOfType<SkinnedMeshRenderer>();
                    foreach (SkinnedMeshRenderer rend in skinnedmeshrender) {
                        rend.material.shader = meshrendershader;
                        rend.material.color = Color.white;
                    }
                    Renderer[] render = GameObject.FindObjectsOfType<Renderer>();
                    foreach (Renderer rend in render) {
                        rend.material.shader = meshrendershader;
                        rend.material.color = Color.white;
                    }

                    normal = true;
                }
            }
        }
    }
}