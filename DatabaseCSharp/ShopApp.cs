using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCSharp
{
    internal class ShopApp
    {
        private readonly ShopDbContext dbContext;
        
        public ShopApp(ShopDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        internal void Init()
        {
            dbContext.Database.EnsureCreated();
            if (dbContext.Products.Count() == 0)
            {
                dbContext.Products.AddRange(
                    new Product { Name = "Arctis Nova Pro Wireless for PC & PlayStation", Price = 3199, Description = "Dubbla USB-anslutningar stöder PC, Mac, PlayStation, Switch, VR och mer\r\nNova Pro Acoustic System med högupplösta högtalare och Sonar-programvara\r\nAktiv brusreducering (ANC) med transparensläge\r\nVarmbyte mellan två batterier för obegränsad speltid\r\nSamtidig 2,4 GHz och Bluetooth blandar spel- och mobiljud" },
                    new Product { Name = "Arctis Nova 3P Wireless for PlayStation - Black", Price = 1299, Description = "Förinställningar för Call of Duty, Fortnite och mer för att höra fotspår bättre i spelet, styrs via mobilappen\r\nOptimerad snabbladdning ger 9 timmars användning efter bara 15 minuter och upp till 40 timmar när den är fulladdad\r\nKomfort och hållbarhet - väger endast 260 g med en hållbar design med dubbla gångjärn\r\nNeodymium Magnetic Drivers är specialdesignade för att producera kristallklara höjder, exakta mellanregister och djup bas\r\nUSB-C Plug & Play för PlayStation, PC, Switch, Mac, VR, handdatorer, mobiler och mycket mer\r\nQuick-Switch Bluetooth - Tryck på strömbrytaren för att direkt växla mellan mobil och spel" },
                    new Product { Name = "Arctis GameBuds™ for PlayStation", Price = 1999, Description = "Hög hastighet 2,4 GHz trådlöst plus Quick-Switch Bluetooth 5.3\r\nAktiv brusreducering med transparent läge\r\n100+ PS5-ljudinställningar med Arctis Companion-appen för iOS och Android\r\nOmslutande 360° rumsljud på PlayStation, PC och mobil\r\n10 timmars batteritid med Qi trådlöst laddningsfodral för totalt 40 timmar\r\nIP55-skydd - vatten-/damm-/smutsresistent" },
                    new Product { Name = "Arctis Nova 1 for PlayStation - Black", Price = 799, Description = "USB-C Plug & Play för PlayStation, PC, Switch, Mac, VR, handdatorer, mobiler och mycket mer\r\nNeodymium Magnetic Drivers är specialdesignade för att producera kristallklara höjder, exakta mellanregister och djup bas\r\nKomfort och hållbarhet - väger endast 240 g med en hållbar design med dubbla gångjärn\r\nInbyggd volymkontroll och mikrofonavstängning på hörluren\r\nAvtagbar ClearCast-mikrofon med Discord-certifierad kristallklar röstkvalitet" }
                );
                dbContext.SaveChanges();
            }
        }

        internal void RunMenu()
        {
            Console.WriteLine("Välkommen!");
            foreach (var product in dbContext.Products)
            {
                Console.WriteLine($"ID: {product.Id}, Name: {product.Name}, Price: {product.Price}, Description: {product.Description}");
            }
        }
    }
}
