﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using XLua;

public class Launch : MonoBehaviour {
    static string mainLuaFile = "Lua/main";
    LuaEnv luaenv = null;
    Action luaUpdate = null;
    Action luaFixedUpdate = null;
    Action luaLateUpdate = null;
    // Use this for initialization
    void Start () {
        luaenv = new LuaEnv();
        //luaenv.AddLoader(MyLoader);
        // 执行代码块，输出 hello world
        string luaScript=string.Format("require '{0}'", mainLuaFile);
        luaenv.DoString(luaScript);
        GameObject.DontDestroyOnLoad(this.gameObject);
        luaUpdate=luaenv.Global.Get<Action>("Update");
        luaFixedUpdate=luaenv.Global.Get<Action>("FixedUpdate");
        luaLateUpdate= luaenv.Global.Get<Action>("LateUpdate");
    }

    private void FixedUpdate()
    {
        if(luaFixedUpdate!=null)
        {
            luaFixedUpdate();
        }
    }

    private void LateUpdate()
    {
        if(luaLateUpdate!=null)
        {
            luaLateUpdate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(luaUpdate!=null)
        {
            luaUpdate();
        }
        if (luaenv != null)
        {
            // 清除 Lua 未手动释放的 Lua对象
            if (Time.frameCount % 100 == 0)
            {
                luaenv.Tick();
            }
            
        }
    }


    public  byte[] MyLoader(ref string filepath)
    {
        Debug.Log(filepath);
        string script = "print('hello zyc')";
        // 将字符串转换成byte[]
        return System.Text.Encoding.UTF8.GetBytes(script);
    }

   
    void OnDestroy()
    {
        if (luaenv != null)
        {
            luaUpdate = null;
            luaLateUpdate = null;
            luaFixedUpdate = null;
            luaenv.Dispose();
        }
    }
}