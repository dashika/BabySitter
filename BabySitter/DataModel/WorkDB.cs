using SQLite;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Windows.Storage;

namespace BabySitter.DataModel
{

    public class WorkDB
    {
        String dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "db.db");

        public WorkDB()
        {
          //  this.DeleteTables();
            this.Init();
        }

        private async void Init()
        {
            var db = new SQLite.SQLiteAsyncConnection(dbPath);
            try
            {
                 db.CreateTableAsync<Children>();
                 db.CreateTableAsync<EveryDay>();

                 db.CreateTableAsync<Reminder>();
                 db.CreateTableAsync<Vaccine>();
                 db.CreateTableAsync<Doctor>();
                 db.CreateTableAsync<Action>();

                 db.CreateTableAsync<Task>();

                 db.CreateTableAsync<Favorite>();
                 db.CreateTableAsync<Contact>();
            }
            catch (System.NullReferenceException e)
            {
               
            }
        }

        private async void DeleteTables()
        {
            var db = new SQLite.SQLiteAsyncConnection(dbPath);
            try
            {
                 db.DropTableAsync<Children>();
                 db.DropTableAsync<EveryDay>();

                 db.DropTableAsync<Reminder>();

                 db.DropTableAsync<Task>();
                 db.DropTableAsync<Favorite>();
                 db.DropTableAsync<Contact>();
            }
            catch (System.NullReferenceException e)
            {

            }
        }

