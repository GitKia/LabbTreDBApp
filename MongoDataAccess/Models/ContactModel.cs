using MongoDB.Bson.Serialization.Attributes;

namespace MongoDataAccess.Models;


public  class ContactModel
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]

    public string Id { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }

   public string ContactInformation => $"{FullName}, {Phone}";
}
 