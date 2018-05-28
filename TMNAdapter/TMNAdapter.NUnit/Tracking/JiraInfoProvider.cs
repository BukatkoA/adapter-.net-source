﻿using System;
using System.Collections.Generic;
using System.IO;
using TMNAdapter.Core.Common;
using TMNAdapter.Core.Common.Models;
using TMNAdapter.Core.Tracking;

namespace TMNAdapter.Tracking
{
    public class JiraInfoProvider : BaseJiraInfoProvider
    {
        public override IssueModel SaveAttachment(FileInfo file)
        {
            IssueModel issue = base.SaveAttachment(file);

            IssueManager.AddIssue(issue);

            return issue;
        }

        public override IssueModel SaveParameter<T>(string title, T value)
        {
            IssueModel issue = base.SaveParameter(title, value);

            IssueManager.AddIssue(issue);

            return issue;
        }

        public override IssueModel SaveStackTrace(string issueKey, string stackTrace)
        {
            IssueModel issue = base.SaveStackTrace(issueKey, stackTrace);

            IssueManager.AddIssue(issue);

            return issue;
        }
    }
}
