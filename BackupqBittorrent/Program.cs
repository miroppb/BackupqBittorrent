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
                Console.WriteLine("Closing " + proc + "...");
                process.Kill();
            }
            return true;
        }
    }
    return false;
}

Console.WriteLine("Stopping qBittorrent if running...");
_ = CheckRunningProcess("qbittorrent", true);

while (CheckRunningProcess("qbittorrent", false))
{
    await Task.Delay(10000);
    Console.WriteLine("Checking if qBittorrent is running...");
}

Console.WriteLine("qBittorent is closed. Press key to continue");
Console.ReadLine();

if (Environment.GetCommandLineArgs()[1].ToString() == "restore")
{
    string loc = Environment.GetCommandLineArgs()[2].ToString();
    string rar = Environment.ExpandEnvironmentVariables("%ProgramW6432%") + "\\WinRAR\\Rar.exe";

    string b = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    Console.WriteLine("Executing: rar " + "x -kb \"" + loc + "\\qbt_local.rar\" \"" + b + "\"");

    Process p = new Process();
    p.StartInfo.FileName = rar;
    p.StartInfo.Arguments = "x -kb \"" + loc + "\\qbt_local.rar\" \"" + b + "\"";
    p.Start();
    p.WaitForExit();

    b = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    Console.WriteLine("Executing: rar " + "x -kb \"" + loc + "\\qbt_roaming.rar\" \"" + b + "\"");

    p.StartInfo.FileName = rar;
    p.StartInfo.Arguments = "x -kb \"" + loc + "\\qbt_roaming.rar\" \"" + b + "\"";
    p.Start();
    p.WaitForExit();

    Console.WriteLine("Done restoring.");
}
else if (Environment.GetCommandLineArgs()[1].ToString() == "backup")
{
    string loc = Environment.GetCommandLineArgs()[2].ToString();
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

    Console.WriteLine("Done backing up.");
}
else
{
    Console.WriteLine("Starting qBittorrent again. Exiting...");
    Process p = new Process();
    p.StartInfo.FileName = Environment.ExpandEnvironmentVariables("%ProgramW6432%") + "\\qBittorrent\\qbittorrent.exe";
    p.StartInfo.Arguments = "";
    p.Start();
}