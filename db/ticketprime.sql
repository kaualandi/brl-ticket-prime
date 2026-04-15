-- Esquema do banco de dados TicketPrime

CREATE DATABASE TicketPrime;
GO

USE TicketPrime;
GO

CREATE TABLE Usuarios (
    Cpf       VARCHAR(14)  NOT NULL PRIMARY KEY,
    Nome      VARCHAR(200) NOT NULL,
    Email     VARCHAR(200) NOT NULL UNIQUE,
    Senha     VARCHAR(200) NOT NULL
);
GO

CREATE TABLE Eventos (
    Id              INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nome            VARCHAR(200)      NOT NULL,
    CapacidadeTotal INT               NOT NULL,
    DataEvento      DATETIME          NOT NULL,
    PrecoPadrao     DECIMAL(10,2)     NOT NULL,
    Descricao       VARCHAR(1000)     NOT NULL DEFAULT '',
    LocalEvento     VARCHAR(200)      NOT NULL DEFAULT '',
    ImageUrl        VARCHAR(500)      NOT NULL DEFAULT ''
);
GO

CREATE TABLE Cupons (
    Codigo               VARCHAR(50)   NOT NULL PRIMARY KEY,
    PorcentagemDesconto  DECIMAL(5,2)  NOT NULL,
    ValorMinimoRegra     DECIMAL(10,2) NOT NULL DEFAULT 0
);
GO

CREATE TABLE Reservas (
    Id              INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UsuarioCpf      VARCHAR(14)       NOT NULL,
    EventoId        INT               NOT NULL,
    CupomUtilizado  VARCHAR(50)       NULL,
    ValorFinalPago  DECIMAL(10,2)     NOT NULL,
    FOREIGN KEY (UsuarioCpf)     REFERENCES Usuarios(Cpf),
    FOREIGN KEY (EventoId)       REFERENCES Eventos(Id),
    FOREIGN KEY (CupomUtilizado) REFERENCES Cupons(Codigo)
);
GO
