using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace R1.Automation.UI.core.Selenium.Base
{
    public class DisposeDriverService
    {
        private static readonly List<string> _processesToCheck =
        new List<string>
        {
            "chromedriver",
            "IEDriverServer",
        };
        public static void FinishHim()
        {
            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                try
                {
                        Debug.WriteLine(process.ProcessName);
                        foreach (var processName in _processesToCheck)
                        {                        
                            if (process.ProcessName.ToLower().Equals(processName))
                            {
                                process.Kill();
                            }
                        }
                }                
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }
}
    
}