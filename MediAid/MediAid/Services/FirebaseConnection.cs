using MediAid.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MediAid.Services
{
    public interface FirebaseConnection
    {

        void Init();

        void Connect();
        Task<bool> CreateUser(string username, string password);
        Task<bool> LoginUser(string username, string password);

        void SetData(string json, params string[] childs);
        void SetData(IDictionary dictionary, params string[] childs);
        void AddReminder(Reminder reminder);

    }
}
