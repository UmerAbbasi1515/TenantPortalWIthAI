using AIServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIServices.Constants
{

    public class AIAssistantConstant
    {
        public static readonly string SupportContact = "PMS Admin (UBS PMS support team)";
        public static readonly string FileDownloadBaseUrl = "http://localhost:5165";
        public static readonly string DownloadsPhysicalPath = "wwwroot/Downloads/Excel";
        public static readonly string[] InjectionSignals =
       [
           "ignore previous instructions", "ignore prior instructions", "forget everything",
            "ignore all previous", "you are now", "act as a system", "system prompt", "reveal your prompt",
            "disregard the instructions", "override your instructions"
       ];

        public static readonly List<AssistantHelpContentItem> HelpContent =
        [
            new AssistantHelpContentItem
            {
                Key = "create-landlord",
                Title = "Create a new landlord",
                Keywords = ["landlord", "create", "creating", "add", "adding", "new", "make", "making", "register", "registering", "insert", "setup", "set up", "form"],
                Body = @"To create a new landlord, follow these steps:
                     1. Open Setup > Landlord from the main menu to open the Landlord Management page.
                     2. Click Add New to open the landlord creation form.
                     3. Choose the landlord type: Company or Personal.
                     4. Enter the required details:
                        - Company: Company Name (English) and Company Name (Arabic).
                        - Personal: First Name (English and Arabic), plus optional Middle and Last names.
                        - Always provide Mobile, Email, and Emirate.
                     5. Complete the optional fields such as Phone, Fax, Nationality, P.O Box, AddressVAT     details,     and     notes.
                     6. Click Save.
                     7. The system checks the landlord name is not already used, saves the landlord,andopens  the   new       landlord  page.
                     Use the link below to open the landlord creation form directly.",
                ActionText = "Open landlord creation form",
                ActionUrl = "/Setup/Landlord/Create"
            },
            new AssistantHelpContentItem
            {
                Key = "create-property",
                Title = "Create a new property",
                Keywords = ["property", "building", "create", "creating", "add", "adding", "new", "make", "making", "register", "registering", "insert", "setup", "set up", "form"],
                Body = @"To create a new property, follow these steps:
                     1. Open Setup > Property from the main menu to open the Property Management page.
                     2. Click Add New to open the property creation form.
                     3. Enter the required details:
                        - Property Reference.
                        - Select the Landlord.
                        - Property Name (English) and Property Name (Arabic).
                        - Select the Portfolio, Emirate, City, and Category.
                        - Select the Property Type such as commercial or residential.
                        - Enter the Plot Number, Relationship of Property, and Status.
                        - Enter the Property Location (English and Arabic), Road Name, and SectorEnglishand       Arabic).
                     4. Complete the remaining tabs such as Documents, Managers, Insurance, Parking, Bank Account,   and   User      Access   as needed.
                     5. Click Save.
                     6. The system saves the property and opens the new property page.
                     Use the link below to open the property creation form directly.",
                ActionText = "Open property creation form",
                ActionUrl = "/Setup/Property/Create"
            }
        ];
    }
}
