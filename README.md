
###Features
- Industry Standard Authentication Based On JWT hash token
- Auto Completion Search Keyword Based On EF In Memory Data
- Auto Completion Search Keyword Based On Elasticsearch
- Aggegated Log Statistic Based On Elasticsearch
- Flight Route Map Visualization Based On Google Map
- Kibana Dashboard
- Swagger API Documentation
- Registration And Login
- Alert Notice, Error and Exception
- Pagination


### Technical Points
- .Net Core & Visual Studio Code
>This is sort of technical trends. 
>Even though they have a bit incomvenience for debugging, unit test and so on, those are light and cheap. 
>This gives us and company a good oppotunity to have cost effective projects. 
>Furthermore technically, thesy are actually pretty fancy and rapidly glowing. 
>For example, you as a .Net developer can make some server modules running on IoT device based on Linux.

- Data For DB
>`<link>` : https://openflights.org/data.html 
>I got all data from above organization to develope this assignment. 
>Of course I refined data and it took some time. But the data contains latitude and longitude data so I picked them up to provide routing map gui.

- Structure Of Module & Projects
>It was divided into as many as possible according to the role of each module. 
>In my experiences through several projects, seperation of concerns is the most important factor for maintenance with many different developers. Especially, I think controllers should not contain many logic in itself.

- Rest API
>To be honest, I did not pay much attention to this parts, since most operations are read only. 
>So I used some readable words and names on them unlike Rest API CRUD conventions but it works well. :) 

- Unit Test On Backend
>I just made one simple unit test case for API module with moq and xunit.
>I am sort of unit test believer and so I always create interfaces and injections except dtos.
>I believe every project should cover Sonar Quebe coverage 75%.
>By the way there were some .Net Code version conflict among referenced libraries so I had to specify explicit version number in project file. 
>When you build it successfully, the compiler will complaint it but it will work.

- Elasticsearch
>When I received this assignment email, I decided to use Elasticsearch immediately. 
>As you may know, it has a lot of benefits for flexible search and aggregation and of course scalability. 
>In this project, all log data and flight route informations are stored in it as well as in-memory db.

- Kibana Dashboard
>If someone choosed Elasticsearch, Kibana is next. No doubt. 
>Of course I considered using some other dashboard directly. But I had to select Kibana for practical use.

- Docker
>As a backend guy, I love Docker and Kubernetes.
>But for this project I need only docker to deploy ELK handy.

- In Memory DB
>Regadrless of this project, sometimes I make some in-memory cache in real. When a service is launched, it load data into memory 
>with EF. So we can handle memory data with same manner like other ORMs. 
>While loading data into memory, the service do indexing all data into Elastisearch as mentioned above.

- Integrated Test On Frontend
>I couldn't impelement an integrated test(e2e) on frontend, since there are some version conflicts of nodeJs so I had to give up one of Bonus. 
>But I made one simple unit test on Frontend as a example. You can see 'auth.service.spec.ts' and run Karma.

- Google Map
>I wanted to provide users with some visualized feature. 
>That's why I looked for geograph data. I think it is a useful feature personally.

### Read Me
- Prerequisite
> - .Net Core 2.2.103
> - Docker 18.09.1
> - NodeJs 10.15.1
> - Npm 6.4.1
> - Angular CLI 7.2.3 

- How To Build
> On Angular project, run npm -i to install all dependencies.
> After that, you will be able to build.

- How To Run
> - Make sure the docker service running on your machine 
> - After above, Run the docker image that is in the root in branch following steps
>> - docker network create esnetwork --driver=bridge
>> - docker-compose up 'on the same folder of branch root'
>> - Elasticsearch and Kinbana will be downloaded and run.
> - After running Docker and Kibana, you can run the main service module 'LinkitAir.API' and run 'dotnet run'
> - It will take some time to load data and indexing into Elasticsearch.
> - After that, you can go to 'LinkitAir-SPA' and run 'ng serve' and open 'http://localhost:4200'
> - Thats it!

- How To Use
> - Swagger : http://localhost:5000/swagger/index.html
> - Linkir Air : http://localhost:4200/
> - Kibana Dashboard : http://localhost:5601/
>> - To use Kibana at first, you have to import one file which is in the root branch named 'kibana_dashboard_indexpattern.json'
	You can easily import it. Go to 'Management' menu and Open 'Save Objects' link and you can see 'Import' link button upper side.
	Then you can import the json file. And then you should make the default index pattern. Go to the 'Management' menu again.  
	Click 'Index Pattern' link button. you can see 'logstash' and 'route' which came from the json file already.
	Select 'logstash' and click 'Star' button. That's it. You will be able to one Dashboad with data provided by Linkit Air service.
>> - Of course you have to make sure the range of date for data and you can set it top side menu.
>> You can compar the dashboard data and 'Admin' json page data which you can see on the Linkit Air site.

###End