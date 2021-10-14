using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Forms;
using Clipboard = System.Windows.Forms.Clipboard;

namespace UIAutomationSaveAs
{
    class Program
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr hWnd, EnumWindowProc lpEnumFunc, IntPtr lParam);

        public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parametr);

        [STAThreadAttribute]
        static void Main(string[] args)
        {
            DateTime timeout = DateTime.Now;
            timeout = timeout.AddSeconds(10);
            string fullfileName = @"c:\Temp\WorkDir\BigFile.jpg";
            IntPtr hWindow = GetHWNDWiondow("Сохранить как", timeout);

            //Process[] RunningProcesses = Process.GetProcesses(System.Environment.MachineName);
            //RunningProcesses = Process.GetProcessesByName("WindowsFormsApp4");
            //Console.WriteLine(RunningProcesses[0].Responding);

            List<IntPtr> windowsList = GetChildWindows(hWindow);

            for (int i = 0; i < windowsList.Count; i++)
            {
                AutomationElement saveAsWindow = AutomationElement.FromHandle(windowsList[i]);
                AutomationElementCollection elementCollectionAll = saveAsWindow.FindAll(TreeScope.Subtree, Condition.TrueCondition);
                if (SendFileNameToDialogBox(elementCollectionAll, fullfileName))
                    break;
            }

            for (int i = 0; i < windowsList.Count; i++)
            {
                AutomationElement saveAsWindow = AutomationElement.FromHandle(windowsList[i]);
                AutomationElementCollection elementCollectionAll = saveAsWindow.FindAll(TreeScope.Subtree, Condition.TrueCondition);
                if (InvokeButtonOk(elementCollectionAll, fullfileName))
                    break;
            }

            //AutomationElement eksWindow = AutomationElement.FromHandle(hWindow);
            //AutomationElement saveAsWindow = AutomationElement.FromHandle(windowsList[0]);
            //AutomationElementCollection elementCollectionAll = saveAsWindow.FindAll(TreeScope.Subtree, Condition.TrueCondition);
            //SendFileNameToDialogBox(elementCollectionAll, "", "");

            Console.WriteLine("Good");
            Console.ReadKey();
        }

        private static bool SendFileNameToDialogBox(AutomationElementCollection elementCollectionAll, string fullFileName)
        {
            foreach (AutomationElement autoElement in elementCollectionAll)
            {
                Console.WriteLine(autoElement.Current.Name);
                Console.WriteLine(autoElement.Current.ClassName);
                Console.WriteLine(new string('*', 10));
                if (autoElement.Current.Name.Equals("Имя файла:") && autoElement.Current.ClassName.Contains("Edit"))
                {
                    Clipboard.Clear();
                    autoElement.SetFocus();
                    SendKeys.SendWait("^{HOME}^+{END}{DEL}");
                    Thread.Sleep(300);
                    SendKeys.SendWait(fullFileName);
                    Thread.Sleep(300);
                    return true;
                }
            }

            return false;
        }

        private static bool InvokeButtonOk(AutomationElementCollection elementCollectionAll, string fieldName)
        {
            foreach (AutomationElement autoElement in elementCollectionAll)
            {
                if (autoElement.Current.ClassName.Equals("Button"))
                {
                    if (autoElement.Current.Name == fieldName)
                    {
                        InvokePattern btnPattern = autoElement.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
                        autoElement.SetFocus();
                        //btnPattern.Invoke();
                        SendKeys.SendWait("{ENTER}");
                        return true;
                    }
                }
            }

            return false;
        }

        //private static void OpenView(AutomationElementCollection elementCollectionAll, string windowName, string view)
        //{
        //    foreach (AutomationElement autoElement in elementCollectionAll)
        //    {
        //        //Console.WriteLine(autoElement.Current.Name);
        //        //Console.WriteLine(autoElement.Current.ClassName);
        //        //Console.WriteLine(autoElement.Current.);
        //        if (autoElement.Current.Name.Equals(windowName) && autoElement.Current.ClassName.Contains("Afx:00400000:b:00010005:00000006"))
        //        {
        //            Clipboard.Clear();
        //            autoElement.SetFocus();
        //            SendKeys.SendWait("^{UP}");
        //            Thread.Sleep(100);
        //            SendKeys.SendWait("+{Left}+^{INS}");

        //            int count = 0;
        //            bool found = false;
        //            while (found == false && count < 200)
        //            {
        //                if (Clipboard.ContainsText() == true && Clipboard.GetText().Contains(view))
        //                {
        //                    found = true;
        //                    autoElement.SetFocus();
        //                    SendKeys.SendWait("%{3}");
        //                    Thread.Sleep(100);
        //                }
        //                else
        //                {
        //                    Clipboard.Clear();
        //                    autoElement.SetFocus();
        //                    SendKeys.SendWait("{DOWN}");
        //                    Thread.Sleep(100);
        //                    SendKeys.SendWait("+{Left}^{INS}");
        //                }
        //                count++;
        //            }

        //            if (found)
        //                break;
        //            else
        //                throw new ArgumentNullException("Cannot found view \"" + view + "\"");
        //        }
        //    }
        //}

        private static IntPtr GetHWNDWiondow(string windowHeader, DateTime timeout)
        {
            IntPtr hWindow = new IntPtr();
            bool isExit = false;

            while (isExit == false)
            {
                hWindow = FindWindow("#32770", windowHeader);
                if (!IsValidHandle(hWindow))
                {
                    if (DateTime.Now > timeout)
                        throw new ArgumentNullException("Cannot found launched window \"" + windowHeader + "\"");
                }
                else
                {
                    isExit = true;
                }
            }

            return hWindow;
        }

        private static bool IsValidHandle(IntPtr hWindow)
        {
            return hWindow != IntPtr.Zero;
        }

        private static List<IntPtr> GetChildWindows(IntPtr parent)
        {
            List<IntPtr> result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try
            {
                EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
                EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
            }

            finally
            {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }

            return result;
        }

        private static bool EnumWindow(IntPtr handle, IntPtr pointer)
        {
            GCHandle gch = GCHandle.FromIntPtr(pointer);
            List<IntPtr> list = gch.Target as List<IntPtr>;

            if (list == null)
                throw new InvalidCastException("GCHandle Targer could not be cast as list");

            list.Add(handle);

            return true;
        }
    }
}
