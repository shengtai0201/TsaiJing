CREATE TABLE [dbo].[ProductCategory] (
    [ProductCategoryId] INT           IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (64) NOT NULL,
    CONSTRAINT [PK_ProductCategory] PRIMARY KEY CLUSTERED ([ProductCategoryId] ASC)
);

