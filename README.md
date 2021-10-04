# Project's development lifecycle:

At start, **Entity Framework Core** with **code first** approach was used, then I migrated to **database first**, later on **EF Core** was entirely removed and I developed my **own custom object-relational mapping framework**.

The project was also initially started with a **MVC architecture**, with all the business logic implemented in the controller levels. After which, a new **Web API** project was initialized with a proper business layer where the business logic was migrated. The **MVC** project send requests to the **API** and retrieved responses in the form of view-models to display in its views.

This was incomplete and the **MVC** architecture was redundant, having to support its controllers (only to make calls to the **API**). I implemented a new web client, under the hood of **Blazor**, using latest **.NET 5**. Now, the new web client was dynamic and was just that - a web client, nothing more.

# What I have learned

* Working with **.NET 5** & **.NET Core 3.1**
* Implementing two web clients - **Blazor** and **MVC**, both consuming data from the **API**
* Working with **Web API**
* Building my **own custom ORM Framework**
* Working with **AutoMapper**
* Incorporating **Facade Design Pattern**
* Further understanding of **Entity Framework Core**
* Further experience with **JavaScript**, **jQuery**, **AJAX** & **JSON**
* Further experience with **generics**, **abstractions**, **reflection** and **asynchronous programming**
* Further experience with **Bootstrap**
* Ability to publish with **Azure**
* Basic work with **SignalR**
* Creating custom logos with **Photoshop**

# Business description

* A platform where housemates keep track and communicate through their apartment/house
* Double authentication process - user & home
* Storage and traceability of inventory, hygiene tasks, chores and issues
* Surveys functionality - grouping questions with answers, applying deadlines and participants status types (started/not started/completed a survey) 
* Metainfo supported for business objects
* Comment section functionality over business objects
* Image storing functionality over business objects
* Participation functionality over business objects
* Home settings
