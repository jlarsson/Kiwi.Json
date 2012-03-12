using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public class EsentInstance : IEsentInstance
    {
        public EsentInstance(IEsentDatabase database, Instance instance)
        {
            Database = database;
            Instance = instance;
        }

        #region IEsentInstance Members

        public void Dispose()
        {
            Instance.Dispose();
        }

        public IEsentDatabase Database { get; protected set; }
        public Instance Instance { get; protected set; }

        public IEsentSession CreateSession()
        {
            return new EsentSession(this, new Session(Instance));
        }

        #endregion
    }
}