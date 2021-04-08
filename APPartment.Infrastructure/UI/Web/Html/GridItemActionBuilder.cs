using Microsoft.AspNetCore.Mvc.Rendering;
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

        public static string BuildCustomAction(string currentArea, string currentController, string action, long modelID, string buttonColorClass, string faIconClass)
        {
            var href = string.Empty;

            if (currentArea.Equals("default"))
                href = $"href='/{currentArea}/{currentController}/{action}/{modelID}'";
            else
                href = $"href='/{currentController}/{action}/{modelID}?area='{currentArea}''";

            var button = $@"<a {href} class='no-underline btn {buttonColorClass} btn-xs btn-icon' title='{action}'><i class='{faIconClass}'></i></a>";
            return button;
        }

        public static string BuildCustomActionWithModal(string currentArea, string currentController, string action, long modelID, string buttonTitle, string modalTitle, string modalBody, string buttonColorClass, string faIconClass)
        {
            var href = string.Empty;

            if (currentArea.Equals("default"))
                href = $"href='/{currentArea}/{currentController}/{action}/{modelID}'";
            else
                href = $"href='/{currentController}/{action}/{modelID}?area='{currentArea}''";

            var modalID = $"{action}-{modelID}";
            var button = $@"<button type='button' title='{action}' class='btn {buttonColorClass} btn-xs btn-icon' data-toggle='modal' data-target='#{modalID}'><i class='{faIconClass}'></i></button>";
            var modal = $@"<div class='modal fade' id='{modalID}' tabindex='-1' role='dialog' aria-labelledby='label-{modalID}' aria-hidden='true'>
                                            <div class='modal-dialog' role='document'>
                                                <div class='modal-content'>
                                                    <div class='modal-header'>
                                                        <h5 class='modal-title' id='label-{modalID}'>{modalTitle}</h5>
                                                        <button type='button' class='close' data-dismiss='modal' aria-label='Close'>
                                                            <span aria-hidden='true'>&times;</span>
                                                        </button>
                                                    </div>
                                                    <div class='modal-body'>
                                                        {modalBody}
                                                    </div>
                                                    <div class='modal-footer'>
                                                        <button type='button' class='btn btn-secondary' data-dismiss='modal'>Cancel</button>
                                                        <a {href} class='no-underline btn btn-primary'>{buttonTitle}</a>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>";
            var buttonWithModal = button + modal;
            return buttonWithModal;
        }

        public static string BuildCustomActionWithDropdownWithModal(string currentArea, string currentController, string action, long modelID, string buttonTitle, string modalTitle, string buttonColorClass, string faIconClass, List<SelectListItem> options)
        {
            var href = string.Empty;

            if (currentArea.Equals("default"))
                href = $"href='/{currentArea}/{currentController}/{action}/{modelID}'";
            else
                href = $"href='/{currentController}/{action}/{modelID}?area='{currentArea}''";

            var modalID = $"{action}-{modelID}";
            var button = $@"<button type='button' title='{action}' class='btn {buttonColorClass} btn-xs btn-icon' data-toggle='modal' data-target='#{modalID}'><i class='{faIconClass}'></i></button>";

            var optionsHtml = string.Empty;

            foreach (var option in options)
            {
                optionsHtml += $"<option selected='{option.Selected}' value='{option.Value}'>{option.Text}</option>";
            }

            var modal = $@"<div class='modal fade' id='{modalID}' tabindex='-1' role='dialog' aria-labelledby='label-{modalID}' aria-hidden='true'>
                                            <div class='modal-dialog' role='document'>
                                                <div class='modal-content'>
                                                    <div class='modal-header'>
                                                        <h5 class='modal-title' id='label-{modalID}'>{modalTitle}</h5>
                                                        <button type='button' class='close' data-dismiss='modal' aria-label='Close'>
                                                            <span aria-hidden='true'>&times;</span>
                                                        </button>
                                                    </div>
                                                    <div class='modal-body'>
                                                        <div class'form-group'>
                                                            <select class='form-control form-control-solid' name='{action}'>
                                                                    {optionsHtml}
                                                            </select>
                                                        </div>
                                                    </div>
                                                    <div class='modal-footer'>
                                                        <button type='button' class='btn btn-secondary' data-dismiss='modal'>Cancel</button>
                                                        <button type='button' {href} class='no-underline btn btn-primary'>{buttonTitle}</a>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>";
            var buttonWithModal = button + modal;
            return buttonWithModal;
        }
    }
}