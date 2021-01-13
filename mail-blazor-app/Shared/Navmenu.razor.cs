using System.Collections.Generic;
using System.Threading.Tasks;
using mailBlazzorApp.Library.Dto;
using mailBlazzorApp.Library.Queries;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorApp.Shared
{
    public partial class NavMenuRazor : ComponentBase
    {
        [Inject] private IMediator _mediator  { get; set; }
        [Inject] private NavigationManager NavigationManager  { get; set; }

      
        [Parameter]  public bool CollapseNavMenu { get; set; }
        
        [Parameter] public  IEnumerable<Folder> Folders { get; set; }


        public NavMenuRazor()
        {
            CollapseNavMenu = true;
        }    
        

        private string NavMenuCssClass => CollapseNavMenu ? "collapse" : null;

        protected void ToggleNavMenu()
        {
            CollapseNavMenu = !CollapseNavMenu;
        }

        // protected void HandleClick(string folderName)
        // {
        //     NavigationManager.NavigateTo("/getmail/FolderName=" + folderName, true);
        // }
        
        protected override async Task OnInitializedAsync()
        {
            await FetchMail(null);
        }
        
        protected  async Task FetchMail(MouseEventArgs args)
        {
            Folders = await _mediator.Send(new GetFoldersQuery());
        }
    
    }
}