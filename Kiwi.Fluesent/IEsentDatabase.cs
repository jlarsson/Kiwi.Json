using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public interface IEsentDatabase
    {
        string Path { get; }
        //IEsentInstance CreateInstance(string name, string displayName, InitGrbit grbit);
        IEsentSession CreateSession(bool attachAndOpenDatabase);
    }
}