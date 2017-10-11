CREATE TABLE [dbo].[ProductDetail] (
    [ProductDetailId]       INT IDENTITY (1, 1) NOT NULL,
    [ProductId]             INT NOT NULL,
    [FirstSpecificationId]  INT NOT NULL,
    [SecondSpecificationId] INT NULL,
    [Price]                 INT NOT NULL,
    [SafeStock]             INT NOT NULL,
    CONSTRAINT [PK_ProductDetail] PRIMARY KEY CLUSTERED ([ProductDetailId] ASC),
    CONSTRAINT [FK_ProductDetail_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([ProductId]),
    CONSTRAINT [FK_ProductDetail_ProductSpecification] FOREIGN KEY ([FirstSpecificationId]) REFERENCES [dbo].[ProductSpecification] ([ProductSpecificationId]),
    CONSTRAINT [FK_ProductDetail_ProductSpecification1] FOREIGN KEY ([SecondSpecificationId]) REFERENCES [dbo].[ProductSpecification] ([ProductSpecificationId])
);

