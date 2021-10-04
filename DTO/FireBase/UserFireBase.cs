using gmc_api.Base.dto;
using Google.Cloud.Firestore;
using System;

namespace gmc_api.DTO.FireBase
{
    [FirestoreData]
    public class UserFireBase : FireBaseID
    {
        [FirestoreProperty]
        public string content { get; set; }
        [FirestoreProperty]
        public string sndUser { get; set; }
        [FirestoreProperty]
        public string type { get; set; }
        [FirestoreProperty]
        public int objectId { get; set; }
        [FirestoreProperty]
        public DateTime sndDate { get; set; }
    }
}
