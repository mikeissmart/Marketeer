namespace Marketeer.Common.Configs
{
    public abstract class PythonConfig
    {
        public string ScriptFolder { get; set; }
        public string DataFolder { get => Path.Combine(ScriptFolder, "Data"); }
    }
}
