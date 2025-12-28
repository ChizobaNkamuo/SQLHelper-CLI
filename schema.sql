CREATE DATABASE CommoditiesDB;
GO

USE CommoditiesDB;
GO

CREATE TABLE prices (
    id INT IDENTITY(1,1) PRIMARY KEY,
    symbol VARCHAR(15) NOT NULL,
    price_date DATE NOT NULL,
    open_price DECIMAL(10,1),
    high_price DECIMAL(10,1),
    low_price DECIMAL(10,1),
    close_price DECIMAL(10,1),
    volume INT
);
GO

ALTER TABLE prices
ADD CONSTRAINT uq_price UNIQUE (symbol, price_date);
GO

CREATE PROCEDURE GetPrices
    @Symbol VARCHAR(15),
    @From DATE,
    @To DATE
AS
BEGIN
    SELECT
        price_date  AS PriceDate,
        open_price  AS OpenPrice,
        high_price  AS HighPrice,
        low_price   AS LowPrice,
        close_price AS ClosePrice,
        volume      AS Volume
    FROM prices
    WHERE symbol = @Symbol
      AND price_date BETWEEN @From AND @To
    ORDER BY price_date;
END
GO

CREATE PROCEDURE GetLatestPrice
    @Symbol VARCHAR(15)
AS
BEGIN
    SELECT TOP 1 
        price_date  AS PriceDate,
        open_price  AS OpenPrice,
        high_price  AS HighPrice,
        low_price   AS LowPrice,
        close_price AS ClosePrice,
        volume      AS Volume
    FROM prices
    WHERE symbol = @Symbol
    ORDER BY price_date DESC;
END
GO

CREATE PROCEDURE GetHighestPrice
    @Symbol VARCHAR(15),
    @From DATE,
    @To DATE
AS
BEGIN
    SELECT price_date AS PriceDate, MAX(high_price) AS HighestPrice
    FROM prices
    WHERE symbol = @Symbol
      AND price_date BETWEEN @From AND @To;
END
GO

CREATE PROCEDURE GetLowestPrice
    @Symbol VARCHAR(15),
    @From DATE,
    @To DATE
AS
BEGIN
    SELECT price_date AS PriceDate, MIN(low_price) AS LowestPrice
    FROM prices
    WHERE symbol = @Symbol
      AND price_date BETWEEN @From AND @To;
END
GO

CREATE PROCEDURE GetDailyReturns
    @Symbol VARCHAR(15),
    @From DATE,
    @To DATE
AS
BEGIN
    SELECT
        price_date AS PriceDate,
        close_price AS ClosePrice,
        close_price - LAG(close_price) OVER (ORDER BY price_date) AS DailyReturn
    FROM prices
    WHERE symbol = @Symbol
      AND price_date BETWEEN @From AND @To
    ORDER BY price_date;
END
GO

CREATE PROCEDURE InsertData
    @Symbol VARCHAR(15),
    @Date DATE,
    @Open DECIMAL(10,1),
    @High DECIMAL(10,1),
    @Low DECIMAL(10,1),
    @Close DECIMAL(10,1),
    @Volume INT
AS
BEGIN
    INSERT INTO prices
        (symbol, price_date, open_price, high_price, low_price, close_price, volume)
    VALUES
        (@Symbol, @Date, @Open, @High, @Low, @Close, @Volume);
END
GO

CREATE TABLE prices_staging (
    symbol VARCHAR(15),
    price_date VARCHAR(10),
    open_price DECIMAL(10,1),
    high_price DECIMAL(10,1),
    low_price DECIMAL(10,1),
    close_price DECIMAL(10,1),
    volume INT
);
GO

BULK INSERT prices_staging
FROM "C:\Users\chzba\OneDrive\Desktop\Code\SQLHelper\commodity 2000-2022.csv"
WITH (
    FIRSTROW = 2,
    FIELDTERMINATOR = ",",
    ROWTERMINATOR = "\n"
);
GO

INSERT INTO prices (
    symbol,
    price_date,
    open_price,
    high_price,
    low_price,
    close_price,
    volume
)
SELECT
    symbol,
    CONVERT(DATE, price_date, 103),
    open_price,
    high_price,
    low_price,
    close_price,
    volume
FROM prices_staging;
GO