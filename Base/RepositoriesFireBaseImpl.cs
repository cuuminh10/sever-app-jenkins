namespace gmc_api.Base
{
    public class RepositoriesFireBaseImpl//<T> : IReponsitoriesFireBase<T> where T : FireBaseID
    {
        /* private string collectionName;
          public FirestoreDb fireStoreDb;
          public RepositoriesFireBaseImpl(string CollectionName)
          {
              string filepath = "D:\\GMC\\gmc-api\\gmc_api\\gmcexperterp-firebase-adminsdk.json";
              Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);
              fireStoreDb = FirestoreDb.Create("gmcexperterp");
              collectionName = CollectionName;
          }
          public T Get(T record)
          {
              throw new NotImplementedException();
          }

          public List<T> GetAll()
          {
              throw new NotImplementedException();
          }

          public T Add(T record)
          {
              CollectionReference colRef = fireStoreDb.Collection(collectionName);
              DocumentReference doc = colRef.AddAsync(record).GetAwaiter().GetResult();
              record.Id = doc.Id;
              return record;
          }

          public bool Update(T record)
          {
              DocumentReference recordRef = fireStoreDb.Collection(collectionName).Document(record.Id);
              recordRef.SetAsync(record, SetOptions.MergeAll).GetAwaiter().GetResult();
              return true;
          }

          public bool Delete(T record)
          {
              DocumentReference recordRef = fireStoreDb.Collection(collectionName).Document(record.Id);
              recordRef.DeleteAsync().GetAwaiter().GetResult();
              return true;
          }*/
    }
}
