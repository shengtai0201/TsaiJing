CREATE TABLE [dbo].[Product] (
    [ProductId] INT           IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (64) NOT NULL,
    [Price]     INT           NULL,
    [SafeStock] INT           NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([ProductId] ASC)
);

