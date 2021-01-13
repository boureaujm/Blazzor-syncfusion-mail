using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Limilabs.Client.SMTP;
using mailBlazzorApp.Library.Dto;
using mailBlazzorApp.Library.Queries;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.WebUtilities;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Notifications;
using Syncfusion.Blazor.Popups;
using Syncfusion.Blazor.RichTextEditor;
using Syncfusion.Blazor.Spinner;


namespace BlazorApp.Pages
{
    public partial class InBoxRazor : ComponentBase
    {
        [Inject] private IMediator _mediator { get; set; }
        [Inject] private NavigationManager navigationManager { get; set; }

        public InBoxRazor()
        {
            ToastPosition = "Right";
        }

        [Parameter] public string FolderName { get; set; }

        [Parameter] public bool Loading { get; set; }

        // grid
        [Parameter] public Mail CurrentMail { get; set; }
        [Parameter] public SfRichTextEditor MailContent { get; set; }
        [Parameter] public IEnumerable<Mail> Mails { get; set; }
        [Parameter] public SfSpinner SpinnerObj { get; set; }
        [Parameter] public SfGrid<Mail> Grid { get; set; }

        // toast
        [Parameter] public SfToast ToastObj { get; set; }
        [Parameter] public string ToastPosition { get; set; }

        // action
        [Parameter] public SfDialog Alert { get; set; }
        [Parameter] public bool AlertIsVisible { get; set; } = false;

        // new mail
        [Parameter] public bool NewMailVisibility { get; set; } = false;
        [Parameter] public SfTextBox NewMailSubjectObj { get; set; }
        [Parameter] public SfTextBox NewMailRecipientObj { get; set; }
        [Parameter] public SfRichTextEditor NewMailContentObj { get; set; }

        [Parameter] public string NewMailSubject { get; set; }
        [Parameter] public string NewMailRecipient { get; set; }
        [Parameter] public string NewMailContent { get; set; }

        // grid

        protected void RowSelecthandler(RowSelectEventArgs<Mail> Args)
        {
            CurrentMail = Args.Data;
        }

        protected override async Task OnParametersSetAsync()
        {
            //the param will be set now
            await FetchMail(FolderName);
        }
        
        public override Task SetParametersAsync(ParameterView parameters)
        {
            foreach(var param in parameters)
            {
                Console.WriteLine($"Parameter: {param.Name}, Value: {param.Value?.ToString()}");
            }
            return base.SetParametersAsync(parameters);
        }
        
        protected override async Task OnInitializedAsync()
        {
            var uri = navigationManager.ToAbsoluteUri(navigationManager.Uri); //you can use IURIHelper for older versions

            if(QueryHelpers.ParseQuery(uri.Query).TryGetValue("FolderName", out var token))
            {
                FolderName = token.First();
            }
        }

        
        // action
        protected async Task Refresh(MouseEventArgs args)
        {
            await FetchMail(FolderName);
        }

        private async Task FetchMail(string folderName)
        {
            if (folderName != null)
            {
                CurrentMail = null;
                Loading = true;
                Mails = await _mediator.Send(new GetMailQuery(folderName));
                this.StateHasChanged();
            }
        }

        protected async Task Delete(MouseEventArgs args)
        {
            var selectedRecords = await this.Grid.GetSelectedRecords();

            if (selectedRecords.Count == 0)
            {
                ShowAlert("Avertissement", "Veuillez sélectioner au moins une ligne");
            }
            else
            {
                var uids = new List<long>();
                foreach (var record  in selectedRecords)
                {
                    var uid = record.Uid;
                    uids.Add(uid);
                }
                await _mediator.Send(new DeleteMailCommand(this.FolderName, uids.ToArray()));
            }
           
        }

        protected void ShowAlert(string title, string content)
        {
            Alert.Content = content;
            Alert.Header = title;
            Alert.Visible = true;
            AlertIsVisible = true;
        }

        protected void CloseDialog()
        {
            this.AlertIsVisible = false;
        }

        // new mail

        protected async Task SendBtnclicked()
        {
            NewMailContent = NewMailContentObj.Value;

            var newMail = new Mail
            {
                Subject = NewMailSubject,
                Sender = "jm.boureau@e.email",
                Html = NewMailContent,
                Recipient = NewMailRecipient
            };
            var result = await _mediator.Send(new SendMailQuery(newMail));

            if (result.Status == SendMessageStatus.Success)
            {
                this.ToastObj.Title = "Succès";
                this.ToastObj.Content = "Message envoyé";
                this.ToastObj.CssClass = "e-toast-success";
            }
            else
            {
                this.ToastObj.Title = "Ereur";
                this.ToastObj.Content = "Erreur lors de l'envoi du message";
                this.ToastObj.CssClass = "e-toast-danger";
            }

            await this.ToastObj.Show();

            NewMailDialogClose();
        }

        protected void ShowModal()
        {
            this.NewMailVisibility = true;
        }

        protected void NewMailDialogClose()
        {
            this.NewMailVisibility = false;
        }

        // menu

        protected List<ToolbarItemModel> Tools = new List<ToolbarItemModel>()
        {
            new ToolbarItemModel() {Command = ToolbarCommand.Bold},
            new ToolbarItemModel() {Command = ToolbarCommand.Italic},
            new ToolbarItemModel() {Command = ToolbarCommand.Underline},
            new ToolbarItemModel() {Command = ToolbarCommand.StrikeThrough},
            new ToolbarItemModel() {Command = ToolbarCommand.FontName},
            new ToolbarItemModel() {Command = ToolbarCommand.FontSize},
            new ToolbarItemModel() {Command = ToolbarCommand.FontColor},
            new ToolbarItemModel() {Command = ToolbarCommand.BackgroundColor},
            new ToolbarItemModel() {Command = ToolbarCommand.LowerCase},
            new ToolbarItemModel() {Command = ToolbarCommand.UpperCase},
            new ToolbarItemModel() {Command = ToolbarCommand.SuperScript},
            new ToolbarItemModel() {Command = ToolbarCommand.SubScript},
            new ToolbarItemModel() {Command = ToolbarCommand.Separator},
            new ToolbarItemModel() {Command = ToolbarCommand.Formats},
            new ToolbarItemModel() {Command = ToolbarCommand.Alignments},
            new ToolbarItemModel() {Command = ToolbarCommand.OrderedList},
            new ToolbarItemModel() {Command = ToolbarCommand.UnorderedList},
            new ToolbarItemModel() {Command = ToolbarCommand.Outdent},
            new ToolbarItemModel() {Command = ToolbarCommand.Indent},
            new ToolbarItemModel() {Command = ToolbarCommand.Separator},
            new ToolbarItemModel() {Command = ToolbarCommand.CreateLink},
            new ToolbarItemModel() {Command = ToolbarCommand.Image},
            new ToolbarItemModel() {Command = ToolbarCommand.CreateTable},
            new ToolbarItemModel() {Command = ToolbarCommand.Separator},
            new ToolbarItemModel() {Command = ToolbarCommand.ClearFormat},
            new ToolbarItemModel() {Command = ToolbarCommand.Print},
            new ToolbarItemModel() {Command = ToolbarCommand.SourceCode},
            new ToolbarItemModel() {Command = ToolbarCommand.Separator},
            new ToolbarItemModel() {Command = ToolbarCommand.Undo},
            new ToolbarItemModel() {Command = ToolbarCommand.Redo}
        };
    }
}