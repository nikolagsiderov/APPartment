using APPartment.Infrastructure.UI.Common.Constants;

namespace APPartment.Infrastructure.UI.Web.Html
{
    public class ObjectActionBuilder
    {
        public static string BuildDetailsAction(string currentArea, string currentController, long modelID)
        {
            var button = $@"<a href='/{currentArea}/{currentController}/Details/{modelID}' title='Details' class='no-underline btn btn-outline-info btn-xs'><i class='fas fa-info-circle'></i> Details</a>";
            return button;
        }

        public static string BuildEditAction(string currentArea, string currentController, long modelID)
        {
            var button = $@"<a href='/{currentArea}/{currentController}/Edit/{modelID}' title='Edit' class='no-underline btn btn-outline-primary btn-xs'><i class='fas fa-edit'></i> Edit</a>";
            return button;
        }

        public static string BuildDeleteAction(string currentArea, string currentController, long modelID)
        {
            var modalID = $"deleteModal-{modelID}";
            var button = $@"<button type='button' title='Delete' class='btn btn-outline-danger btn-xs' data-toggle='modal' data-target='#{modalID}'><i class='fas fa-trash-alt'></i> Delete</button>";
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

            if (!currentArea.Equals(Areas.Default))
                href = $"href='/{currentArea}/{currentController}/{action}/{modelID}'";
            else
                href = $"href='/{currentController}/{action}/{modelID}?area='{currentArea}''";

            var button = $@"<a {href} name='{action}' class='no-underline btn {buttonColorClass} btn-xs' title='{action}'><i class='{faIconClass}'></i> {action}</a>";
            return button;
        }
    }
}
