# Jobsity .Net Chat Challenge

## Information:
Name: Anthony Del Rosario

Email: adelrosarioh@gmail.com 

# Table of Contents
1. [Installation instructions](#installation-instructions)
2. [How to Use](#how-to-use)
	* [Management Tools](#management-tools)
	* [Shutting down app](#shutting-down-app)
3. [Technologies used](#technologies-used)
	* [Back-End](#back-end)
	* [Front-End](#front-end)
	* [Libraries](#libraries)
4. [Project Architecture](#project-architecture)
	* [Web API - RESTful Endpoints](###web-api-restful-endpoints)
	* [Web API - WebSockets Endpoints](###web-api-websockets-endpoints)
5. [Requirements](#requirements)
6. [Requirements](#bonuses)
7. [Development](#development)
	* [Runtime and SDKs](#runtime-and-sdks)
	* [Dependencies](#dependencies)
	* [Front-End - WebApp](#front-end-webapp)
	* [Back-End - WebAPI](#back-end-webapi)
	* [Back-End - StocksBot](#back-end-stocksbot)

# Installation instructions

- Download zip or clone this repository using git
- Install Docker (you can download docker from [here](https://docs.docker.com/get-docker/))
- Open repository folder into terminal
- Execute the following command `docker-compose up -d` to start the services in detached mode 
- or execute `docker-compose up` to see the logs

The installation process should take about 15 minutes depending on your internet connection and computer specs.

# How to Use

Click [here](http://localhost:8081) to go into the webapp. You can register a new account in [here](http://localhost:8081/register)
or login [here](http://localhost:8081/login) if you already have one.

You can have multiple user sessions login in from a different browser window (not browser tab).

Make sure you are using one of the following supported browsers:
- Chrome 	latest
- Firefox 	latest and extended support release (ESR) 
- Edge 	2 most recent major versions
- IE 	11

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

# Technologies used

### Back-End

- .NET Core 3.1
- ASP.NET Core 3.1
- RabbitMQ
- MSSQL Server 2017
- NGINX
- Docker

### Front-End

- Angular 10

### Libraries

- Bootstrap 4
- MomentJS
- ASP.NET Core SignalR
- RabbitMQ .NET Client
- Entity Framework Core
- TinyCsvParser

# Project Architecture

The project is separated in three application binaries. The FrontEnd web application developed using the Angular Framework,
The BackEnd ASP.NET Core RESTful API Web API and a .NET Core Console application.

The BackEnd solution structure is divided into the following projects:
- **Core**: defines business entities and interfaces.
- **Infrastructure**: implements service interfaces and handles data storage and message queuing.
- **ASP.NET Core WebAPI**: fullfil frontend requests like, login, register and chat, also keeps listening for incomming stock share price responses from the queue.
- **StockBot Console app**: keeps listening for incomming stock share price requests from queue, requests stock share price from http://stooq.com and 
pushes a message into the stock share price response queue with the parsed data.

The WebAPI has RESTful endpoints for account authentication, account registration and getting the last messages and implements 
WebSocket endpoints to handle real-time chat features using ASP.NET Core SignalR.

The FrontEnd web application is dockerized and served by NGINX which is also used as a reverse proxy, forwarding requests to the Web API.

### Web API - RESTful Endpoints
| HTTP Method 	| URI Path           	| Request Headers                                                  	| Request Body                              	| Description                                              	| Response Body                                            	|
|-------------	|--------------------	|------------------------------------------------------------------	|-------------------------------------------	|----------------------------------------------------------	|----------------------------------------------------------	|
| GET         	| /api/messages      	| Authorization Bearer {{ jwt }}<br>Content-Type: application/json 	|                                           	| Returns the last 50 messages ordered by timestamp        	| [{id: '', userName: '', message: '', createdAt: ''}]     	|
| POST        	| /api/auth/register 	| Content-Type: application/json                                   	| { userName: '', email: '', password: '' } 	| Register a new user account                              	| [{ message: '', errors: [{ code: '', description: ''}]}] 	|
| POST        	| /api/auth/login    	| Content-Type: application/json                                   	| { userName: '', password: '' }            	| Authenticate user credentials and return an access token 	| [{ message: '', errors: [{ code: '', description: ''}]]  	|

### Web API - WebSockets Endpoints
| URI Path   	| Command        	| Arguments                                    	| Query String Parameters 	| Description                          	|
|------------	|----------------	|----------------------------------------------	|-------------------------	|--------------------------------------	|
| /hubs/chat 	| SendMessage    	| { userName: '', message: '' }                	| access_token={{ jwt }}  	| Sends message to WebAPI server       	|
| /hubs/chat 	| ReceiveMessage 	| { userName: '', message: '', createdAt: '' } 	| access_token={{ jwt }}  	| Receives messages from WebAPI server 	|

# Requirements

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
    - Unit Tests were done using NUnit and Moq. Tests are located in the `BackEnd/JobsityStocksChat/JobsityStocksChat.UnitTests` project.

# Bonuses

- [x] Use .NET identity for users authentication
    - User register and login provided by .NET Core Identity.

- [x] Handle messages that are not understood or any exceptions raised within the bot.
    - If the bot didn't find a stock by its stock code it would say "I'm sorry, I couldn't find the stock your asked for.".

- [x] Build an installer.
    - The use `docker-compose` keeps the application setup simple and as closed to deploying the application to a production enviroment as possible.

# Development 

### Runtime and SDKs

- Download and install Docker (https://docs.docker.com/get-docker/)
- Download and install .NET Core 3.1 SDK (https://dotnet.microsoft.com/download)
- Download and install NodeJS (https://nodejs.org/en/download/)

### Dependencies
- Start MSSQL Server 2017 Docker container
- Start RabbitMQ Docker container

### Front-End - WebApp
- Open this repository folder in the terminal and change directory to `FrontEnd/JobsityStocksChat`
- Install project dependencies by running `npm install` command
- Start development server by running `ng serve` command

### Back-End - WebAPI
- Open this repository folder in the terminal and change directory to `BackEnd/JobsityStocksChat/JobsityStocksChat.WebAPI`
- Install project dependencies by running `dotnet restore "JobsityStocksChat.WebAPI.csproj"` command
- Build project by running `dotnet build --output /app/build` command
- Start project by running `dotnet /app/build/JobsityStocksChat.WebAPI.dll` command

### Back-End - StocksBot
- Open this repository folder in the terminal and change directory to `BackEnd/JobsityStocksChat/JobsityStocksChat.StocksBot`
- Install project dependencies by running `dotnet restore "JobsityStocksChat.StocksBot.csproj"` command
- Build project by running `dotnet build --output /app/build` command
- Start project by running `dotnet /app/build/JobsityStocksChat.StocksBot.dll` command
