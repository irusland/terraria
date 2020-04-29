namespace terraria
{
    struct Texture
    {
        private int id;
        private int width, height;

        public int ID
        {
            get { return id; }
        }
        public int Width
        {
            get { return width; }
        }
        public int Height
        {
            get { return height; }
        }
        
        public Texture(int id, int width, int height)
        {
            this.id = id;
            this.width = width;
            this.height = height;
        }
    }
}