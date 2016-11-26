namespace shared.callendpoints
{

    class digital_extension
    {
        public string extension { get; set; }
        public int portnbr { get; set;}

        public digital_extension(string extension, int portnbr)
        {
            this.extension = extension;
            this.portnbr = portnbr;
        }
    }
}
