﻿using UnityEngine;
using System.Collections;
using DTool;

namespace DTool
{
    /// <summary>
    /// 显示FPS
    /// </summary>
    public class ShowFPS 
    {
        public float updateInterval = 0.1F;

        private float accum = 0; // FPS accumulated over the interval
        private int frames = 0; // Frames drawn over the interval
        private float timeleft; // Left time for current interval

        public ShowFPS()
        {
            timeleft = updateInterval;
        }

        public void Update()
        {
            timeleft -= Time.deltaTime;
            accum += Time.timeScale / Time.deltaTime;
            ++frames;

            // Interval ended - update GUI text and start new interval
            if (timeleft <= 0.0)
            {
                // display two fractional digits (f2 format)
                float fps = accum / frames;
                string format = System.String.Format("{0:F2} FPS", fps);

                if (InspectorField.Values.IsShowFPS == true)
                    ShowMessage.Add("fps", format);
                timeleft = updateInterval;
                accum = 0.0F;
                frames = 0;
            }
        }
    }
}