CREATE TABLE [dbo].[PurchaseDetail] (
    [PurchaseDetailId] INT IDENTITY (1, 1) NOT NULL,
    [PurchaseId]       INT NOT NULL,
    [ProductId]        INT NULL,
    [ProductDetailId]  INT NULL,
    [Price]            INT NOT NULL,
    [Inventory]        INT NOT NULL,
    CONSTRAINT [PK_PurchaseDetail] PRIMARY KEY CLUSTERED ([PurchaseDetailId] ASC),
    CONSTRAINT [FK_PurchaseDetail_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([ProductId]),
    CONSTRAINT [FK_PurchaseDetail_ProductDetail] FOREIGN KEY ([ProductDetailId]) REFERENCES [dbo].[ProductDetail] ([ProductDetailId]),
    CONSTRAINT [FK_PurchaseDetail_Purchase] FOREIGN KEY ([PurchaseId]) REFERENCES [dbo].[Purchase] ([PurchaseId])
);

