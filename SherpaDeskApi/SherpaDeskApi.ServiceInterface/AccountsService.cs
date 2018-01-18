// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using ServiceStack;
using SherpaDeskApi.ServiceModel;
using System.Collections.Generic;
using System.Net;
using ServiceStack.Host;

namespace SherpaDeskApi.ServiceInterface
{
    public class AccountsService : Service
    {
        [Secure("super")]
        public object Get(Accounts request)
        {
			if (request.c)
				return GetAccounts(request);
			var query = base.Request.QueryString;
			string search = query["search"];
			if (!string.IsNullOrWhiteSpace (search)) {
				search = search.Trim ();
				if (search == "*"){
                    search = "";
                    query = System.Web.HttpUtility.ParseQueryString(query.ToString());
					query.Remove ("search");
				}
                request.search = search;
			}
	
			if (string.IsNullOrWhiteSpace (query["search"])) {
				return base.RequestContext.ToOptimizedResultUsingCache (base.Cache, string.Format ("urn:{0}:{1}{2}", base.Request.GetBasicAuth (), base.Request.PathInfo.Substring (1), (query.Count > 0 ? ":" + query.ToString () : "")),
					new System.TimeSpan (2, 0, 0), () => {
					return GetAccounts (request);
				});
			}
            else
                return GetAccounts(request);
        }

        private object GetAccounts(Accounts request)
		{
			ApiUser hdUser = request.ApiUser;
			//if (!string.IsNullOrWhiteSpace(request.search))

			return request.QueryResult<Models.Account>(Models.Accounts.SearchAccounts(hdUser, request.search ?? "", request.is_with_statistics ?? true, request.is_open_tickets ?? false, request.is_watch_info ?? false, request.is_locations_info ?? false, request.page, request.limit));
			//if (!hdUser.IsTechAdmin)
			//	return request.FilteredResult<Models.Account>(Models.Accounts.GetUserAccounts(hdUser.OrganizationId, hdUser.DepartmentId, request.user ?? hdUser.UserId, request.is_with_statistics ?? true));
			//return request.FilteredResult<Models.Account>(Models.Accounts.GetAllAccounts(hdUser.OrganizationId, hdUser.DepartmentId, request.user ?? hdUser.UserId, request.is_with_statistics ?? true));
		}

        [Secure("tech")]
        public object Get(Account request)
        {
            ApiUser hdUser = request.ApiUser;
            if (request.account_id == 0)
                throw new HttpError(HttpStatusCode.NotFound, "Incorrect account id");
            return Models.Account_Details.GetAccountDetails(hdUser, request.account_id, request.is_with_statistics ?? true); 
        }

        [Secure("tech")]
        public object Put(Account request)
        {
            ApiUser hdUser = request.ApiUser;
            if (request.account_id == 0)
                throw new HttpError(HttpStatusCode.NotFound, "Incorrect account id");
			Models.Account.SaveNote (hdUser.OrganizationId, hdUser.DepartmentId, request.account_id, request.note ?? "");
		    return new HttpResult("", HttpStatusCode.OK);
        }

		/*
void BindData()
        {
            DataTable dtCustomFields = Data.CustomFields.SelectTicketCustomFields(User.lngDId, -1);


            if (dtCustomFields != null && dtCustomFields.Rows.Count > 0)
            {
                Table tblForm = new Table();
                tblForm.CellSpacing = tblForm.CellPadding = 2;

                foreach (DataRow rCustomField in dtCustomFields.Rows)
                {
                    TableRow rFormItem = new TableRow();

                    TableCell cCaption = new TableCell();
                    TableCell cInputControl = new TableCell();

                    Literal ltrCaption = new Literal();
                    if (rCustomField["Caption"] != DBNull.Value)
                        ltrCaption.Text = (string)rCustomField["Caption"];
                    if (rCustomField["Required"] != DBNull.Value)
                        if ((bool)rCustomField["Required"])
                            ltrCaption.Text += "*";
                    ltrCaption.Text += ":";

                    switch ((Data.CustomFields.Type)(byte)rCustomField["Type"])
                    {
                        case Data.CustomFields.Type.Checkboxes:

                            CheckBoxList chblCheckboxes = new CheckBoxList();

                            if (rCustomField["Choices"] != DBNull.Value)
                            {
                                chblCheckboxes.DataSource = ((string)rCustomField["Choices"]).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                                if (rCustomField["DefaultValue"] != DBNull.Value)
                                {
                                    foreach (string selection in ((string)rCustomField["DefaultValue"]).Split(','))
                                    {
                                        string sel = selection.Trim();
                                        foreach (ListItem listItem in chblCheckboxes.Items)
                                            if (sel == listItem.Value)
                                                listItem.Selected = true;
                                    }
                                }
                            }
                            cInputControl.Controls.Add(chblCheckboxes);
                            break;

                        case Data.CustomFields.Type.DropDown:
                            DropDownList ddlDropDown = new DropDownList();

                            if (rCustomField["Choices"] != DBNull.Value)
                            {
                                ddlDropDown.DataSource = ((string)rCustomField["Choices"]).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                                ddlDropDown.DataBind();
                                if (rCustomField["DefaultValue"] != DBNull.Value)
                                    try
                                    {
                                        ddlDropDown.SelectedValue = (string)rCustomField["DefaultValue"];
                                    }
                                    catch { }

                                if (ddlDropDown.Items.Count > 0)
                                    ddlDropDown.Items.Insert(0, new ListItem("(choose below)", ""));
                            }
                            cInputControl.Controls.Add(ddlDropDown);
                            break;

                        case Data.CustomFields.Type.TextArea:
                            TextBox ctlTextArea = new TextBox();
                            ctlTextArea.TextMode = TextBoxMode.MultiLine;
                            ctlTextArea.Width = inputFieldWidth;
                            if (rCustomField["DefaultValue"] != DBNull.Value)
                                ctlTextArea.Text = (string)rCustomField["DefaultValue"];
                            cInputControl.Controls.Add(ctlTextArea);
                            break;

                        case Data.CustomFields.Type.TextBox:
                            TextBox ctlTextBox = new TextBox();
                            ctlTextBox.Width = inputFieldWidth;
                            if (rCustomField["DefaultValue"] != DBNull.Value)
                                ctlTextBox.Text = (string)rCustomField["DefaultValue"];
                            cInputControl.Controls.Add(ctlTextBox);
                            break;
                        case Data.CustomFields.Type.DateTime:
                            DateTimePicker _dateBox = (DateTimePicker)LoadControl("~/classes/Controls/DateTimePicker.ascx");
                            if (_dateBox != null)
                            {
                                if (rCustomField["DefaultValue"] != DBNull.Value)
                                {
                                    DateTime defDate = DateTime.MinValue;
                                    if (DateTime.TryParse(rCustomField["DefaultValue"].ToString(), null, System.Globalization.DateTimeStyles.RoundtripKind, out defDate) && defDate > DateTime.MinValue)
                                        _dateBox.SelectedDate = Functions.DB2UserDateTime(defDate);
                                }
                                cInputControl.Controls.Add(_dateBox);
                            }
                            break;
                    }

        }

        protected void apCustomFields_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            Data.CustomFields.Move(User.lngDId, aCustomFieldId, dCustomFieldId);
         }
		 */

    }
}
