How to run

Programms: Docker, PowerShell, Visual Studio or Rider, DbVisualizer, DBeaver

Common
	- Download and install Docker
	- Download and install PowerShell
	- Run Docker and open PowerShell and write
	- PowerShell	cd C:\yourPathHere\Hero.Challenge
	- PowerShell	docker-compose up -d
	- For run WebAPI In solution properties, choose "Multiple startup projects:" and for Project:Receiver, WebApi set Action to start.
	- For run MauiMobApp need to Set as Startup Project

Cassandra
	Run database migration for Cassandra 
		- in PowerShell go to base path
		- PowerShell:	database\run_migrations_for_cassandra.ps1
	Run init (if need it)
		- PowerShell:	database\run_init_for_cassandra.ps1
	For see data in DB
		- Download and install DbVisualizer
		- Complite connection tab (see Cassandra Connection.png)

Postgres
	Run database migration
		- in PowerShell go to base path
		- PowerShell:	database\run_migrations_for_postgres.ps1
	Run init (if need it)
		- PowerShell:	database\run_init_for_postgres.ps1
	For see data in DB
		- Download and install DBeaver
		- Complite connection tab (see Postgres Connection.png)

Seq
	Url:		http://localhost:8081/

RabbitMQ
	Url:		http://localhost:15672/
	Username:	"guest"
	Password:	"guest"
