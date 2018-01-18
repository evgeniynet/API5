// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System.Collections.Generic;
using System.Net;
using System;
using System.IO;
using bigWebApps.bigWebDesk.Data;
using System.Data;
using ServiceStack;
using SherpaDeskApi.ServiceModel;
using ServiceStack.Web;

namespace SherpaDeskApi.ServiceInterface
{
    public class FilesService : Service
    {
        [Secure()]
        public object Get(Files request)
        {
            ApiUser hdUser = request.ApiUser;
           //v2
            if (string.IsNullOrEmpty(request.ticket))
                throw new HttpError(HttpStatusCode.NotFound, "No ticket provided");
            int ticketId = Models.Ticket.GetId(hdUser.OrganizationId, hdUser.DepartmentId, request.ticket);
            if (ticketId <= 0)
                return new HttpResult(HttpStatusCode.NotFound, "ticket key must be correct.");
            return Models.Files.GetFiles(hdUser.InstanceId, ticketId, hdUser.OrganizationId);
        }

        [Secure()]
        public object Get(File request)
        {
            ApiUser hdUser = request.ApiUser;
            int ticketId = Models.Ticket.GetId(hdUser.OrganizationId, hdUser.DepartmentId, request.ticket);
            Models.File file = Models.Files.GetFile(hdUser.InstanceId, ticketId, request.file_id);

            if (request.is_link_only)
            {
                return file.Url;
            }
            var sfile = new MemoryStream(Models.Files.GetFileContent(hdUser.InstanceId, ticketId, file.FileId));
            string mimeType = file.ContentType ?? "text/html";
            return new FileResult(sfile, mimeType, file.Name);
        }

        [Secure()]
        public object Post(Files request)
        {
            ApiUser hdUser = request.ApiUser;
            int ticketId = Models.Ticket.GetId(hdUser.OrganizationId, hdUser.DepartmentId, request.ticket);
            if (ticketId <= 0)
            {
                return new HttpResult(HttpStatusCode.NotFound, "TicketId must be correct.");
            }
            string[] uploadedFileNames = new string[100];
            int filesCount = 0;
            foreach (var uploadedFile in RequestContext.Files)
            {
                if (uploadedFile.ContentLength <= 0) continue;
                using (var ms = new MemoryStream())
                {
                    uploadedFile.WriteTo(ms);
                    ms.Position = 0;
                    FileItem _fi = new FileItem(0, uploadedFile.FileName, (int)ms.Length, DateTime.UtcNow, string.Empty, ms.ToArray());
                    BWA.Api.Models.Files.UploadFileToFileService(hdUser.InstanceId, ticketId.ToString(), "tickets-tickets-files", _fi, hdUser.OrganizationId, hdUser.UserId);
                    uploadedFileNames[filesCount++] = uploadedFile.FileName;
                }
            }
            if (filesCount > 0)
            {
                Array.Resize(ref uploadedFileNames, filesCount);
                if (request.post_id > 0)
                    bigWebApps.bigWebDesk.Data.Ticket.UpdateLogMessageWithFileChanges(hdUser.OrganizationId, hdUser.DepartmentId, request.post_id, uploadedFileNames);
                else
                    bigWebApps.bigWebDesk.Data.Ticket.InsertLogMessage(hdUser.OrganizationId, hdUser.DepartmentId, hdUser.UserId, ticketId, uploadedFileNames);
            }
            return new HttpResult("", HttpStatusCode.OK);
        }

        public class FileResult : IHasOptions, IStreamWriter
        {
            private readonly Stream _responseStream;
            public IDictionary<string, string> Options { get; private set; }

            public FileResult(Stream responseStream, string contentType, string fileName)
            {
                _responseStream = responseStream;

                Options = new Dictionary<string, string> {
             {"Content-Type", contentType},
             {"Content-Disposition", "attachment; filename=\"" + fileName + "\";"}
                };
            }

            public void WriteTo(Stream responseStream)
            {
                if (_responseStream == null)
                    return;

                _responseStream.WriteTo(responseStream);
                responseStream.Flush();
            }
        }

        [Secure()]
        public object Delete(File request)
        {
            ApiUser hdUser = request.ApiUser;
            BWA.Api.Models.Files.Delete(hdUser.InstanceId, request.ticket, request.file_id);
            return new HttpResult("OK", HttpStatusCode.OK);
        }
    }
}
