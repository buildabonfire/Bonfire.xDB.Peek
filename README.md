# Bonfire.Analytics.Dto
This code is currently written for Sitecore 9.0 Initial Release. There is a tag for version 8.2 update 4.

## Use
The analytics DTO exposes the current user's profile data in a JSON format.

## Api
There are two APIs available:

#### VisitorDetails
**Route**: `/Apis/v1/VisitorDetails`
Outputs the aggregation of the user's profile.

#### 
**Route**: `/Apis/v1/ClearVisitorSession`
Clears the user's session is Sitecore. This has the same effect as timing the user's sessions out. This causes the session to be written to Mongo.

## JSON
An example of the output https://gist.github.com/dnstommy/734691e550b2578c4335a292b5ff15ca

## Deploy
Build the project in VS 2015. Copy the App_Config folder to your site. Copy the Bonfire.Analytics.Dto.dll to your bin.

