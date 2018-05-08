﻿using System;
using System.Collections.Generic;
using System.IO;
using TMNAdapter.Entities;
using TMNAdapter.Tracking;

namespace TMNAdapter.Utilities
{
    public class JiraInfoProvider
    {
        private static Dictionary<string, List<TestParameters>> jiraKeyParameters = new Dictionary<string, List<TestParameters>>();
        private static Dictionary<string, List<string>> jiraKeyAttachments = new Dictionary<string, List<string>>();

        private static string GetJiraTestKey()
        {
            return AnnotationTracker.GetAttributeInCallStack<JiraIssueKeyAttribute>()?.Key;
        }

        public static void SaveAttachment(FileInfo file)
        {
            string key = GetJiraTestKey();

            if (key == null || !file.Exists)
            {
                return;
            }

            string pathToFile = file.DirectoryName;
            string currentDirectory = Directory.GetCurrentDirectory();
            bool isOutOfAttachmentsDir = pathToFile.StartsWith(currentDirectory + FileUtils.GetAttachmentsDir());

            string targetFilePath = isOutOfAttachmentsDir ?
                               FileUtils.SaveFile(file, file.DirectoryName) :
                               pathToFile.Replace(currentDirectory, String.Empty);

            if (jiraKeyAttachments.ContainsKey(key))
            {
                jiraKeyAttachments[key].Add(targetFilePath);
            }
            else
            {
                List<string> files = new List<string>();
                files.Add(targetFilePath);
                jiraKeyAttachments.Add(key, files);
            }
        }

        public static void SaveParameter<T>(string title, T value)
        {
            string key = GetJiraTestKey();
            if (key != null)
            {
                TestParameters parameters = new TestParameters(title, value != null ? value.ToString() : "null");
                if (jiraKeyParameters.ContainsKey(key))
                {
                    jiraKeyParameters[key].Add(parameters);
                }
                else
                {
                    List<TestParameters> newTestParameters = new List<TestParameters>();
                    newTestParameters.Add(parameters);
                    jiraKeyParameters.Add(title, newTestParameters);
                }
            }
        }

        public static void CleanFor(string issueKey)
        {
            if (jiraKeyParameters.ContainsKey(issueKey))
            {
                jiraKeyParameters.Remove(issueKey);
            }
            if (jiraKeyAttachments.ContainsKey(issueKey))
            {
                jiraKeyAttachments.Remove(issueKey);
            }
        }

        public static List<TestParameters> GetIssueParameters(string issueKey)
        {
            return jiraKeyParameters[issueKey] ?? null;
        }

        public static List<string> GetIssueAttachments(string issueKey)
        {
            return jiraKeyAttachments[issueKey] ?? null;
        }
    }
}
