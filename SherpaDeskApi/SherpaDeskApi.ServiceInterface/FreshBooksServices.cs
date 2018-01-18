// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using ServiceStack;
using SherpaDeskApi.ServiceModel;
using System.Net;
using System.Data;
using System;

namespace SherpaDeskApi.ServiceInterface
{
    public class FreshBooksServices : Service
    {
        [Secure()]
        public object Get(FB_Clients request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);

            return request.FilteredResult<bigWebApps.bigWebDesk.Data.FBClient>(FBClients.GetFBClients(instanceConfig, request.page, request.limit));
        }

        [Secure()]
        public object Get(FB_Staff request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);

            return request.FilteredResult<bigWebApps.bigWebDesk.Data.FBStaff>(FBStaffs.GetFBStaff(instanceConfig, request.page, request.limit));
        }

        [Secure()]
        public object Get(FB_Projects request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);

            int clientID = 0;
            if (!int.TryParse(request.client, out clientID))
            {
                throw new HttpError(HttpStatusCode.NotFound, "incorrect client id");
            }
            int staffID = 0;
            if (!int.TryParse(request.staff, out staffID))
            {
                throw new HttpError(HttpStatusCode.NotFound, "incorrect staff id");
            }

            return request.FilteredResult<bigWebApps.bigWebDesk.Data.FBProject>(FBProjects.GetFBProjects(instanceConfig, request.page, request.limit, clientID, staffID));
        }

        [Secure()]
        public object Get(FB_Tasks request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);

            if (request.project.HasValue)
            {
                return request.FilteredResult<bigWebApps.bigWebDesk.Data.FBTask>(FBTasks.GetFBTasks(instanceConfig, request.page, request.limit, request.project.Value));
            }
            else if (!string.IsNullOrEmpty(request.name))
                return FBTasks.FindFBTask(instanceConfig, request.name);
            throw new HttpError(HttpStatusCode.NotFound, "incorrect id or name");
        }

        [Secure()]
        public object Post(FB_Data request)
        {
            ApiUser hdUser = request.ApiUser;
            try
            {
                FBModel.UpdateData(hdUser, request.user_id, request.fb_staff_id, request.account_id, request.fb_client_id,
                    request.project_id, request.fb_project_id, request.task_type_id, request.fb_task_type_id,
                    request.category_id, request.fb_category_id);
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
            return new HttpResult("", HttpStatusCode.OK);
        }

        [Secure()]
        public object Post(FB_Time request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            bool isProjectLog = false;
            string notes = "";
            int staffID = 0;
            DateTime date = DateTime.MinValue;
            if (request.is_project_log.HasValue)
            {
                isProjectLog = request.is_project_log.Value;
            }
            if (request.notes != null)
            {
                notes = request.notes;
            }
            if (request.fb_staff_id.HasValue)
            {
                staffID = request.fb_staff_id.Value;
            }
            if (request.date.HasValue)
            {
                date = request.date.Value;
            }
            try
            {
                string result = FBTimeEntries.CreateTimeEntry(hdUser, instanceConfig, staffID, request.fb_project_id, request.fb_task_type_id,
                    request.hours, notes, date, request.time_id, isProjectLog, 0);
                if (result == "ok")
                {
                    return new HttpResult("", HttpStatusCode.OK);
                }
                else
                {
                    throw new HttpError(Utils.ClearString(result));
                }
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        [Secure()]
        public object Put(FB_Time_Update request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            string notes = "";
            int staffID = 0;
            DateTime date = DateTime.MinValue;
            if (request.notes != null)
            {
                notes = request.notes;
            }
            if (request.fb_staff_id.HasValue)
            {
                staffID = request.fb_staff_id.Value;
            }
            if (request.date.HasValue)
            {
                date = request.date.Value;
            }
            try
            {
                string result = FBTimeEntries.CreateTimeEntry(hdUser, instanceConfig, staffID, request.fb_project_id, request.fb_task_type_id,
                    request.hours, notes, date, 0, false, request.key);
                if (result == "ok")
                {
                    return new HttpResult("", HttpStatusCode.OK);
                }
                else
                {
                    throw new HttpError(Utils.ClearString(result));
                }
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        [Secure()]
        public object Delete(FB_Time_Delete request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            bool isProjectLog = false;
            if (request.is_project_log.HasValue)
            {
                isProjectLog = request.is_project_log.Value;
            }
            if (request.is_unlink)
            {
                try
                {
                    FBTimeEntries.UnlinkFreshBooksTimeEntry(hdUser, request.key, isProjectLog);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
            else
            {
                try
                {
                    FBTimeEntries.DeleteFreshBooksTimeEntry(hdUser, instanceConfig, request.key, isProjectLog);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
        }

        [Secure()]
        public object Post(FB_Clients_Create request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            try
            {
                return FBClients.CreateClient(hdUser, instanceConfig, request.account_id);
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        [Secure()]
        public object Post(FB_Tasks_Create request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            try
            {
                return FBTasks.CreateTask(hdUser, instanceConfig, request.task_type_id, request.fb_project_id);
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        [Secure()]
        public object Post(FB_Projects_Create request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            try
            {
                return FBProjects.CreateProject(hdUser, instanceConfig, request.project_id, request.fb_staff_id,
                    request.fb_client_id, request.account_id);
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        [Secure()]
        public object Post(FB_Expense request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            string notes = "";
            int projectID = 0;
            int clientID = 0;
            string vendor = "";
            DateTime date = DateTime.MinValue;
            if (request.notes != null)
            {
                notes = request.notes;
            }
            if (request.fb_project_id.HasValue)
            {
                projectID = request.fb_project_id.Value;
            }
            if (request.fb_client_id.HasValue)
            {
                clientID = request.fb_client_id.Value;
            }
            if (request.date.HasValue)
            {
                date = request.date.Value;
            }
            if (request.vendor != null)
            {
                vendor = request.vendor;
            }
            try
            {
                string result = FBExpenses.CreateExpense(hdUser, instanceConfig, request.fb_staff_id, projectID, request.fb_category_id,
                    clientID, request.amount, vendor, notes, date, request.expense_id, 0);
                if (result == "ok")
                {
                    return new HttpResult("", HttpStatusCode.OK);
                }
                else
                {
                    throw new HttpError(Utils.ClearString(result));
                }
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        [Secure()]
        public object Put(FB_Expense_Update request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            string notes = "";
            int projectID = 0;
            int clientID = 0;
            string vendor = "";
            DateTime date = DateTime.MinValue;
            if (request.notes != null)
            {
                notes = request.notes;
            }
            if (request.fb_project_id.HasValue)
            {
                projectID = request.fb_project_id.Value;
            }
            if (request.fb_client_id.HasValue)
            {
                clientID = request.fb_client_id.Value;
            }
            if (request.date.HasValue)
            {
                date = request.date.Value;
            }
            if (request.vendor != null)
            {
                vendor = request.vendor;
            }
            try
            {
                string result = FBExpenses.CreateExpense(hdUser, instanceConfig, request.fb_staff_id, projectID, request.fb_category_id,
                    clientID, request.amount, vendor, notes, date, "", request.key);
                if (result == "ok")
                {
                    return new HttpResult("", HttpStatusCode.OK);
                }
                else
                {
                    throw new HttpError(Utils.ClearString(result));
                }
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        [Secure()]
        public object Delete(FB_Expense_Delete request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            if (request.is_unlink)
            {
                try
                {
                    FBExpenses.UnlinkFreshBooksExpense(hdUser, request.key);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
            else
            {
                try
                {
                    FBExpenses.DeleteFreshBooksExpense(hdUser, instanceConfig, request.key);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
        }
        
        
        [Secure()]
        public object Post(FB_Category_Create request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            try
            {
                return FBCategories.CreateCategory(hdUser, instanceConfig, request.category_id);
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        [Secure()]
        public object Delete(FB_Client_Delete request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            if (request.is_unlink)
            {
                try
                {
                    FBClients.UnlinkFreshBooksExpense(hdUser, request.key);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
            else
            {
                try
                {
                    FBClients.DeleteFreshBooksExpense(hdUser, instanceConfig, request.key);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
        }

        [Secure()]
        public object Delete(FB_Category_Delete request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            if (request.is_unlink)
            {
                try
                {
                    FBCategories.UnlinkFreshBooksCategory(hdUser, request.key);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
            else
            {
                try
                {
                    FBCategories.DeleteFreshBooksCategory(hdUser, instanceConfig, request.key);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
        }

        [Secure()]
        public object Get(FB_Categories request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);

            return FBCategories.GetFBCategories(instanceConfig);
        }

        [Secure()]
        public object Delete(FB_Task_Delete request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            if (request.is_unlink)
            {
                try
                {
                    FBTasks.UnlinkFreshBooksTask(hdUser, request.key);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
            else
            {
                try
                {
                    FBTasks.DeleteFreshBooksTask(hdUser, instanceConfig, request.key);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
        }

        [Secure()]
        public object Delete(FB_Staff_Delete request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            try
            {
                FBStaffs.UnlinkFreshBooksStaff(hdUser, request.key);
                return new HttpResult("", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        [Secure()]
        public object Delete(FB_Project_Delete request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            if (request.is_unlink)
            {
                try
                {
                    FBProjects.UnlinkFreshBooksProject(hdUser, request.key);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
            else
            {
                try
                {
                    FBProjects.DeleteFreshBooksProject(hdUser, instanceConfig, request.key);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
        }
    }
}
