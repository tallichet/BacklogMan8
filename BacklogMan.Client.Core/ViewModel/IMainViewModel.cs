﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BacklogMan.Client.Core.ViewModel
{
    public interface IMainViewModel
    {
        /// <summary>
        /// Api key used to login on backlog man
        /// </summary>
        string ApiKey { get; set; }

        /// <summary>
        /// List of all organizations
        /// </summary>
        ObservableCollection<Model.Organization> Organizations { get; }


        /// <summary>
        /// The currently selected Organization
        /// </summary>
        Model.Organization CurrentOrganization { get; set; }

        /// <summary>
        /// Contains the list of projects for the current organization
        /// </summary>
        ObservableCollection<Model.Project> OrganizationProjects { get; }

        /// <summary>
        /// Contains the list of backlogs for the current organization
        /// </summary>
        ReorderableCollection<Model.Backlog> OrganizationBacklogs { get; }


        /// <summary>
        /// List of all projects
        /// </summary>
        ObservableCollection<Model.Project> Projects { get; }

        /// <summary>
        /// List of projects not in an organization
        /// </summary>
        ObservableCollection<Model.Project> ProjectsStandalone { get; }


        /// <summary>
        /// The currently selected project
        /// </summary>
        Model.Project CurrentProject { get; set; }

        /// <summary>
        /// Backlog of the current project
        /// </summary>
        ReorderableCollection<Model.Backlog> ProjectBacklogs { get; }

        /// <summary>
        /// The currently selected backlog
        /// </summary>
        Model.Backlog CurrentBacklog { get; set; }

        /// <summary>
        /// Stories of the current backlog
        /// </summary>
        ReorderableCollection<Model.Story> BacklogStories { get; }

        /// <summary>
        /// Try to get a new API Key
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        Task<bool> GetApiKey(string Username, string Password);

        /// <summary>
        /// Formerly, this is a log out from the Core view
        /// </summary>
        /// <returns></returns>
        void ClearApiKey();

        void RefreshBacklogStories();

        /// <summary>
        /// Contains all stories that are not estimated
        /// </summary>
        ObservableCollection<Model.Story> NotEstimatedStories { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storiesToDelete"></param>
        void DeleteStories(Model.Story[] storiesToDelete);

        /// <summary>
        /// Update story status
        /// </summary>
        /// <param name="story"></param>
        /// <param name="newStatus"></param>
        Task<bool> SetStoriesStatus(Model.Story story, Model.StoryStatus newStatus);

        /// <summary>
        /// Update number of points of a story
        /// </summary>
        /// <param name="story"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        Task<bool> SetStoryPoints(Model.Story story, int points);

        /// <summary>
        /// Contains all main backlogs from all organizations
        /// </summary>
        ObservableCollection<Model.Backlog> MainBacklogs { get; }

        bool IsInProgress { get; }

        Task<bool> UpdateStory(Model.Story editStory);

        #region Commands
        ICommand RefreshBacklogCommand { get; }
        ICommand RefreshHomeCommand { get; }
        ICommand RefreshProjectCommand { get; }
        ICommand RefreshOrganzationCommand { get; }

        #endregion

        void MoveStoriesToBacklog(Model.Backlog backlogTarget, List<Model.Story> draggedStories);
    }
}
