-- Esquema do banco de dados TicketPrime

CREATE DATABASE ticket_prime;
GO

USE ticket_prime;
GO

CREATE TABLE usuarios (
    id INT PRIMARY KEY IDENTITY(1,1),
    cpf VARCHAR(11) NOT NULL UNIQUE,
    nome VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE
);
GO

CREATE TABLE eventos (
    id INT PRIMARY KEY IDENTITY(1,1),
    nome VARCHAR(100) NOT NULL,
    descricao TEXT NOT NULL,
    capacidade_total INT NOT NULL,
    data_evento DATETIME NOT NULL,
    preco_padrao DECIMAL(10,2) NOT NULL,
    local_evento VARCHAR(200) NOT NULL
);
GO

CREATE TABLE cupons (
    id INT PRIMARY KEY IDENTITY(1,1),
    codigo VARCHAR(20) NOT NULL UNIQUE,
    percentual_desconto DECIMAL(5,2) NOT NULL,
    valor_minimo_regra DECIMAL(10,2) NOT NULL
);
GO

CREATE TABLE reservas (
    id INT PRIMARY KEY IDENTITY(1,1),
    usuario_id INT NOT NULL,
    evento_id INT NOT NULL,
    cupom_id INT NULL,
    valor_final_pago DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id),
    FOREIGN KEY (evento_id) REFERENCES eventos(id),
    FOREIGN KEY (cupom_id) REFERENCES cupons(id)
);
GO
