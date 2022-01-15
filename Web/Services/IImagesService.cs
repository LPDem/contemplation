namespace Contemplation.Services
{
    public interface IImagesService
    {
        string[] GetAll();

        byte[] GetFull(string name, int? width = null, int? height = null);
    }
}
