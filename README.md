# Jobsity .Net Chat Challenge

### Information:
Name: Anthony Del Rosario

Email: adelrosarioh@gmail.com 

### Installation instructions

- Download zip or clone this repository using git
- Install Docker (you can download docker from [here](https://docs.docker.com/get-docker/))
- Open repository folder into terminal
- Execute the following command `docker-compose up -d` to start the services in detached mode 
- or execute `docker-compose up` to see the logs

The installation process should take about 15 minutes depending on your internet connection and computer specs.

### How to Use

After building and running our containers, you can access the chat app in the following address http://localhost:8081 using any internet browser compatible with Angular ([Angular's browser support](https://angular.io/guide/browser-support)). 

If you don't have a user account, go into the register page and create one. After that you should login with your newly created or existing user account and start chatting!.

Multi browser window sessions are supported. If you want to start a different session, create a new browser window (not browser tab) and login.

### Management Tools 
To connect to MSSQL Server 2017 using SQL Management Studio or other tool use the following connection parameters:
- **Server**: localhost,1433
- **User**: SA
- **Password**: P455w0rd123456789

To connect to RabbitMQ Management go to http://localhost:15672/ in your browser and login with the following credentials:
- **Username**: guest
- **Password**: guest

### Shutting down app
You can shutdown all services at once executing `docker-compose down` in your terminal.

### Technologies used

#### Back-End

- **.NET Core 3.1**
    - Used to develop the StockBot Background Service that keeps consuming from the queue.
- **ASP.NET Core 3.1**
    - Used to develop the Web API that handles authentication and chat features.
- **RabbitMQ**
    - Handles the stock prices requests and responses.
- **MSSQL Server 2017**
    - Stores user accounts and messages.
- **NGINX**
    - Host Angular application and serves as http reverse proxy forwarding requests to the Web API.
- **Docker**
    - Glue everything together.

#### Front-End

- **HTML/CSS/JavaScript/TypeScript**
- **Angular**
    - Component based frontend application framework used to develop login, register and chat features.

#### Libraries

- Bootstrap 4
- MomentJS
- ASP.NET Core SignalR
- RabbitMQ .NET Client
- Entity Framework Core
- TinyCsvParser

### Project Architecture

The project was built following  some of the conventions of Onion Architecture. Separating the core functionalities, entities, interfaces of the domain outside of any client. Also have the infrastructure functionalities that are in charge of implementing the core interfaces, services and data handling. Finally the clients, as our web api which depends on the infrastructure implementation. 

Authentication and Message Query requests are handled by an ASPNET Core RESTful API. The chatroom messages are handled by the same service but using a WebSocket connection, provided by ASPNET Core SignalR. If the message handler identifies a command, it's sent to a specific queue of RabbitMQ for asynchronous processing of the consumer. 

The consumer, in our case the StockBot is a .NET Core Console app, that implements the RabbitMQ NET Client, which consumes from the command queue and processes the request and later producing a response and pushing it into a response queue, which our ASPNET Core API is consuming in the background. 

#### Web API RESTful Endpoints
| HTTP Method 	| URI Path           	| Request Headers                                                  	| Request Body                              	| Description                                              	| Response Body                                            	|
|-------------	|--------------------	|------------------------------------------------------------------	|-------------------------------------------	|----------------------------------------------------------	|----------------------------------------------------------	|
| GET         	| /api/messages      	| Authorization Bearer {{ jwt }}<br>Content-Type: application/json 	|                                           	| Returns the last 50 messages ordered by timestamp        	| [{id: '', userName: '', message: '', createdAt: ''}]     	|
| POST        	| /api/auth/register 	| Content-Type: application/json                                   	| { userName: '', email: '', password: '' } 	| Register a new user account                              	| [{ message: '', errors: [{ code: '', description: ''}]}] 	|
| POST        	| /api/auth/login    	| Content-Type: application/json                                   	| { userName: '', password: '' }            	| Authenticate user credentials and return an access token 	| [{ message: '', errors: [{ code: '', description: ''}]]  	|

#### Web API - WebSockets Endpoints
| URI Path   	| Command        	| Arguments                                    	| Query String Parameters 	| Description                          	|
|------------	|----------------	|----------------------------------------------	|-------------------------	|--------------------------------------	|
| /hubs/chat 	| SendMessage    	| { userName: '', message: '' }                	| access_token={{ jwt }}  	| Sends message to WebAPI server       	|
| /hubs/chat 	| ReceiveMessage 	| { userName: '', message: '', createdAt: '' } 	| access_token={{ jwt }}  	| Receives messages from WebAPI server 	|

### DevOps

Docker provides the containerization and glue everything together, but isolates each individual service, thus preventing one service from breaking another. Docker builds our services and guarantees that it works the same every time.

### Requirements

- [x] Allow registered users to log in and talk with other users in a chatroom.
    - Users can register a new account and logging in into the chatroom.

- [x] Allow users to post messages as commands into the chatroom with the following format
/stock=stock_code
    - the /stock command is handled asynchronously, if the app don't understand the command a message is displayed.

- [x] Create a decoupled bot that will call an API using the stock_code as a parameter
(https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv, here aapl.us is the
stock_code)

- [x] The bot should parse the received CSV file and then it should send a message back into
the chatroom using a message broker like RabbitMQ. The message will be a stock quote
using the following format: “APPL.US quote is $93.42 per share”. The post owner will be
the bot.

    - A decoupled NET Core app is consuming from the request queue, calling the stooq web service, parsing CSV response and then pushing that stock information into the queue for any client consumer to do something with it.

- [x] Have the chat messages ordered by their timestamps and show only the last 50
messages.
    - After login and page is refreshed the chatroom displays the last 50 messages ordered by their timestamps.

- [x] Unit test the functionality you prefer.
    - Unit Tests were done using NUnit and Moq libraries and are located in the `BackEnd/JobsityStocksChat/JobsityStocksChat.UnitTests` project.

### Bonuses

- [x] Use .NET identity for users authentication
    - User register and login provided by .NET Core Identity.

- [x] Handle messages that are not understood or any exceptions raised within the bot.
    - If bot didn't find a stock by their code it would say "I'm sorry, I couldn't find the stock your asked for.".

- [x] Build an installer.
    - Docker provides the docker-compose cli which I used to keep the application setup as easy as just installing docker, cloning the repository and running `docker-compose up`. A complete installer would do every step automatically I supposed but, this is close enough.

### Development 

#### Runtime and SDKs

- Download and install Docker (https://docs.docker.com/get-docker/)
- Download and install .NET Core 3.1 SDK (https://dotnet.microsoft.com/download)
- Download and install NodeJS (https://nodejs.org/en/download/)

#### Dependencies
- Start MSSQL Server 2017 Docker container
- Start RabbitMQ Docker container

#### Front-End - WebApp
- Open this repository folder in the terminal and change directory to `FrontEnd/JobsityStocksChat`
- Install project dependencies by running `npm install` command
- Start development server by running `ng serve` command

#### Back-End - WebAPI
- Open this repository folder in the terminal and change directory to `BackEnd/JobsityStocksChat/JobsityStocksChat.WebAPI`
- Install project dependencies by running `dotnet restore "JobsityStocksChat.WebAPI.csproj"` command
- Build project by running `dotnet build --output /app/build` command
- Start project by running `dotnet /app/build/JobsityStocksChat.WebAPI.dll` command

#### Back-End - StocksBot
- Open this repository folder in the terminal and change directory to `BackEnd/JobsityStocksChat/JobsityStocksChat.StocksBot`
- Install project dependencies by running `dotnet restore "JobsityStocksChat.StocksBot.csproj"` command
- Build project by running `dotnet build --output /app/build` command
- Start project by running `dotnet /app/build/JobsityStocksChat.StocksBot.dll` command
