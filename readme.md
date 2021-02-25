# netcore-blazor-mail-reader

A simple mail reader build with Blazor and Syncfusion library
using also :
+ MediatR
+ CQRS pattern
+ Mail api (https://www.limilabs.com/mail)


# Usage
**********************
## appsettings.json
### MailSettings

``` 
"MailSettings": {
    "PopHost": "pop.mail.xx",
    "ImapHost": "imap.mail.xx",
    "UseImap" : true,
    "SmtpHost": "stmp.mail.xx",
    "User": "",
    "Password": "",
    "Folders" : [
      { "Name" : "INBOX",
        "Translate" : "Boîte de réception",
        "Order" : 1
      },
      { "Name" : "Drafts",
        "Translate" : "Brouillons",
        "Order" : 2
      },
      { "Name" : "Sent",
        "Translate" : "Envoyés",
        "Order" : 3
      },
      { "Name" : "Trash",
        "Translate" : "Corbeille",
        "Order" : 4
      },
      { "Name" : "Spam",
        "Translate" : "Spam",
        "Order" : 5
      }
    ]
  } 
``` 
**Remarks**

If UseImap = true, no need to provide popHost

The 'Folders' settings allow to rename imap folders
* Name is the imap original name
* Translate is the new name
* Order allow to sort folders before display

### InterfaceSettings

``` 
"InterfaceSettings": {
    "GridSize": 5,
    "DefaultPage" : 1,
    "syncfusionKey" : ""
  }
``` 
**Remarks**

* GridSize is the number of row by page in the grid
* DefaultPage is the default page of the grid when display
* syncfusionKey is the licence key for blazzor syncfusion components you must provide
