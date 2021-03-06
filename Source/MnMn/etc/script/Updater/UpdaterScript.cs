﻿/*-*/using System;
/*-*/using System.IO;
/*-*/using System.Data;
/*-*/using System.Linq;

public class UpdaterScript
{
    void ChangeTemporaryColor(string s, ConsoleColor fore, ConsoleColor back)
    {
        var tempFore = Console.ForegroundColor;
        var tempBack = Console.BackgroundColor;

        Console.ForegroundColor = fore;
        Console.BackgroundColor = back;
        Console.WriteLine(s);

        Console.ForegroundColor = tempFore;
        Console.BackgroundColor = tempBack;
    }

    void RemoveFiles(string baseDirectoryPath, string platform)
    {
        var platformDir = platform + @"\";
        var notPlatformDir = string.Compare(platform, "x86", true) == 0 ? @"x64\" : @"x86\";
        var targets = new string[] {
            @"etc\script\define\update-state.html",
            // -- #567 >>
            @"lib\Microsoft.CSharp.dll",
            @"lib\PresentationCore.dll",
            @"lib\PresentationFramework.dll",
            @"lib\System.Data.DataSetExtensions.dll",
            @"lib\System.Data.dll",
            @"lib\System.dll",
            @"lib\System.Drawing.Design.dll",
            @"lib\System.Drawing.dll",
            @"lib\System.Net.Http.dll",
            @"lib\System.Windows.Forms.dll",
            @"lib\System.Xaml.dll",
            @"lib\System.Xml.dll",
            @"lib\System.Xml.Linq.dll",
            @"lib\WindowsBase.dll",
            // -- #567 <<
        };
        var tagetPathList = targets.Select(s => Path.Combine(baseDirectoryPath, s));
        foreach(var targetPath in tagetPathList) {
            var found = false;
            var isDir = targetPath.Last() == Path.DirectorySeparatorChar;
            var fileOrDir = isDir ? 'D' : 'F';
            try {
                if(isDir) {
                    if(Directory.Exists(targetPath)) {
                        found = true;
                        Directory.Delete(targetPath, true);
                    }
                } else if(File.Exists(targetPath)) {
                    found = true;
                    File.Delete(targetPath);
                }
                Console.WriteLine("DEL:{0}: {1}, FOUND = {2}", fileOrDir, targetPath, found);
            } catch(Exception ex) {
                Console.WriteLine("ERR:{0}: {1}, {2}", fileOrDir, targetPath, ex);
            }
        }
    }

    public void Main(string[] args)
    {
        var prevFore = Console.ForegroundColor;
        var prevBack = Console.BackgroundColor;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.BackgroundColor = ConsoleColor.Black;

        Console.WriteLine("<UpdaterScript: START>");
        try {
            if(args.Length != 3) {
                throw new ArgumentException();
            }
            var scriptFilePath = args[0];
            var baseDirectoryPath = args[1];
            var platform = args[2];
            Console.WriteLine("S: {0}", scriptFilePath);
            Console.WriteLine("D: {0}", baseDirectoryPath);
            Console.WriteLine("P: {0}", platform);

            RemoveFiles(baseDirectoryPath, platform);

        } catch(Exception ex) {
            ChangeTemporaryColor("<UpdaterScript: ERROR>", ConsoleColor.Magenta, prevBack);
            Console.WriteLine(ex);
        }

        Console.WriteLine("<UpdaterScript: END>");
        Console.ForegroundColor = prevFore;
        Console.BackgroundColor = prevBack;
    }
}
