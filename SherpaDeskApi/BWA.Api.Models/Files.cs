// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com



﻿using bigWebApps.bigWebDesk.Data;
using Micajah.AzureFileService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Dal;

namespace BWA.Api.Models
{
   /// <summary>
   /// Summary description for TicketLogRecords
   /// </summary>
    [DataContract(Name = "Files")]
    public class Files : Collection<Micajah.AzureFileService.File>
    {
        #region Members

        public const string OrganizationLogoObjectType = "organization-logo";
        public const string InstanceLogoObjectType = "instance-logo";
        public const string ProfileImageObjectType = "profile-image";

        protected Collection<Micajah.AzureFileService.File> _files;

        public List<File> List = null;

        #endregion

        public Files(Collection<Micajah.AzureFileService.File> Files)
        {
            if (Files != null && Files.Count > 0)
            {
                List = new List<File>();
                foreach (Micajah.AzureFileService.File file in Files)
                {
                    List.Add(new File(file));
                }
            }
        }

        #region Private Methods

        private static string GetContainerName(Guid containerId, bool publicAccess)
        {
            string containerName = (publicAccess ? string.Format(CultureInfo.InvariantCulture, "{0:N}p", containerId) : containerId.ToString("N"));
            return containerName;
        }

        public static bool IsPublicAccess(string objectType)
        {
            bool publicAccess = false;

            switch (objectType)
            {
                case "tickets-tickets-files":
                    publicAccess = true;
                    break;
                case OrganizationLogoObjectType:
                    publicAccess = true;
                    break;
                case InstanceLogoObjectType:
                    publicAccess = true;
                    break;
                case ProfileImageObjectType:
                    publicAccess = true;
                    break;
            }

            return publicAccess;
        }

        private static string GetOrganizationLogoUrl(Guid organizationId, int width, int height)
        {
            string url = null;
            string objectId = organizationId.ToString("N");

            FileManager fileManager = CreateFileManager(organizationId, OrganizationLogoObjectType, objectId);

            Collection<Micajah.AzureFileService.File> files = fileManager.GetFiles();
            if (files.Count > 0)
            {
                Micajah.AzureFileService.File file = files[0];

                url = fileManager.GetThumbnailUrl(file.FileId, width, height, 0, true);
            }

            return url;
        }

        public static string GetInstanceLogoUrl(Guid instanceId, int width, int height)
        {
            string url = null;
            string objectId = instanceId.ToString("N");

            FileManager fileManager = CreateFileManager(instanceId, InstanceLogoObjectType, objectId);

            Collection<Micajah.AzureFileService.File> files = fileManager.GetFiles();
            if (files.Count > 0)
            {
                Micajah.AzureFileService.File file = files[0];

                url = fileManager.GetThumbnailUrl(file.FileId, width, height, 0, true);
            }

            return url;
        }

        #endregion

        public static void CreateContainer(Guid containerId, bool publicAccess)
        {
            string containerName = GetContainerName(containerId, publicAccess);

            ContainerManager.CreateContainer(containerName, publicAccess);
        }

        public static void CreateInstanceContainers(Guid instanceId)
        {
            CreateContainer(instanceId, true);
            CreateContainer(instanceId, false);
        }

        public static void CreateOrganizationAndInstanceContainers(Guid organizationId)
        {
            CreateContainer(organizationId, true);

            ClientDataSet.InstanceDataTable table = InstanceProvider.GetInstances(organizationId);

            foreach (ClientDataSet.InstanceRow row in table)
            {
                CreateInstanceContainers(row.InstanceId);
            }
        }

        public static List<File> GetFiles(Guid instanceId, int objectId, Guid organizationId = default(Guid), string objectType = "tickets-tickets-files")
        {
            FileManager fileManager = Models.Files.CreateFileManager(instanceId, objectType, objectId.ToString());
            return new Files(fileManager.GetFiles()).List;
        }

