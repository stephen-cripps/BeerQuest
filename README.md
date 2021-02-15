# Overview
This document outlines how I decided to take on the Leeds Beer Quest challenge. First, I'll go over the approach I took to the challenge. Then, I'll cover the design of the application. Finally, I've outlined a potential future roadmap for extending scope and functionality. 

The final part of the document covers more technical aspects of the written code, including an API definition and instructions for running locally. 
# Approach
I wanted to tackle the exercise in a way that would allow me to demonstrate not just my ability to write code, but also to architect a solution and manage a project. To do this I've worked as if I were developing this into a full product, taking a lean, agile approach starting with an extensible MVP and identifying potential future epics.
# Design
## UI
The first thing I designed was the UI. As this is a user facing tool, the UI and UX are key. Having the freedom to design the UI first allows us to create a user-focused experience. The wireframe can be seen here: 
![](./img/listview.png)
![](./img/mapview.png)

I've opted for simple design, taking inspiration from sites such as RightMove, with a list view to allow for sorting data and a map view for a better sense of area. 

For the MVP, there will be two required endpoints. `GET FilterInformation` required to populate the sidebar and `GET Venues` which gets relevant user data to display. Initially, the two views will receive the same set of data and the frontend will handle how to display them. This will allow the users to look though the data without needing frequent API requests to get the next page. There is a risk though of the initial loading of data taking too long, in which case we can update the api to have `GET ListView`, `GET MapView` and `GET Venue`. The list view requesting paginated data and the map view only fetching location data and ID, allowing for the rest to be requested once clicked. This data can be cached to allow for faster loading. 
## Backend
For the backend, I've taken a Domain-Driven Vertical Slice architecture. For the MVP, the domain need only be a simple object model. When more complex functionality is required, this can be extended to a full rich domain. 
I've chosen the vertical slice architecture, because it provides a way to add functionality at a later date with minimal risk of producing side effects. It is not vertical slices in their purest form however, as I still like using an injected repository and having the option to change out data infrastructure at a later date. 

## Infrastructure
An application used to find bars will have to compete with giants such as Google Maps. As such, it probably wouldn't find an immediate niche in the market. Because of this, I've decided to use serverless infrastructure in the form of Azure Functions which provides a straightforward way to host a SPA as well as table storage for the data. 

# Roadmap 
- Epic: MVP - The MVP covers the basic presentation of filterable data to an end user.  
    - Feature: List View
        - Story: Filter Sidebar
        - Story: Venue List
    - Feature: Map View
        - Story: Venue map
        - Story: Map View Filter
- Epic: Metrics - Adding a system to collect user metrics will allow us to see how the product is being used, and shape the future roadmap of the tool. Examples of what we could focus on are where people are when they are using the tool, and popular search filters or areas. 
    - Feature: Metrics Collection
        - Story: Metrics Enablement - Build out the necessary framework to capture metrics
        - Story: GeoLocation Metrics
        - Story: Search Metrics
    - Feature: Metrics Views
        - Story: Metrics Dashboard
- Epic: Contributors & Admins - With an increasing userbase, it may be necessary to add contributors to manage the dataset, and admins to manage those users. 
    - Feature: User Accounts
        - Story: User Auth
        - Story: User Roles
        - Story: User Management
    - Feature: Data Management
        - Story: Create Venues
        - Story: Update Venues
        - Story: Remove venues
    - Feature: ChangeLog
        - Story: Event Sourcing
        - Story: Display Changes    

# Running the Program
## Requirements
- [Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator)
- [Azure Functions Core Tools V3.x](https://github.com/Azure/azure-functions-core-tools)
- Visual Studio 2019
## Initialising Storage
For this exercise, I have taken a shortcut to getting the data into the storage emulator by downloading the [CSV](https://datamillnorth.org/download/leeds-beer-quest/c8884f6c-84a0-4a54-9c71-c5016bf4d878/leedsbeerquest.csv) replacing the heading *name* with *RowKey* and *category* with *PartitionKey*. Then created a table in Azure Storage Emulator with the name *BeerQuest* and directly uploaded the CSV. 
## Debugging
To Run the program, simply run the BeerQuest.Functions project from within Visual Studio
## Api Definition
### GetVenues 
#### Request
```
GET /api/GetVenues
```
#### Query Parameters
Note: All parameters are optional. If a parameter is not sent, that filter is not applied
 Name | Type | Description | Restrictions 
 ---|---|---|---
MinBeer |int| Minimum rating for Beer| between 0 and 5
MinAtmosphere|int|Minimum rating for Atmosphere| between 0 and 5
MinAmenities|int|Minimum rating for Amenities| between 0 and 5
MinValue|int|Minimum rating for Value| between 0 and 5
MinLat|double|Minimum Latitude Value| between -90 and 90, less than MaxLat
MaxLat |double|Maximum Latitude Value|between -90 and 90
MinLng|double|Minimum Longitude Value| between -180 and 180, less than MaxLng
MaxLng|double|Maximun Longitude value|between -180 and 180
Lat|double|Used in conjunction with Lng and MaxDistKm to specify a maximum distance from a coordinate |between -90 and 90
Lng|double|Used in conjunction with Lat and MaxDistKm to specify a maximum distance from a coordinate|between -180 and 180
MaxDistKm|int|Maximum distance in KM allowed from Lat and Lng coordinate| not 0
Category|string|Filters out any venues which do not match this category (ignoring case)|
NameSearch|string|Filters out any venues that who's names don't contain this string|
tags|string|Comma separated tags. Filters out any venues which don't have at least one of these tags|

#### Response Body
```
{
        "name": "",
        "category": "",
        "url": "",
        "date": "",
        "thumbnail": "",
        "lat": 0,
        "lng": 0,
        "address": "",
        "phone": "",
        "twitter": "",
        "beer": 0,
        "atmosphere": 0,
        "amenities": 0,
        "value": 0,
        "tags": [
            ""
        ]
    },
```
# TODO
- Mention Mediator Pattern in Vertical Slices
- Mention Testability 
- Written code demonstrates extensible architecture and a vertical slice
- Add CSV Upload from properties to run on startup
    - Update how partition/Row key is handled
    - Immutability
    - Rich Domain
- Publish to my Azure Account
- GitHub CICD? 