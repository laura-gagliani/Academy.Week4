create database Ticketing

create table Tickets (
	ID int identity(1,1) primary key,
	Descrizione varchar(500) not null,
	[Data] datetime not null,
	Utente varchar(100),
	Stato varchar(10) check (Stato in ('New', 'OnGoing', 'Resolved'))
	)
