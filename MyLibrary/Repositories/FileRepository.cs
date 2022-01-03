using LiteDB;

namespace LaXiS.MyLibrary.Repositories
{
    public class FileRepository
    {
        // TODO should be disposed
        private readonly ILiteDatabase _db;
        private readonly ILiteCollection<Models.File> _files;

        public FileRepository()
        {
            _db = new LiteDatabase("mylibrary.db");
            _files = _db.GetCollection<Models.File>();
        }

        public void Add(Models.File file)
        {
            _files.Insert(file);
        }

        public bool Exists(Models.File file)
        {
            return _files.Count(f => f.Name == file.Name && f.Path == file.Path) > 0;
        }
    }
}
