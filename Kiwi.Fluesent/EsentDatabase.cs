using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public class EsentDatabase : IEsentDatabase
    {
        public string Path { get; protected set; }

        public EsentDatabase(string path)
        {
            Path = path;
        }

        public IEsentInstance CreateInstance(string name, string displayName, InitGrbit grbit)
        {
            var instance = new Instance(name, displayName);
            var folder = System.IO.Path.GetDirectoryName(Path);
            instance.Parameters.CircularLog = true;
            instance.Parameters.CleanupMismatchedLogFiles = true;
            instance.Parameters.CreatePathIfNotExist = true;
            instance.Parameters.AlternateDatabaseRecoveryDirectory = folder;
            instance.Parameters.LogFileDirectory = folder;
            instance.Parameters.SystemDirectory = folder;
            instance.Parameters.TempDirectory = folder;

            instance.Init(grbit);

            return new EsentInstance(this, instance);
        }
    }
}