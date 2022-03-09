// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

bool CheckRunningProcess(string proc, bool close)
{
    Process[] running = Process.GetProcesses();
    foreach (Process process in running)
    {
        if (process.ProcessName == proc)
        {
            if (close)
            {
                Console.WriteLine("Closing qBittorrent...");
                process.CloseMainWindow();
            }
            return true;
        }
    }
    return false;
}

if (Environment.GetCommandLineArgs().Length == 1)
{
    Console.Error.WriteLine("Please specify a folder");
    Environment.Exit(0);
}    

Console.WriteLine("Checking if qBittorrent is running...");
_ = CheckRunningProcess("qbittorrent", true);

Console.WriteLine("Checking if qBittorrent is running...");
while (CheckRunningProcess("qbittorrent", false))
{
    await Task.Delay(10000);
    Console.WriteLine("Checking if qBittorrent is running...");
}

Console.WriteLine("qBittorent is closed. Press key to continue");
Console.ReadLine();
string loc = Environment.GetCommandLineArgs()[1].ToString();
string rar = Environment.ExpandEnvironmentVariables("%ProgramW6432%") + "\\WinRAR\\Rar.exe";

string b = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\qBittorrent";

Process p = new Process();
p.StartInfo.FileName = rar;
p.StartInfo.Arguments = "a -m0 -ep1 \"" + loc + "\\qbt_local.rar\" \"" + b + "\"";
p.Start();
p.WaitForExit();

b = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\qBittorrent";

p.StartInfo.FileName = rar;
p.StartInfo.Arguments = "a -m0 -ep1 \"" + loc + "\\qbt_roaming.rar\" \"" + b + "\"";
p.Start();
p.WaitForExit();

Console.WriteLine("Done backing up. Starting qbittorrent again");
p.StartInfo.FileName = Environment.ExpandEnvironmentVariables("%ProgramW6432%") + "\\qBittorrent\\qbittorrent.exe";
p.StartInfo.Arguments = "";
p.Start();