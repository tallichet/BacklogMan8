﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BacklogMan.Client.Core.Service
{
    internal interface INetworkService
    {
        string APIKey { get; set; }

        Task<List<Model.Project>> DownloadProjects();
        Task<Model.Project> DownloadProject(int projectId);

        Task<List<Model.Backlog>> DownloadBacklogs(int projectId);
        Task<Model.Backlog> DownloadBacklog(int projectId, int backlogId);

        Task<List<Model.Story>> DownloadStories(int projectId, int backlogId);
        Task<Model.Story> DownloadStory(int projectId, int backlogId, int stroyId);

        Task<List<Model.Organization>> DownloadOrganizations();
        Task<Model.Organization> DownloadOrganization(string organizationId);

        Task<string> GetApiKey(string username, string password);

        void ClearCache();

        Task<int> AddStory(int projectId, int backlogId, Model.Story newStory);
        Task<int> UpdateStory(Model.Story story);
        Task<bool> DeleteStory(Model.Story Story);        
        Task<bool> MoveStory(int projectId, int targetBacklogId, int movedStoryId, int[] storyIdOrder);
        Task<bool> OrderBacklogInProject(int projectId, int movedBacklog, int[] backlogIdOrder);
        Task<bool> OrderBacklogInOrganization(int organizationId, int movedBacklog, int[] backlogIdOrder);

                    
        
    }
}
