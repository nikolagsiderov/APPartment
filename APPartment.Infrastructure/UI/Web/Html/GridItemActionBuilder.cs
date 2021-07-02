using APPartment.Infrastructure.UI.Common.Constants;
using System.Collections.Generic;

namespace APPartment.Infrastructure.UI.Web.Html
{
    public static class GridItemActionBuilder
    {
        public static string BuildDetailsAction(string currentArea, string currentController, long modelID)
        {
            var button = $@"<a href='/{currentArea}/{currentController}/Details/{modelID}' title='Details' class='no-underline btn btn-outline-info btn-xs btn-icon'><i class='fas fa-info-circle'></i></a>";
            return button;
        }

        public static string BuildEditAction(string currentArea, string currentController, long modelID)
        {
            var button = $@"<a href='/{currentArea}/{currentController}/Edit/{modelID}' title='Edit' class='no-underline btn btn-outline-primary btn-xs btn-icon'><i class='fas fa-edit'></i></a>";
            return button;
        }

        public static string BuildDeleteAction(string currentArea, string currentController, long modelID)
        {
            var modalID = $"deleteModal-{modelID}";
            var button = $@"<button type='button' title='Delete' class='btn btn-outline-danger btn-xs btn-icon' data-toggle='modal' data-target='#{modalID}'><i class='fas fa-trash-alt'></i></button>";
            var modal = $@"<div class='modal fade' id='{modalID}' tabindex='-1' role='dialog' aria-labelledby='deleteObjectModalLabel' aria-hidden='true'>
                                            <div class='modal-dialog' role='document'>
                                                <div class='modal-content'>
                                                    <div class='modal-header'>
                                                        <h5 class='modal-title' id='deleteObjectModalLabel'>Confirm Deletion</h5>
                                                        <button type='button' class='close' data-dismiss='modal' aria-label='Close'>
                                                            <span aria-hidden='true'>&times;</span>
                                                        </button>
                                                    </div>
                                                    <div class='modal-body'>
                                                        Are you sure you want to delete this item?
                                                    </div>
                                                    <div class='modal-footer'>
                                                        <button type='button' class='btn btn-secondary' data-dismiss='modal'>Cancel</button>
                                                        <a href='/{currentArea}/{currentController}/Delete/{modelID}' class='no-underline btn btn-danger'>Delete</a>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>";
            var buttonWithModal = button + modal;
            return buttonWithModal;
        }

        public static string BuildCustomAction(string currentArea, string currentController, string action, long modelID, string buttonColorClass, string faIconClass, string buttonName = null, bool isModal = false, bool isForDropdown = false)
        {
            var button = string.Empty;
            var href = $"href='/{currentArea}/{currentController}/{action}/{modelID}'";

            if (string.IsNullOrEmpty(buttonName))
                buttonName = action;

            if (isModal)
            {
                if (isForDropdown)
                    button = $@"<button {href} name='{action}' class='no-underline btn btn-xs dropdown-item' title='{buttonName}'><i class='{faIconClass}'></i> &nbsp;{buttonName}</button>";
                else
                    button = $@"<button {href} name='{action}' class='no-underline btn {buttonColorClass} btn-xs btn-icon' title='{buttonName}'><i class='{faIconClass}'></i></button>";
            }
            else
            {
                if (isForDropdown)
                    button = $@"<a {href} name='{action}' class='no-underline btn {buttonColorClass} btn-xs dropdown-item' title='{buttonName}'><i class='{faIconClass}'></i> &nbsp;{buttonName}</a>";
                else
                    button = $@"<a {href} name='{action}' class='no-underline btn {buttonColorClass} btn-xs btn-icon' title='{buttonName}'><i class='{faIconClass}'></i></a>";
            }

            return button;
        }

        public static string BuildDropdownButton(long modelID)
        {
            var button = $@"<div class='dropdown'><button class='btn btn-outline-info btn-xs btn-icon' type='button' id='dropdownButtonForModelID-{modelID}' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'><i class='fas fa-ellipsis-v'></i></button>";
            return button;
        }

        public static string BuildDropdownButton(long modelID, string groupName, string title = null, string buttonColorClass = null)
        {
            if (string.IsNullOrEmpty(buttonColorClass))
                buttonColorClass = "btn-outline-info";

            var button = $@"<br/><div class='dropdown'><button class='btn {buttonColorClass} btn-xs' title='{title}' type='button' id='dropdownButtonForModelID-{modelID}' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'><i class='fas fa-ellipsis-v'></i> &nbsp;{groupName}</button>";
            return button;
        }

        public static string PopulateDropdownButton(long modelID, string dropdownButton, IEnumerable<string> actions)
        {
            var populatedButton = dropdownButton + $@"<div class='dropdown-menu' aria-labelledby='dropdownButtonForModelID-{modelID}'>{string.Join(" ", actions)}</div></div>";
            return populatedButton;
        }
    }
}