        #region EveryDay
        public async void InsertEveryDay(EveryDay ed)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            await db.InsertAsync(ed);
        }

        public async void UpdateEveryDay(EveryDay ed)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            await db.UpdateAsync(ed);
        }

        public async void DeleteEveryDay(EveryDay ed)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            await db.DeleteAsync(ed);
        }

        public async System.Threading.Tasks.Task<List<EveryDay>> SelectAllEveryDay(DateTime date, int child)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            var query = db.Table<EveryDay>().Where(x => x.date == date && x.id_child == child); //((x.date.Year == date.Year) & (x.date.Month == date.Month) & (x.date.Day == date.Day)));
            var result = await query.ToListAsync();
            return result;
        }

        public async System.Threading.Tasks.Task<List<EveryDay>> SelectEveryDay30(int child)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            var query = db.Table<EveryDay>().Where(x => x.id_child == child).OrderByDescending(i=>i.date).Take(30); //((x.date.Year == date.Year) & (x.date.Month == date.Month) & (x.date.Day == date.Day)));
            var result = await query.ToListAsync();
            return result;
        }

        public async System.Threading.Tasks.Task<List<EveryDay>> SelectEveryDay(int id)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            var query = db.Table<EveryDay>().Where(x => x.id == id);
            var result = await query.ToListAsync();
            return result;
        }
        #endregion

        #region Reminder
        private async void InsertReminder(Reminder ed)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            await db.InsertAsync(ed);
        }

        public async void InsertReminderVaccine(Vaccine ed, DateTime dt, String str)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            var res =  await db.InsertAsync(ed);

            Reminder rem = new Reminder();
            rem.id_child = CurrentChild.id;
            rem.dateTime = dt;
            rem.type = ed.id;
            rem.stype = "Прививка";
            rem.other = str;
            InsertReminder(rem);
        }

        public async void InsertReminderDoctor(Doctor ed, DateTime dt, String str)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            var res = await db.InsertAsync(ed);

            Reminder rem = new Reminder();
            rem.id_child = CurrentChild.id;
            rem.dateTime = dt;
            rem.type = ed.id;
            rem.stype = "Прием врача";
            rem.other = str;
            InsertReminder(rem);
        }

        public async void InsertReminderAction(Action ed, DateTime dt, String str)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            var res = await db.InsertAsync(ed);

            Reminder rem = new Reminder();
            rem.id_child = CurrentChild.id;
            rem.dateTime = dt;
            rem.type = ed.id;
            rem.stype = "Мероприятие";
            rem.other = str;
            InsertReminder(rem);
        }

        public async void UpdateReminderA(Action ed, DateTime dt, String str, Reminder r)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            int id = await db.InsertAsync(ed);
            r.dateTime = dt;
            r.other = str;
            r.stype = "Мероприятие";
            r.type = ed.id;
            await db.UpdateAsync(r);
        }

        public async void UpdateReminderV(Vaccine ed, DateTime dt, String str, Reminder r)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            int id = await db.InsertAsync(ed);
            r.dateTime = dt;
            r.other = str;
            r.stype = "Прививка";
            r.type = ed.id;
            await db.UpdateAsync(r);
        }

        public async void UpdateReminderD(Doctor ed, DateTime dt, String str, Reminder r)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            int id = await db.InsertAsync(ed);
            r.dateTime = dt;
            r.other = str;
            r.stype = "Прием врача";
            r.type = ed.id;
            await db.UpdateAsync(r);
        }

        public async void DeleteReminder(Reminder ed)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            await db.DeleteAsync(ed);
        }

        public async void DeleteReminderA(int id)
        {
            Action ed = await SelectAction(id);
            var db = new SQLiteAsyncConnection(dbPath);
            await db.DeleteAsync(ed);
        }

        public async void DeleteReminderV(int id)
        {
            Vaccine ed = await SelectVaccine(id);
            var db = new SQLiteAsyncConnection(dbPath);
            await db.DeleteAsync(ed);
        }

        public async void DeleteReminderD(int id)
        {
            Doctor ed = await SelectDoctor(id);
            var db = new SQLiteAsyncConnection(dbPath);
            await db.DeleteAsync(ed);
        }

        public async System.Threading.Tasks.Task<List<Reminder>> SelectAllReminder(int child)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            var query = db.Table<Reminder>().Where(x => x.id_child == child); //((x.date.Year == date.Year) & (x.date.Month == date.Month) & (x.date.Day == date.Day)));
            var result = await query.ToListAsync();
            return result;
        }


        public async System.Threading.Tasks.Task<List<Reminder>> SelectReminder(int id, int child)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            var query = db.Table<Reminder>().Where(x => x.id == id && x.id_child == child);
            var result = await query.ToListAsync();
            return result;
        }

        public async System.Threading.Tasks.Task<Vaccine> SelectVaccine(int id)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            var query = db.Table<Vaccine>().Where(x => x.id == id);
            var result = await query.FirstOrDefaultAsync();
            return result;
        }

        public async System.Threading.Tasks.Task<Doctor> SelectDoctor(int id)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            var query = db.Table<Doctor>().Where(x => x.id == id);
            var result = await query.FirstOrDefaultAsync();
            return result;
        }

        public async System.Threading.Tasks.Task<Action> SelectAction(int id)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            var query = db.Table<Action>().Where(x => x.id == id);
            var result = await query.FirstOrDefaultAsync();
            return result;
        }
        #endregion

        #region Task
        public async void InsertTask(Task t)
        {
            var db = new SQLiteAsyncConnection(dbPath);

            await db.InsertAsync(t);
        }

        public async void UpdateTask(Task ed)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            await db.UpdateAsync(ed);
        }

        public async void DeleteEveryTask(Task ed)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            await db.DeleteAsync(ed);
        }

        public async System.Threading.Tasks.Task<List<Task>> SelectAllTask(int child)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            var query = db.Table<Task>().Where(x => x.id_child == child); //((x.date.Year == date.Year) & (x.date.Month == date.Month) & (x.date.Day == date.Day)));
            var result = await query.ToListAsync();
            return result;
        }

        public async System.Threading.Tasks.Task<List<Task>> SelectTask(int id)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            var query = db.Table<Task>().Where(x => x.id == id);
            var result = await query.ToListAsync();
            return result;
        }
        #endregion

        #region Favorite
        public async void InsertFavorite(Favorite t)
        {
            var db = new SQLiteAsyncConnection(dbPath);

            await db.InsertAsync(t);
        }

        public async void UpdateFavorite(Favorite ed)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            await db.UpdateAsync(ed);
        }

        public async void DeleteFavorite(Favorite ed)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            await db.DeleteAsync(ed);
        }

        public async System.Threading.Tasks.Task<List<Favorite>> SelectFavorite(int child)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            var query = db.Table<Favorite>().Where(x => x.id_child == child);
            var result = await query.ToListAsync();
            return result;
        }
        #endregion

        #region Contact
        public async void InsertContact(Contact t)
        {
            var db = new SQLiteAsyncConnection(dbPath);

            await db.InsertAsync(t);
        }

        public async void UpdateContact(Contact ed)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            await db.UpdateAsync(ed);
        }

        public async void DeleteContact(Contact ed)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            await db.DeleteAsync(ed);
        }

        public async System.Threading.Tasks.Task<List<Contact>> SelectContact(int child)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            var query = db.Table<Contact>().Where(x => x.id_child == child);
            var result = await query.ToListAsync();
            return result;
        }
        #endregion


        #region Child
        internal async System.Threading.Tasks.Task<List<Children>> SelectAllChildren()
        {
            var db = new SQLiteAsyncConnection(dbPath);
            var query = db.Table<Children>();
            var result = await query.ToListAsync();
            return result;
        }

        internal Children SelectChildName(int id)
        {
            var db = new SQLiteConnection(dbPath);
            var query = db.Table<Children>().Where(x => x.id == id).FirstOrDefault();

            return query;
        }

        public async System.Threading.Tasks.Task<int> InsertChildren(Children t, int r = 0)
        {
            var db = new SQLiteAsyncConnection(dbPath);

            await db.InsertAsync(t);

            return t.id;
        }

        public async void InsertChildren(Children t)
        {
            var db = new SQLiteAsyncConnection(dbPath);

            await db.InsertAsync(t);
        }

        public async void UpdateChildren(Children ed)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            await db.UpdateAsync(ed);
        }

        public async void DeleteChildren(Children ed)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            await db.DeleteAsync(ed);
        }

        #endregion
    }
}
