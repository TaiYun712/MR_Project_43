using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineShellCtrl : MonoBehaviour
{
    public Renderer shell;           // 指到 OutlineShell 上的 Renderer
    public Material orangeMat;       // 未達基礎用
    public Material yellowMat;       // 已達基礎用

    private void Awake()
    {
        if (shell == null)
        {
            shell = GetComponentInChildren<Renderer>(true);
        }
        Hide();
    }

    public void ShowOrange()
    {
        if (shell == null || orangeMat == null) { return; }
        shell.sharedMaterial = orangeMat;
        shell.enabled = true;
    }

    public void ShowYellow()
    {
        if (shell == null || yellowMat == null) { return; }
        shell.sharedMaterial = yellowMat;
        shell.enabled = true;
    }

    public void Hide()
    {
        if (shell == null) { return; }
        shell.enabled = false;
    }
}
