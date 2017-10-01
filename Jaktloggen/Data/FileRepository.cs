using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Jaktloggen.Helpers;
using Jaktloggen.Interfaces;
using Jaktloggen.IO;
using Jaktloggen.Models;
using Jaktloggen.Models.Extensions;
using Jaktloggen.Services;
using Xamarin.Forms;

namespace Jaktloggen.Data
{
    public class FileRepository : IDataRepository
    {
        private static string EXT = ".json";
        private static string FILE_DOG = "dogs" + EXT;
        private static string FILE_JAKT = "jakt" + EXT;
        private static string FILE_JEGER = "jegere" + EXT;
        private static string FILE_LOGG = "logger" + EXT;
        private static string FILE_SELECTED_ARTIDS = "selectedartids" + EXT;
        private static string FILE_SELECTED_LOGGTYPEIDS = "selectedloggtypeids" + EXT;
        private static string FILE_MY_ART = "myspecies" + EXT;

        private static string FILE_ART = "arter.xml";
        private static string FILE_LOGGTYPE_GROUP = "loggtypegroup.xml";
        private static string FILE_LOGGTYPER = "loggtyper.xml";
        private static string FILE_ARTGROUP = "artgroup.xml";
        

        private List<Jakt> JaktList = new List<Jakt>();
        private List<Logg> LoggList = new List<Logg>();
        private List<Jeger> JegerList = new List<Jeger>();
        private List<Dog> DogList = new List<Dog>();
        private List<Art> ArtList = new List<Art>();
        private List<ArtGroup> ArtGroupList = new List<ArtGroup>();
        private List<Art> SelectedArtList = new List<Art>();
        private List<LoggType> LoggTypeList = new List<LoggType>();
        private List<LoggTypeGroup> LoggTypeGroupList = new List<LoggTypeGroup>();
        private List<LoggType> SelectedLoggTypeList = new List<LoggType>();
        
        public FileRepository()
        {
            
        }

        public void Init()
        {
            //todo Make Async and show activityindicator

            //copy xml files on first load(should it be here?)
            if (!LocalFileStorage.Exists(FILE_ARTGROUP))
            {
                LocalFileStorage.CopyToAppFolder(FILE_ARTGROUP);
            }

            if (!LocalFileStorage.Exists(FILE_ART))
            {
                LocalFileStorage.CopyToAppFolder(FILE_ART);
            }

            if (!LocalFileStorage.Exists(FILE_LOGGTYPE_GROUP))
            {
                LocalFileStorage.CopyToAppFolder(FILE_LOGGTYPE_GROUP);
            }

            if (!LocalFileStorage.Exists(FILE_LOGGTYPER))
            {
                LocalFileStorage.CopyToAppFolder(FILE_LOGGTYPER);
            }
            
            GetJegere();
            GetArter();
            GetDogs();
            GetJakts();
            GetLoggs();
            GetSelectedArter();
            GetSelectedLoggTyper();
        }

        /* GET ALL */
        public List<T> GetEntity<T>(string filename) where T : class, IEntity
        {
            var localList = LocalFileStorage.LoadFromLocalStorage<List<T>>(filename);
            var remoteList = new List<T>();

            if (App.SyncWithServer)
            {
                remoteList = DataService.LoadFromServer<T>(filename);

                if (remoteList != null && remoteList.Any())
                {
                    if (localList == null || !localList.Any() || remoteList.Max(r => r.Changed) > localList.Max(l => l.Changed))
                    {
                        //remote er nyere enn local
                        remoteList.Save(filename);
                        localList = remoteList;
                    }
                    else if (!remoteList.Any() || remoteList.Max(r => r.Changed) < localList.Max(l => l.Changed))
                    {
                        //remote er eldre enn local
                        localList.UploadToServer(filename);
                    }
                }
                else
                {
                    localList.UploadToServer(filename);
                }
            }
            return localList;
        }

        
        public IEnumerable<Jakt> GetJakts()
        {
            var filename = FILE_JAKT;
            if (!JaktList.Any())
            {
                JaktList = GetEntity<Jakt>(filename);
            }
            return JaktList.OrderByDescending(o => o.ID);
        }

