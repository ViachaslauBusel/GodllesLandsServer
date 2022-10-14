namespace Protocol.Data
{
    [MessageObject]
    public struct ServerInfo
    {
        public string Name { get;  set; }
        public string IP { get;  set; }
        public int Port { get;  set; }
    }
}