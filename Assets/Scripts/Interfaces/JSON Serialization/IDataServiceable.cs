public interface IDataServiceable
{
    bool SaveData<T>(string RelativePathj, T Data, bool Encrypted);

    T LoadData<T>(string RelativePathj, bool Encrypted);
}
