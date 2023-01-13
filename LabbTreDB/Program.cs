using Microsoft.VisualBasic;
using MongoDataAccess.DataAccess;
using MongoDataAccess.Models;
using MongoDB.Bson;
using MongoDB.Driver;

ContactDataAccess db = new ContactDataAccess();

while (true) 
{

    //Hård kodning för att fylla databasen

    //await db.CreateContact(new ContactModel()
    //{
    //    FullName = "adam sonson",
    //    Phone = "0737887878"
    //});


    //await db.CreateContact(new ContactModel()
    //{
    //    FullName = "Adams kompis ",
    //    Phone = "073 788 78 79"
    //});


    Console.WriteLine("><><><><><><><><><><><><><>");
    Console.WriteLine("1. New Contact\n2. Show Contacts\n3. Update Contact" +
    "\n4. Delete Contac\n5. Close App");                                       
    Console.WriteLine("><><><><><><><><><><><><><>");


    int UserInput = db.inputController();


    // En swith med båda kod och metoder 
    switch (UserInput)
    {
        case 1:
            string name = "";
            string phone = "";

            while (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Input Name:");
                Console.WriteLine("-----------");

                name = Console.ReadLine();
            }
            while (string.IsNullOrEmpty(phone))
            {
                Console.WriteLine("Input Phone Nr:");
                Console.WriteLine("-----------");
                phone = Console.ReadLine();
            }

            var contact = new ContactModel { FullName = name, Phone = phone };
            await db.CreateContact(contact);
            Console.WriteLine("Contact Added\n");
            Console.WriteLine("-----------");
            break;


        case 2:
            int counter = 1;
            var ContectCollection = await db.GetAllContacts();
            foreach (var Contact in ContectCollection)
            {
                Console.WriteLine($"{counter}. Name:{Contact.FullName}\n" +
                                  $"   Phone Nr:: {Contact.Phone}\n");
                counter++;
            }
            Console.WriteLine("\n");

            break;

        //anropar metoden från ContactDataAccess
        case 3:
            await db.ConcatcUpdateControl();

            break;
        //anropar metoden från ContactDataAccess
        case 4:
            await db.DeleteOneContact();

            break;
        case 5:
            Environment.Exit(1);
            break;

        default:
            break;
    }
}