        public IEnumerable<Logg> GetLoggs()
        {
            var filename = FILE_LOGG;
            if (!LoggList.Any())
            {
                LoggList = GetEntity<Logg>(filename);
            }
            return LoggList.OrderByDescending(o => o.ID);
        }

        
        public IEnumerable<Jeger> GetJegere()
        {
            var filename = FILE_JEGER;
            if (!JegerList.Any())
            {
                JegerList = GetEntity<Jeger>(filename);
            }
            return JegerList.OrderByDescending(o => o.ID);
        }
        public IEnumerable<Dog> GetDogs()
        {
            var filename = FILE_DOG;
            if (!DogList.Any())
            {
                DogList = GetEntity<Dog>(filename);
            }
            return DogList.OrderByDescending(o => o.ID); ;
        }
        public IEnumerable<Art> GetSelectedArter()
        {
            var filename = FILE_SELECTED_ARTIDS;
            if (!SelectedArtList.Any())
            {
                SelectedArtList = GetEntity<Art>(filename);
            }
            return SelectedArtList;
        }
        public List<LoggType> GetSelectedLoggTyper()
        {
            var filename = FILE_SELECTED_LOGGTYPEIDS;
            if (!SelectedLoggTypeList.Any())
            {
                SelectedLoggTypeList = GetEntity<LoggType>(filename);
            }
            return SelectedLoggTypeList;
        }
        public IEnumerable<Art> GetArter()
        {
            ArtList = LocalFileStorage.LoadFromLocalStorage<List<Art>>(FILE_ART);
            GetSelectedArter();
            foreach (var art in SelectedArtList)
            {
                var artToSelect = ArtList.FirstOrDefault(a => a.ID == art.ID);
                if (artToSelect != null)
                {
                    artToSelect.Selected = true;
                }

            }
            //ArtList.AddRange(LocalFileStorage.Load<List<Art>>(FILE_MY_ART)); //TODO: add this functionality (egne arter)
            return ArtList;
        }
        public IEnumerable<ArtGroup> GetArtGroups()
        {
            ArtGroupList = LocalFileStorage.LoadFromLocalStorage<List<ArtGroup>>(FILE_ARTGROUP);
            return ArtGroupList;
        }
        
        public IEnumerable<LoggType> GetLoggTyper()
        {
            LoggTypeList = LocalFileStorage.LoadFromLocalStorage<List<LoggType>>(FILE_LOGGTYPER);
            return LoggTypeList;
        }
        public IEnumerable<LoggTypeGroup> GetLoggTypeGroups()
        {
            LoggTypeGroupList = LocalFileStorage.LoadFromLocalStorage<List<LoggTypeGroup>>(FILE_LOGGTYPE_GROUP);
            return LoggTypeGroupList;
        }


        /* GET */
        public Jakt GetJakt(int id)
        {
            return JaktList.SingleOrDefault(x => x.ID == id);
        }
        public Logg GetLogg(int id)
        {
            return LoggList.FirstOrDefault(x => x.ID == id);
        }

        public Jeger GetJeger(int id)
        {
            return JegerList.FirstOrDefault(x => x.ID == id);
        }

        public Dog GetDog(int id)
        {
            return DogList.FirstOrDefault(x => x.ID == id);
        }
        public Art GetArt(int id)
        {
            if (!ArtList.Any())
            {
                GetArter();
            }
            return ArtList.FirstOrDefault(x => x.ID == id);
        }
        public LoggType GetLoggType(string key)
        {
            return LoggTypeList.FirstOrDefault(x => x.Key == key);
        }

        /* SAVE */


        //public int SaveEntity<T>(T entity, List<T> entityList, string filename) where T : class, IEntity
        //{
        //    entity.Changed = DateTime.Now;
        //    if (entity.ID <= 0)
        //    {
        //        entity.ID = entityList.Any() ? entityList.Max(i => i.ID) + 1 : 1;
        //        entity.Created = DateTime.Now;
        //        entityList.Add(entity);
        //    }
        //    else
        //    {
        //        var index = JaktList.FindIndex(x => x.ID == entity.ID);
        //        entityList[index] = entity;
        //    }

