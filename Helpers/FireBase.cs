using gmc_api.DTO.FireBase;
using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace gmc_api.Helpers
{
    public class FireBase
    {
        readonly FirestoreDb db;

        public FireBase()
        {
            this.db = FirestoreDb.Create("gmcexperterp"); ;
        }

        public bool AddFireBase(string collection, int userId, UserFireBase userFireBase)
        {
            string id = db.Collection(collection).Document("notification").Collection(userId.ToString()).Document().Id;
            DocumentReference docRef = db.Collection(collection).Document("notification").Collection(userId.ToString()).Document(id);
            var json = JsonConvert.SerializeObject(userFireBase);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            docRef.SetAsync(dictionary);
            return true;
        }

        public bool UpdateReadFlagFireBase(string collection, int userId, string recordId)
        {
            DocumentReference docRef = db.Collection(collection).Document("notification").Collection(userId.ToString()).Document(recordId);
            Dictionary<string, object> updateData = new Dictionary<string, object>
            {
                { "readFlag", "true" }
            };
            docRef.UpdateAsync(updateData);
            return true;
        }
    }
}
