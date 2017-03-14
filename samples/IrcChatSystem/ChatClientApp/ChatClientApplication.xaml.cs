using System.Windows;
using Hik.Samples.Scs.IrcChat.Client;
using Hik.Samples.Scs.IrcChat.Windows;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using Hardcodet.Wpf.TaskbarNotification;

namespace Hik.Samples.Scs.IrcChat
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class ChatClientApplication : Application
    {
        string myLanIP = "";
        string myInternetIP = "";
        string myServerIP = "192.168.11.8";
        string myServerPort = "22222";
        string myCheckInternet = "http://checkip.dyndns.org";
        int waitMinute = 1;

        private TaskbarIcon notifyIcon;
        
        /// <summary>
        /// 
        /// </summary>
        public ChatClientApplication()
        {
           

            while(isRecheck)
            {
                //
                
                myLanIP = GetComputer_LanIP();
                if(myLanIP=="" || myLanIP =="127.0.0.1")
                {
                    Console.WriteLine("Suljeegyi");
                    MessageBox.Show("Сүлжээгүй байна.");
                    
                    //wpf wait window running
                    Thread.Sleep(1000 * 60 * waitMinute);  
                    continue;
                }

                Console.WriteLine("LanIP:");
                Console.WriteLine(myLanIP);

                if(CheckForInternetConnection()==true)
                {
                    myInternetIP = GetComputer_InternetIP(myCheckInternet);
                    if (myInternetIP == "")
                    {
                        Console.WriteLine("Internet");
                        //MessageBox.Show("Сүлжээгүй байна.");
                    }
                    Console.WriteLine("InternetIP:");
                    Console.WriteLine(myInternetIP);
                }
                
                if(PingHost(myServerIP) ==false)
                {
                    Console.WriteLine("Server holbolt alga");
                    MessageBox.Show("Сэрвэр ажиллахгүй байна.");
                    //wpf wait window running
                    Thread.Sleep(1000 * 60 * waitMinute);  
                    continue;
                }

                Console.WriteLine("Server Ok (" + myServerIP + ")");
                
                break;
            }
            
            //network check

            //get ip
            
            //server check
            
            //auto start
            //InstallMeOnStartUp();

            //
            Startup += AppStartUp;
            

        }

        static void AppStartUp(object sender, StartupEventArgs e)
        {
            
        }
        
        /// <summary>
        /// only one start
        /// http://pietschsoft.com/post/2009/01/Single-Instance-WPF-Application-in-NET-3
        /// </summary>
        protected override void OnStartup(StartupEventArgs e)
        {
            // Get Reference to the current Process
            Process thisProc = Process.GetCurrentProcess();
            // Check how many total processes have the same name as the current one
            if (Process.GetProcessesByName(thisProc.ProcessName).Length > 1)
            {
                // If ther is more than one, than it is already running.
                MessageBox.Show("Application is already running.");
                Application.Current.Shutdown();
                return;
            }

            base.OnStartup(e);

            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }

        /// <summary>
        /// How to make an Application auto run on Windows startup
        /// https://social.msdn.microsoft.com/Forums/vstudio/en-US/36e3a48e-9fe4-4190-b61a-7f965173276a/c-wpf-run-the-application-at-windows-startup-?forum=wpf
        /// Auto start
        /// </summary>
        void InstallMeOnStartUp()
        {
            try
            {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                Assembly curAssembly = Assembly.GetExecutingAssembly();
                key.SetValue(curAssembly.GetName().Name, curAssembly.Location);
            }
            catch { }
        }

        private string GetComputer_LanIP()
        {
            string strHostName = System.Net.Dns.GetHostName();

            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

            foreach (IPAddress ipAddress in ipEntry.AddressList)
            {
                if (ipAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    return ipAddress.ToString();
                }
            }

            return "-";
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private string GetComputer_InternetIP(string requestUrl)
        {
            try { 
            // check IP using DynDNS's service
            WebRequest request = WebRequest.Create(requestUrl);
            WebResponse response = request.GetResponse();
            StreamReader stream = new StreamReader(response.GetResponseStream());

            // IMPORTANT: set Proxy to null, to drastically INCREASE the speed of request
            //request.Proxy = null;

            // read complete response
            string ipAddress = stream.ReadToEnd();

            // replace everything and keep only IP
            return ipAddress.
                Replace("<html><head><title>Current IP Check</title></head><body>Current IP Address: ", string.Empty).
                Replace("</body></html>", string.Empty);
            }
            catch {
                Console.WriteLine("No internet");
                return "";
            }
        }

        public static bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = new Ping();
            try
            {
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            return pingable;
        }
    }


}
