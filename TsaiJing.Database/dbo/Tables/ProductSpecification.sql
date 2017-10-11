CREATE TABLE [dbo].[ProductSpecification] (
    [ProductSpecificationId] INT           IDENTITY (1, 1) NOT NULL,
    [ProductCategoryId]      INT           NOT NULL,
    [Name]                   NVARCHAR (64) NOT NULL,
    CONSTRAINT [PK_ProductSpecification] PRIMARY KEY CLUSTERED ([ProductSpecificationId] ASC),
    CONSTRAINT [FK_ProductSpecification_ProductCategory] FOREIGN KEY ([ProductCategoryId]) REFERENCES [dbo].[ProductCategory] ([ProductCategoryId])
);