        //    entityList.Save(filename);

        //    return entity.ID;
        //}

        public int SaveJakt(Jakt jakt)
        {
            jakt.Changed = DateTime.Now;
            if (jakt.ID <= 0)
            {
                jakt.ID = JaktList.Any() ? JaktList.Max(i => i.ID) + 1 : 1;
                jakt.Created = DateTime.Now;
                JaktList.Add(jakt);
            }
            else
            {
                var index = JaktList.FindIndex(x => x.ID == jakt.ID);
                JaktList[index] = jakt;
            }

            JaktList.Save(FILE_JAKT);

            return jakt.ID;
        }
        
        public int SaveLogg(Logg logg)
        {
            logg.Changed = DateTime.Now;
            if (logg.ID <= 0)
            {
                logg.ID = LoggList.Any() ? LoggList.Max(i => i.ID) + 1 : 1;
                logg.Created = DateTime.Now;
                LoggList.Add(logg);
            }
            else
            {
                var index = LoggList.FindIndex(x => x.ID == logg.ID);
                LoggList[index] = logg;
            }

            LoggList.Save(FILE_LOGG);

            return logg.ID;
        }

        public int SaveJeger(Jeger jeger)
        {
            jeger.Changed = DateTime.Now;
            if (jeger.ID <= 0)
            {
                jeger.ID = JegerList.Any() ? JegerList.Max(i => i.ID) + 1 : 1;
                jeger.Created = DateTime.Now; ;
                JegerList.Add(jeger);
            }
            else
            {
                var index = JegerList.FindIndex(x => x.ID == jeger.ID);
                JegerList[index] = jeger;
            }

            JegerList.Save(FILE_JEGER);

            return jeger.ID;
        }

        public int SaveDog(Dog dog)
        {
            dog.Changed = DateTime.Now;
            if (dog.ID <= 0)
            {
                dog.ID = DogList.Any() ? DogList.Max(i => i.ID) + 1 : 1;
                dog.Created = DateTime.Now;
                DogList.Add(dog);
            }
            else
            {
                var index = DogList.FindIndex(x => x.ID == dog.ID);
                DogList[index] = dog;
            }

            DogList.Save(FILE_DOG);

            return dog.ID;
        }

        public int SaveArt(Art art)
        {
            art.Changed = DateTime.Now;
            if (art.ID <= 0)
            {
                art.ID = ArtList.Any() ? ArtList.Max(i => i.ID) + 1 : 1;
                art.Created = DateTime.Now;
                ArtList.Add(art);
            }
            else
            {
                var index = ArtList.FindIndex(x => x.ID == art.ID);
                ArtList[index] = art;
            }

            ArtList.Where(a => a.GroupId != 100).ToList().SaveToLocalStorage(FILE_ART);
            ArtList.Where(a => a.GroupId == 100).ToList().Save(FILE_MY_ART);

            return art.ID;
        }

        public void SaveLoggType(LoggType loggType)
        {
            var index = LoggTypeList.FindIndex(x => x.Key == loggType.Key);
            LoggTypeList[index] = loggType;

            LoggTypeList.Where(a => a.GroupId != 100).ToList().SaveToLocalStorage(FILE_LOGGTYPER);
        }
        /* DELETE */
        public void DeleteJakt(Jakt jakt)
        {
            while (LoggList.Any(j => j.JaktId == jakt.ID))
            {
                LoggList.Remove(LoggList.First(j => j.JaktId == jakt.ID));
            };

            JaktList.Remove(jakt);
            
            LoggList.Save(FILE_LOGG);
            JaktList.Save(FILE_JAKT);
        }

