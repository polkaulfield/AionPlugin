using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace AionPlugin
{
    public partial class Form1 : Form
    {

        //hardcoded config todo better.
        static string aionPath = "D:\\Games\\GameforgeLive\\Games\\GBR_eng\\AION\\Download";
        static string aionLog = "Chat.log";
        static string playerName = "Shakku";
        static string chatLog = Path.Combine(aionPath, aionLog);

        //event raiser method when chat.log is modified
        private static void RaiseEventOnChatlog()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = aionPath;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = aionLog;
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }

        //event handler
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            try
            {
                var lastLine = File.ReadLines(chatLog).Last();
                ParseChatCommand(lastLine);
            }
            catch { return; }
        }

        //parse the chat commands when event is handled
        private static void ParseChatCommand(string input)
        {
            string chatTimePattern = @"\d\d\d\d.\d\d.\d\d \d\d:\d\d:\d\d : ";
            string basePattern = chatTimePattern + playerName + ": ";

            //command example, if gets matched prints to game
            if (Regex.IsMatch(input, basePattern + @".test "))
            {
                PrintToGame("AionPlugin v0.1 Test ");

            }

        }

        //Print output to game using keystrokes
        private static async void PrintToGame(string output)
        {
            int KeySendDelay = 100;
            string playerToWisp = playerName; //can change to any name like 0 for example so you can get the sended message on the chat window
            string blankChar = "\u00A0"; //blank character

            //filing the 255 chars so the ouput fills the chat window (todo better)
            if (output.Length < 255)
            {
                int charsToWrite = 255 - output.Length;
                for (int i = 0; i < charsToWrite; i++)
                {
                    output = output + blankChar;
                }
            }

            //Todo newline when sending the message so it looks nicer.
            output = "/w " + playerToWisp + " " + output;
            SendKeys.SendWait("{ENTER}");
            await Task.Delay(KeySendDelay);
            SendKeys.SendWait(output);
            await Task.Delay(KeySendDelay);
            SendKeys.SendWait("{ENTER}");

        }


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //turn on the event raiser and handler when form loads
            RaiseEventOnChatlog();
        }
    }
}
