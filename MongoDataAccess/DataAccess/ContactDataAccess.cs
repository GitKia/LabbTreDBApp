using MongoDataAccess.Models;
using MongoDB.Driver;


namespace MongoDataAccess.DataAccess;

public class ContactDataAccess
{
    // koppling till databas

    private const string ConnectionString = "mongodb://127.0.0.1:27017";
    private const string DatabaseName = "contactdb";
    private const string ContactCollection = "contactInfo";


    private IMongoCollection<T> ConnectToMongo<T>(in string collection)
    {
        var client = new MongoClient(ConnectionString);
        var db = client.GetDatabase(DatabaseName);
        return db.GetCollection<T>(collection);
    }
    // CRUD för att läsa in alla kontakter 
    public async Task<List<ContactModel>> GetAllContacts()
    {
        var contactColletion = ConnectToMongo<ContactModel>(ContactCollection);
        var results = await contactColletion.FindAsync(_ => true);
        return results.ToList();
    }

    // CRUD metod för att skapa ny kontakt 
    public Task CreateContact(ContactModel contact)
    {
        var contactColletion = ConnectToMongo<ContactModel>(ContactCollection);
        return contactColletion.InsertOneAsync(contact);
    }

    // Crud metod för att updatera en kontakt 
    public Task UpdateContact(ContactModel contact)
    {
        var contactColletion = ConnectToMongo<ContactModel>(ContactCollection);
        var filter = Builders<ContactModel>.Filter.Eq("Id", contact.Id);
        return contactColletion.ReplaceOneAsync(filter, contact,new ReplaceOptions { IsUpsert = true });
    }

    // Crud metod för att ta bort 
    public Task DeleteContact(ContactModel contact)
    {
        var contactColletion = ConnectToMongo<ContactModel>(ContactCollection);
        return contactColletion.DeleteOneAsync(c => c.Id == contact.Id);
    }

    // egen kod för att läsa in användarens val, tillkallas sedan av programmet 
    public int inputController()
    {
        int val = 0;

        while (!Int32.TryParse(Console.ReadLine(), out val) || val < 1 || val > 5)
        {
            Console.WriteLine("Wrong Input");
            Console.WriteLine("-----------");
        }
        return val;
    }

    // Egen kod för uppdate 
    public async Task ConcatcUpdateControl()
    {
        string NameChange = "";
        string NewPhone = "";
        int choice = 0;
        int counter = 1;
        var contacts = await GetAllContacts();

        // hämtar en alla kontakter från databasen och lägger en sifra framför 
        foreach (var contact in contacts)
        {
            Console.WriteLine(counter + " " + contact.FullName);
            counter++;
        }

        Console.WriteLine("Choose Contact");
        Console.WriteLine("-----------");

        // vid fle anmatning sker om rätt nästa
        while (!Int32.TryParse(Console.ReadLine(), out choice) || choice > contacts.Count() || choice < 1)
        {
            Console.WriteLine("Input is invalid, try again");
            Console.WriteLine("-----------");
        }

        // Efterfrågan updatering på namn om rätt
        while (string.IsNullOrEmpty(NameChange))
        {
            Console.WriteLine("Insert FullName?");
            Console.WriteLine("-----------");
            NameChange = Console.ReadLine();
                                              // lägga till en icke null ?  
        }

        // läser in det valda kontakten och efterfrågan om nytt nummer
        contacts[choice - 1].FullName = NameChange;
        while (string.IsNullOrEmpty(NewPhone))
        {
            Console.WriteLine("Insert Phone Nr:");
            Console.WriteLine("-----------");
            NewPhone = Console.ReadLine();
        }
        contacts[choice - 1].Phone = NewPhone;
        await UpdateContact(contacts[choice - 1]);
        Console.WriteLine($"Contact updated\n");
        Console.WriteLine("-----------");
                                            // lägga till inte null ? 
    }


    // läser inte alla kontakter på samma sätt igen från databasen och lägger till ett nummer före
    public async Task DeleteOneContact()
    {
        int counter1 = 1;
        int choice = 0;
        var contacts = await GetAllContacts();
        foreach (var contact in contacts)
        {
            Console.WriteLine($"{counter1}. Name:{contact.FullName}\n" +
                              $"   Phone Nr: {contact.Phone}\n");
            counter1++;
        }
        Console.WriteLine("Input the number of the contact you like to remove");

        while (!Int32.TryParse(Console.ReadLine(), out choice) || choice > contacts.Count() || choice < 1)
        {
            Console.WriteLine("Input invalid, try again");
        }

        await DeleteContact(contacts[choice - 1]);
        Console.WriteLine($"Contact Deleted\n");
    }

}
