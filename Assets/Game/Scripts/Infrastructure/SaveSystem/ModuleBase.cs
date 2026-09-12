namespace Infrastructure.SaveSystem
{
    public interface IModule
    {
        object ReadData(string dir);
        void ApplyData(object data);
        void Save(string dir);
        void Reset();
    }
    
    public abstract class ModuleBase<TData> : IModule where TData : class, new()
    {
        public TData Data { get; protected set; } = new();

        protected virtual string FileName => $"{typeof(TData).Name}.msgpack";

        public object ReadData(string dir) => SaveService.LoadData<TData>(dir, FileName);

        public void ApplyData(object data)
        {
            Data = (TData)data;
            OnAfterLoad();
        }

        public void Reset() => ApplyData(new TData());

        public virtual void Save(string dir)
        {
            OnBeforeSave();
            SaveService.SaveData(dir, FileName, Data);
        }

        protected virtual void OnAfterLoad() { }
        protected virtual void OnBeforeSave() { }
    }
}