        public void DeleteJeger(Jeger jeger)
        {
            foreach (var logg in LoggList.Where(l => l.JegerId == jeger.ID))
            {
                logg.JegerId = 0;
            }
            foreach (var jakt in JaktList.Where(j => j.JegerIds.Contains(jeger.ID)))
            {
                jakt.JegerIds.Remove(jeger.ID);
            }
            JegerList.Remove(jeger);
            
            LoggList.Save(FILE_LOGG);
            JaktList.Save(FILE_JAKT);
            JegerList.Save(FILE_JEGER);
        }

        public void DeleteDog(Dog dog)
        {
            foreach (var logg in LoggList.Where(l => l.DogId == dog.ID))
            {
                logg.DogId = 0;
            }
            foreach (var jakt in JaktList.Where(j => j.DogIds.Contains(dog.ID)))
            {
                jakt.DogIds.Remove(dog.ID);
            }
            DogList.Remove(dog);
            
            LoggList.Save(FILE_LOGG);
            JaktList.Save(FILE_JAKT);
            DogList.Save(FILE_DOG);
        }

        public void DeleteArt(Art art)
        {
            foreach (var logg in LoggList.Where(l => l.ArtId == art.ID))
            {
                logg.Art = new Art();
            }

            ArtList.Remove(art);

            LoggList.Save(FILE_LOGG);
            ArtList.Where(a => a.GroupId != 100).ToList().SaveToLocalStorage(FILE_ART);
            ArtList.Where(a => a.GroupId == 100).ToList().Save(FILE_MY_ART);
        }

        public void DeleteLog(Logg logg)
        {
            LoggList.Remove(logg);
            LoggList.Save(FILE_LOGG);
        }


        /* MISC */
        public List<int> AddJegerToJakt(int jaktId, int jegerId)
        {
            var jegerIds = JaktList.Single(j => j.ID == jaktId).JegerIds;
            jegerIds.Add(jegerId);
            JaktList.Save(FILE_JAKT);
            return jegerIds;
        }
        public List<int> RemoveJegerFromJakt(int jaktId, int jegerId)
        {
            var jegerIds = JaktList.Single(j => j.ID == jaktId).JegerIds;
            jegerIds.Remove(jegerId);
            JaktList.Save(FILE_JAKT);
            return jegerIds;
        }
        public List<int> AddDogToJakt(int jaktId, int id)
        {
            var dogIds = JaktList.Single(j => j.ID == jaktId).DogIds;
            dogIds.Add(id);
            JaktList.Save(FILE_JAKT);
            return dogIds;
        }
        public List<int> RemoveDogFromJakt(int jaktId, int id)
        {
            var dogIds = JaktList.Single(j => j.ID == jaktId).DogIds;
            dogIds.Remove(id);
            JaktList.Save(FILE_JAKT);
            return dogIds;
        }
        public void AddSelectedArt(Art art)
        {
            if (SelectedArtList.All(a => a.ID != art.ID))
            {
                SelectedArtList.Add(art);
                SelectedArtList.Save(FILE_SELECTED_ARTIDS);
            }
        }
        public void RemoveSelectedArt(Art art)
        {
            var artToRemove = SelectedArtList.SingleOrDefault(a => a.ID == art.ID);
            if (artToRemove != null)
            {
                SelectedArtList.Remove(artToRemove);
                SelectedArtList.Save(FILE_SELECTED_ARTIDS);
            }
            
        }
        public void AddSelectedLoggType(LoggType loggType)//todo change to store only key
        {
            if (SelectedLoggTypeList.All(l => l.Key != loggType.Key))
            {
                SelectedLoggTypeList.Add(loggType);
                SelectedLoggTypeList.Save(FILE_SELECTED_LOGGTYPEIDS);
            }
        }
        public void RemoveSelectedLoggType(string loggTypeKey)
        {
            var itemToRemove = SelectedLoggTypeList.SingleOrDefault(l => l.Key == loggTypeKey);
            if (itemToRemove != null)
            {
                SelectedLoggTypeList.Remove(itemToRemove);
                SelectedLoggTypeList.Save(FILE_SELECTED_LOGGTYPEIDS);
            }
        }
    }
}