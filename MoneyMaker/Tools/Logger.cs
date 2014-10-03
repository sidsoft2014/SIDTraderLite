using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Objects;

namespace Toolbox
{
    public static class Logger
    {
        static string logFolder = string.Format("{0}\\{1}", Application.StartupPath, "Logs");
        static string eventLog = string.Format("{0}\\{1}.txt", logFolder, "EventLogs");

        static string tradeRecordFolder = string.Format("{0}\\{1}", Application.StartupPath, "Trades");
        static string tradeLog = string.Format("{0}\\{1},txt", tradeRecordFolder, "TradeRecords");

        public static void WriteEvent(string message)
        {
            if (!Directory.Exists(logFolder))
                Directory.CreateDirectory(logFolder);
            if (!File.Exists(eventLog))
                File.Create(eventLog).Dispose();

            File.AppendAllText(eventLog, DateTime.Now.ToString() + "\r\n");
            File.AppendAllText(eventLog, message + "\r\n");
        }
        public static void WriteEvent(object itemOfInterest)
        {
            if (null != itemOfInterest)
                WriteEvent(itemOfInterest.ToString());
        }
        
        public static void RecordTrade(string[] tradeInfo)
        {
            if (!Directory.Exists(tradeRecordFolder))
                Directory.CreateDirectory(tradeRecordFolder);
            if (!File.Exists(tradeLog))
                File.Create(tradeLog).Dispose();

            File.AppendAllText(tradeLog, DateTime.Now.ToString() + "\r\n");
            File.AppendAllLines(tradeLog, tradeInfo);
        }
        public static void RecordTrade(TradeObject trade)
        {
            if (!Directory.Exists(tradeRecordFolder))
                Directory.CreateDirectory(tradeRecordFolder);
            if (!File.Exists(tradeLog))
                File.Create(tradeLog).Dispose();

            File.AppendAllText(tradeLog, DateTime.Now.ToString());
            File.AppendAllText(tradeLog, trade.ToStringVerbose());
        }
    }
}
