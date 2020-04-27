# APPartment

APPartment is an ASP.NET Core MVC project for sharing with your housemate/s and keeping track of everything at the apartment, simplifying the whole process of living with people under a common roof.

# What I have learned

* Further experience with generics
* Further experience with abstractions
* Further experience with asynchronous programming
* Ability to publish project with Azure
* Basic work with SignalR
* Creating custom logos with Photoshop

# Further description

* Web project with double authorization (login, register) - first authorization for user, second authorization for house/home, because one user can have many houses/homes and a home can have many participants/users.
* Top navigation menu with dropdowns for profile, home status with ability to display and set current home status.
* Side menu with home page where widgets with latest information about last updated object for each module are displayed, widget for rent due date with ability to redirect to settings to change rent due date, widget for home status, also chat room using SignalR. Three pages from different modules which inherite similar views, base controller and models. Create, details, edit, mark and delete options in grid view for every object, with metadata sub-object in details in edit pages for every object. Grid view stands for data table, provided from datatables.net, with search, pagination, info and displaying selected number of objects functionalities.
* Metadata for objects stands for comments, images and history - every user can add a comment for every object, attach an image or see its history. History is used in widgets for home page.
* Settings page where users can modify their home's name or change rent due date.

# Database structure

![APPartment DB Structure Image](https://trello-attachments.s3.amazonaws.com/5d531c3be843f538bdff0d0d/5e6bb43f0aefba3e8a28a962/d70cdb6086d01007ac01c6c1712792ca/db-structure-scheme.png)
