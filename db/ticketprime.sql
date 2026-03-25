-- TicketPrime Database Schema

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TicketPrime')
BEGIN
    CREATE DATABASE TicketPrime;
END
GO

USE TicketPrime;
GO

-- Eventos
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Eventos')
BEGIN
    CREATE TABLE Eventos (
        Id             INT           NOT NULL IDENTITY(1,1) PRIMARY KEY,
        Nome           VARCHAR(100)  NOT NULL,
        CapacidadeTotal INT          NOT NULL,
        DataEvento     DATETIME      NOT NULL,
        PrecoPadrao    DECIMAL(10,2) NOT NULL
    );
END
GO

-- Seed data for development
INSERT INTO Eventos (Nome, CapacidadeTotal, DataEvento, PrecoPadrao) VALUES
    ('Show Rock in Rio',       50000, '2026-09-20 19:00:00', 350.00),
    ('Peça de Teatro Hamlet',    800, '2026-10-05 20:30:00',  90.00),
    ('Corrida de Fórmula E',   15000, '2026-11-12 14:00:00', 220.00);
GO
