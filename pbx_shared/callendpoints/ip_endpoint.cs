namespace shared.callendpoints
{
    class ip_endpoint
    {
        public System.Net.IPAddress ipaddress { get; set; }

        public ip_endpoint(System.Net.IPAddress ipaddress)
        {
            this.ipaddress = ipaddress;
        }
    }
}
