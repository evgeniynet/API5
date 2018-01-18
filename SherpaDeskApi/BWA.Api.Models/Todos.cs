// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Runtime.Serialization;
using bigWebApps.bigWebDesk;

namespace BWA.Api.Models
{
    [DataContract(Name = "Todos")]
    public class Todos : ModelItemCollectionGeneric<Todo>
    {
        public Todos(DataTable TodosTable) : base(TodosTable) { }

        public static List<Todo> GetTicketTodos(Guid organizationId, int departmentId, int ticketId, int projectId, bool allItemTypes, int assignedId, bool? is_completed)
        {
            Todos _ticketTodos = new Todos(bigWebApps.bigWebDesk.Data.ToDo.SelectToDoListAndItems(organizationId, departmentId, ticketId, projectId, assignedId, is_completed));
            if (allItemTypes)
                return _ticketTodos.ToList();
            else
                return _ticketTodos.Where(x => x.ItemType == 1 || x.ItemType == 2).ToList();
        }
    }
}
