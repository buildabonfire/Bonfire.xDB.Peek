# xDB Peek
The master branch is on 9.2, but there is a release branch for each version of Sitecore 9.x. I would like to get the 8.2 branch updated. But it is so far off from 9.x, it a hard lift to maintain both.

## Use
The xDB Peek exposes the current user's profile data in a JSON format and UI.

## Api
There are two APIs available:

## UI
Just visit `VisitorDetails`

![](https://www.waitingimpatiently.com/content/images/2019/08/image.png)

#### VisitorDetails JSON
**Route**: `/Apis/v1/VisitorDetails`
Outputs the aggregation of the user's profile.

####  Clear the session
**Route**: `/Apis/v1/ClearVisitorSession`
Clears the user's session is Sitecore. This has the same effect as timing the user's sessions out. This causes the session to be written to Mongo.

####  Tell Sitecore I am not a robot
**Route**: `/Apis/v1/MakeSessionHuman`
Tells Sitecore that the current session is real. This works well for UI/UX testing where you are checking xConnect data after the session is closed.

## JSON
An example of the output https://gist.github.com/dnstommy/8f8c1d644f0bc4950869375e5141e0b5

## Deploy
Pull the version from releases that matches your version. Install as an update package. 

