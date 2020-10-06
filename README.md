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

### Technologies used

#### Back-End
- .NET Core 3.1
    - Used to develop the StockBot Background Service that keeps consuming from the queue.
- ASPNET Core 3.1
    - Used to develop the Web API that handles authentication and chat features.
- RabbitMQ
    - Handles the stock prices requests and responses.
- MSSQL Server 2017
    - Stores user accounts and messages.
- NGINX
    - Host Angular application and serves as http reverse proxy forwarding requests to the Web API.
- Docker
    - Glue everything together.

#### Front-End
- HTML/CSS/JavaScript/TypeScript
- Angular
    - Component based frontend application framework used to develop login, register and chat features.

#### Libraries
- Bootstrap 4
- MomentJS
- ASPNET Core SignalR
- RabbitMQ .NET Client
- Entity Framework Core
- TinyCsvParser

### Project Architecture

The project was built following  some of the conventions of Onion Architecture. Separating the core functionalities, entities, interfaces of the domain outside of any client. Also have the infrastructure functionalities that are in charge of implementing the core interfaces, services and data handling. Finally the clients, as our web api which depends on the infrastructure implementation. 

Authentication and Message Query requests are handled by an ASPNET Core RESTful API. The chatroom messages are handled by the same service but using a WebSocket connection, provided by ASPNET Core SignalR. If the message handler identifies a command, it's sent to a specific queue of RabbitMQ for asynchronous processing of the consumer. 

The consumer, in our case the StockBot is a .NET Core Console app, that implements the RabbitMQ NET Client, which consumes from the command queue and processes the request and later producing a response and pushing it into a response queue, which our ASPNET Core API is consuming in the background. 

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

### Bonuses

- [x] Use .NET identity for users authentication
    - User register and login provided by .NET Core Identity.

- [x] Handle messages that are not understood or any exceptions raised within the bot.
    - If bot didn't find a stock by their code it would say "I'm sorry, I couldn't find the stock your asked for.".

- [x] Build an installer.
    - Docker provides the docker-compose cli which I used to keep the application setup as easy as just installing docker, cloning the repository and running `docker-compose up`. A complete installer would do every step automatically I supposed but, this is close enough.
