﻿using APPartment.UI.Attributes;
using APPartment.UI.Utilities.Constants;
using APPartment.UI.ViewModels.Base;
using System.ComponentModel.DataAnnotations;
using APPChore = APPartment.Data.Server.Models.Chore.Chore;

namespace APPartment.UI.ViewModels.Chore
{
    [IMapFrom(typeof(APPChore))]
    public class ChorePostViewModel : PostViewModelWithHome
    {
        [APPUIHint(Templates.Hidden)]
        [Display(Name = "Assigned to")]
        public long? AssignedToUserId { get; set; }
        [APPUIHint(Templates.Boolean)]
        public bool IsDone { get; set; }
    }
}
