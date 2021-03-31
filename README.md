# APPartment

APPartment is an **ASP.NET Core MVC** project with an original purpose to serve as a platform where housemates keep track of everything in their apartment/house.
Later on, the purpose changed to an entirely educational one. Throughout the project's development, lots of modifications have been made, for example, at the beginning **Entity Framework Core with code first** approach was used, then I migrated it to **database first**, later **EF Core** was entirely removed and I developed my **own custom object-relational mapping framework with a facade design pattern**.

# What I have learned

* Further experience with **generics**, **abstractions**, **reflection** and **asynchronous programming**
* Ability to publish with **Azure**
* Basic work with **SignalR**
* Creating custom logos with **Photoshop**
* Further understanding of **Entity Framework Core**
* Further experience with **JavaScript**, **jQuery**, **AJAX** and **JSON**
* Building my own custom **ORM Framework**
* Incorporating **Facade Design Pattern**
* Working with **AutoMapper**
* Working with **Web API**

# Further description

* Double authentication process - authentication for user, then authentication for home
* Home presence status, with ability to modify and set custom messages, informing other housemates
* Storage and traceability of inventory, hygiene tasks, chores and issues
* Metadata for all objects. Commenting and image storing functionalities
* Home settings

# Code base

* `APPartment.Web.csproj` - controllers, views, startup, etc.
* `APPartment.API.csproj` - all APIs
* `APPartment.Common.csproj` - configuration
* `APPartment.UI.csproj` - base controllers (check out `BaseCRUDController.cs`), authorization, web services (check out `BaseWebService.cs` and `MapperService.cs`), view models (DTOs), utilites, etc.
* `APPartment.Data.csproj` - facade, server models, etc.
* `APPartment.ORM.Framework.csproj` - `DaoContext.cs`, sql query provider, lambda expression to sql clause translator, declarations, attributes, etc.
* `APPartment.Tests.csproj` - xUnit tests
* `SQL` folder - all scripts in order to build the database
