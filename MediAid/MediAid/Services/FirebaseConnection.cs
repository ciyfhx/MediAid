using MediAid.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MediAid.Services
{
    public abstract class FirebaseConnection
    {


        public bool IsLogin { get; set; } = false;
        public event OnRemindersUpdate ReminderUpdate;

        public abstract Task<bool> CreateUser(string username, string password);
        public abstract Task<bool> LoginUserAsync(string username, string password);
        //public abstract  void LoginUser(string username, string password);

        //public abstract void SetData(string json, params string[] childs);
        //public abstract void SetData(IDictionary dictionary, params string[] childs);
        public abstract void AddReminder(Reminder reminder);
        public abstract void AddDrug(Drug drug);

    }

    public delegate void OnRemindersUpdate(Reminder reminder);

}
