using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using JetBrains.Util;
using JetBrains.Util.Collections;

using Lex;
using Microsoft.VisualBasic;

namespace ReSharperPlugin.MyPlugin.Highlighter;

public class BadWordRepository
{
    private static string path;
    private static FileSystemWatcher watcher;
    private static Thread fileThread;
    public static ConcurrentDictionary<String, String> Words { get; } = new ConcurrentDictionary<string, string>();

    static BadWordRepository()
    {
        
        path = Microsoft.VisualBasic.Interaction.InputBox("Enter path to bad word directory", 
            "Censorship Plugin", 
            "D:\\Marko\\Downloads\\MyPlugin\\src\\dotnet\\ReSharperPlugin.MyPlugin\\Highlighter", 
            -1, -1);
        fileThread = new Thread(RefreshRepository);
        fileThread.Start();
        _setupWatcher();
    }

    private static void _setupWatcher()
    {
        watcher = new FileSystemWatcher
        {
            IncludeSubdirectories = true,
            Path = path,
            Filter = "*.*"
        };

        // Subscribe to the events you're interested in
        watcher.Created += OnFileSystemChanged;
        watcher.Deleted += OnFileSystemChanged;
        watcher.Changed += OnFileSystemChanged;

        // Enable the watcher
        watcher.EnableRaisingEvents = true;

    }

    private static void OnFileSystemChanged(object sender, FileSystemEventArgs e)
    {
        fileThread = new Thread(RefreshRepository);
        fileThread.Start();
    }

    public static void RefreshRepository()
    {
        try
        {
            Words.Clear();
            if (!Directory.Exists(path))
            {
                MessageBox.ShowExclamation("Directory does not exist.");
                return;
            }

            string[] files = Directory.GetFiles(path);

            foreach (string filePath in files)
            {
                if (filePath.EndsWith(".txt"))
                {
                    _loadFile(filePath);
                }
               
            }
        }
        //Current behaviour allows directories with non-text files, just ignores them
        //Alternative would be to throw an exception when we run into a non-text file
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

    }

    private static void _loadFile(string filePath)
    {
        try
        {
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] words = line.Split(' ');
                string[] pair = {};
                if (words.Length == 3)
                {
                    pair = new string[] {words[0], words[2]};
                }
                else
                {
                    Console.WriteLine($"Invalid line format: {line}");
                }
                if (pair.Length == 2)
                {
                    Words[pair[0].ToLower()] = pair[1];
                }
                else
                {
                    Console.WriteLine($"Invalid line format: {line}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
        }
    }
    
}