DROP TABLE [dbo].[Products];

CREATE TABLE [dbo].[Products]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(MAX) NOT NULL, 
    [Price] FLOAT NOT NULL
);

INSERT INTO [dbo].[Products] values (1, '[DEV] - Foo', 9.99);
INSERT INTO [dbo].[Products] values (2, '[DEV] - Bar', 12.99);
INSERT INTO [dbo].[Products] values (3, '[DEV] - Baz', 4.99);
