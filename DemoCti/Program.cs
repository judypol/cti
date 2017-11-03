using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using log4net;
using PLAgentBar;

namespace DemoCti
{
    static class Program
    {
        public static Form1 newFrmMain;
        public static Mutex run;

        private static ILog exLog;

        public static string LastSigninAgentID;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            string logMsgInfo = "";
            Program.ReadLastSigninAgentID();
            if (!Helper.load_sys_LastAgentconfig())
            {
                MessageBox.Show("读取配置文件失败！  ", "读取配置文件", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                bool runone;
                Program.run = new Mutex(true, "PLClient", out runone);
                if (runone)
                {
                    Program.run.ReleaseMutex();
                }
                else if (Helper.Open_One_Instance)
                {
                    MessageBox.Show("你已经打开程序了，不能同时打开多个客户端！", "打开错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                try
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    if (null == Program.exLog)
                    {
                        Program.exLog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                        Program.exLog.Info("client Program is run .....！");
                    }
                    Application.SetCompatibleTextRenderingDefault(false);
                    Program.newFrmMain = new Form1();
                    Application.ThreadException += delegate (object sender, ThreadExceptionEventArgs e)
                    {
                        logMsgInfo = string.Concat(new string[]
                        {
                            "系统发生异常，请联系管理员！",
                            string.Format("版本 {0} ", "1111"),
                            "来源:",
                            e.Exception.Source,
                            ",信息:",
                            e.Exception.Message,
                            ",堆栈:",
                            e.Exception.StackTrace
                        });
                        Program.exLog.Error(logMsgInfo);
                        if (!e.Exception.StackTrace.StartsWith("   在 System.Drawing.BufferedGraphicsContext.CreateCompatibleDIB"))
                        {
                            MessageBox.Show(logMsgInfo, "程序异常", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        }
                    };
                    AppDomain.CurrentDomain.UnhandledException += delegate (object sender, UnhandledExceptionEventArgs e)
                    {
                        Exception ex2 = (Exception)e.ExceptionObject;
                        logMsgInfo = string.Concat(new string[]
                        {
                            "系统发生错误，请联系管理员！",
                            string.Format("版本 {0} ", "111222"),
                            "来源:",
                            ex2.Source,
                            ",信息:",
                            ex2.Message,
                            ",堆栈:",
                            ex2.StackTrace
                        });
                        Program.exLog.Error(logMsgInfo);
                        MessageBox.Show(logMsgInfo, "程序错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    };
                    Application.Run(Program.newFrmMain);
                }
                catch (Exception ex)
                {
                    logMsgInfo = string.Concat(new string[]
                    {
                        "系统发生严重错误，请保存好相关数据，然后重启程序！",
                        string.Format("版本 {0} ", "122"),
                        "来源:\n",
                        ex.Source,
                        ",信息:",
                        ex.Message,
                        ",堆栈:",
                        ex.StackTrace
                    });
                    Program.exLog.Error(logMsgInfo);
                    MessageBox.Show(logMsgInfo, "严重错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    Application.Exit();
                }
            }
        }

        private static void ReadLastSigninAgentID()
        {
            string mDataFilePath = Environment.GetEnvironmentVariable("APPDATA") + "\\" + Helper.App_Directory_Name + "\\data.xml";
            Program.ReadLastSigninAgentIDFromXml(mDataFilePath, "LastSigninID", ref Program.LastSigninAgentID);
        }
        private static void ReadLastSigninAgentIDFromXml(string xmlFileName, string getTypeName, ref string LastSigninAgentID)
        {
            XmlTextReader textReader = new XmlTextReader(xmlFileName);
            textReader.Read();
            try
            {
                while (textReader.Read())
                {
                    XmlNodeType nType = textReader.NodeType;
                    if (nType == XmlNodeType.Element)
                    {
                        if (textReader.Name == getTypeName)
                        {
                            textReader.Read();
                            if (textReader.NodeType == XmlNodeType.Text && textReader.Value != "")
                            {
                                LastSigninAgentID = textReader.Value.ToString();
                            }
                        }
                    }
                }
                textReader.Close();
            }
            catch (Exception e_8C)
            {
            }
        }
    }
}
