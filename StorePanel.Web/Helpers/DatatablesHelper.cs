using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.Helpers
{
    public class DataTable
    {
        //public DataTable(List<> columns,List<Command> commands)
        //{
            
        //}
        public TableConfig Config { get; set; }
        public List<Column> Columns { get; set; }
        public List<Command> Commands { get; set; }
    }

    public class TableConfig
    {
        public int OrderIndex { get; set; } = 0;
        public string OrderType { get; set; } = "Desc";
    }
    public class Column
    {
        public Column(string rowIdentifier)
        {
            this.RowIdentifier = rowIdentifier;
        }
        public Column(string rowIdentifier,string rowName)
        {
            this.RowIdentifier = rowIdentifier;
            this.RowName = rowName;
        }
        public string RowIdentifier { get; set; }

        public string RowName
        {
            get { return String.IsNullOrEmpty(RowName) ? RowIdentifier : RowName; }
            set { RowName = value; }
        }

    }

    public enum ActionType
    {
        Delete= 0,
        Edit = 1,
        Other = 2
    }
    public class Command    
    {
        public Command(string controller, string action)
        {
            this.Controller = controller;
            this.Action = action;
        }
        public Command(string controller, string action,string commandName)
        {
            this.Controller = controller;
            this.Action = action;
            this.CommandName = commandName;
        }
        public Command(string controller, string action, string commandName,bool isModal)
        {
            this.Controller = controller;
            this.Action = action;
            this.CommandName = commandName;
            this.IsModal = isModal;
        }
        public Command(string controller, string action,ActionType actionType)
        {
            this.Controller = controller;
            this.Action = action;
            this.ActionType = actionType;
        }
        public Command(string controller, string action, ActionType actionType, bool isModal)
        {
            this.Controller = controller;
            this.Action = action;
            this.ActionType = actionType;
            this.IsModal = isModal;
        }
        public string CommandName
        {
            get { return string.IsNullOrEmpty(CommandName)? this.Action : CommandName; }
            set { CommandName = value; }
        }

        public bool IsModal { get; set; } = false;
        public string Controller { get; set; }
        public string Action { get; set; }
        public ActionType ActionType { get; set; } = ActionType.Other;

        public string AbsolutePath
        {
            get { return string.IsNullOrEmpty(AbsolutePath)? $"/dashboard/{Controller}/{Action}'": AbsolutePath; }
            set { AbsolutePath = value; }
        }

    }
}