        public static List<FileItem> GetTicketFiles(Guid instanceId, int TicketId, string [] namesFilter)
        {
            FileManager fileManager = Models.Files.CreateFileManager(instanceId, "tickets-tickets-files", TicketId.ToString());
            Collection<Micajah.AzureFileService.File> files = fileManager.GetFiles();

            List<FileItem> filesResult = new List<FileItem>();

            for (int i = 0; i < namesFilter.Length; i++)
            {
                string name = namesFilter[i];

                for (int j = 0; j < files.Count; j++)
                {
                    Micajah.AzureFileService.File file = files[j];
                    if (name == file.Name)
                    {
                        filesResult.Add(new FileItem(0, file.Name, (int)file.Length, file.LastModified, file.Url, fileManager.GetFile(file.FileId)));

                        break;
                    }
                }
            }

            return filesResult;
        }

        public static FileManager CreateFileManager(Guid instanceId, string objectType, string objectId)
        {
            bool publicAccess = IsPublicAccess(objectType);
            string containerName = GetContainerName(instanceId, publicAccess);

            FileManager fileManager = new FileManager(containerName, publicAccess, objectType, objectId);

            return fileManager;
        }

        public static void UploadFileToFileService(Guid instanceId, string object_id, string object_type, FileItem file, Guid organizationId = default(Guid), int userId = 0)
        {
            bool publicAccess = IsPublicAccess(object_type);
            string containerName = GetContainerName(instanceId, publicAccess);

            FileManager fileManager = new FileManager(containerName, publicAccess, object_type, object_id);
            fileManager.UploadFile(file.Name, file.ContentType, file.Data);
        }

        public static File GetFile(Guid instanceId, int ticketId, string file_id, string objectType = "tickets-tickets-files")
        {
            if (string.IsNullOrEmpty(file_id))
            {
                var files = GetFiles(instanceId, ticketId, default(Guid), objectType);
                if (files.Count > 0)
                    return files[0];
            }
            FileManager fileManager = Models.Files.CreateFileManager(instanceId, objectType, ticketId.ToString());
            Micajah.AzureFileService.File file = fileManager.GetFileInfo(file_id);
            if (file != null)
                return new File(file);
            return null;
        }

        public static string GetAccountLogoUrl(Guid instanceId, int accountId, Guid organizationId = default(Guid))
        {
            FileManager fileManager = new FileManager(GetContainerName(instanceId, false), "accounts-accounts-logo", accountId.ToString());

            var files = fileManager.GetFiles();
            if (files.Count > 0)
            {
                return files.First().Url;
            }

            return string.Empty;
        }

        public static string GetOrganizationLogoUrl(Guid organizationId)
        {
            return GetOrganizationLogoUrl(organizationId, 300, 45);
        }

        public static string GetOrganizationLargeLogoUrl(Guid organizationId)
        {
            return GetOrganizationLogoUrl(organizationId, 900, 135);
        }

        public static string GetInstanceLogoUrl(Guid instanceId)
        {
            return GetInstanceLogoUrl(instanceId, 300, 45);
        }

        public static string GetInstanceLargeLogoUrl(Guid instanceId)
        {
            return GetInstanceLogoUrl(instanceId, 900, 135);
        }

        public static byte[] GetFileContent(Guid instanceId, int ticketId, string file_id, string objectType = "tickets-tickets-files")
        {
            FileManager fileManager = Models.Files.CreateFileManager(instanceId, objectType, ticketId.ToString());
            return fileManager.GetFile(file_id);
        }

        internal static void Delete(Guid instanceId, string ticketId, string file_id, string objectType = "tickets-tickets-files")
        {
            FileManager fileManager = Models.Files.CreateFileManager(instanceId, objectType, ticketId);
            fileManager.DeleteFile(file_id);
        }

        public static string GetThumbnailUrl(string containerName, string objectType, string objectId, int width, int height)
        {
            bool publicAccess = IsPublicAccess(objectType);

            string result = GetThumbnailUrl(containerName, publicAccess, objectType, objectId, width, height);

            return result;
        }

        private static string GetThumbnailUrl(string containerName, bool publicAccess, string objectType, string objectId, int width, int height)
        {
            string result = String.Empty;

            FileManager fileManager = new FileManager(containerName, publicAccess, objectType, objectId);

            Collection<Micajah.AzureFileService.File> files = fileManager.GetFiles();
            if (files.Count > 0)
            {
                result = fileManager.GetThumbnailUrl(files[0].FileId, width, height, 0, true);
            }

            return result;
        }
    }
}
