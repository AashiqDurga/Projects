namespace CQRSKata
{
    public class CollectCommand
    {
        private Material _material;

        public CollectCommand(Material material)
        {
            _material = material;
        }

        public Material Material
        {
            set { _material = value; }
            get { return _material; }
        }
    }
}