-- TicketPrime Database Schema

CREATE DATABASE ticket_prime;
GO

USE ticket_prime;
GO

CREATE TABLE users (
    id INT PRIMARY KEY IDENTITY(1,1),
    cpf VARCHAR(11) NOT NULL UNIQUE,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE
);
GO

CREATE TABLE events (
    id INT PRIMARY KEY IDENTITY(1,1),
    name VARCHAR(100) NOT NULL,
    total_capacity INT NOT NULL,
    event_date DATETIME NOT NULL,
    default_price DECIMAL(10,2) NOT NULL
);
GO

CREATE TABLE coupons (
    id INT PRIMARY KEY IDENTITY(1,1),
    code VARCHAR(20) NOT NULL UNIQUE,
    discount_percentage DECIMAL(5,2) NOT NULL,
    minimum_amount_rule DECIMAL(10,2) NOT NULL
);
GO

CREATE TABLE reservations (
    id INT PRIMARY KEY IDENTITY(1,1),
    user_id VARCHAR(11) NOT NULL,
    event_id INT NOT NULL,
    coupon_id VARCHAR(20) NULL,
    final_amount_paid DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (user_id) REFERENCES users(id),
    FOREIGN KEY (event_id) REFERENCES events(id),
    FOREIGN KEY (coupon_id) REFERENCES coupons(id)
);
GO
